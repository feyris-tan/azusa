using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzusaERP.DatabaseIO;
using libgrabsiteclient;
using Npgsql;
using NpgsqlTypes;

namespace dbPictureBlobDownscaler
{
    class Program
    {
        static void Main(string[] args)
        {
            GrabSiteSession grabSite = new GrabSiteSession();

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "Azusa DB Picture Blob Downscaler";
            ncsb.CommandTimeout = 60;
            ncsb.Database = args[0];
            ncsb.Host = args[1];
            ncsb.Password = args[2];
            ncsb.Port = Convert.ToInt32(args[3]);
            ncsb.Username = args[4];

            string tableName = args[5];
            string columName = args[6];
            const int maxSize = 100000;

            NpgsqlConnection connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
            NpgsqlDataReader dataReader;

            NpgsqlCommand getCurrentScalerId = connection.CreateCommand();
            getCurrentScalerId.CommandText = String.Format("SELECT MAX(scalerid) FROM {0}", tableName);
            dataReader = getCurrentScalerId.ExecuteReader();
            dataReader.Read();
            int maxScalerId;
            if (dataReader.IsDBNull(0))
                maxScalerId = 0;
            else
                maxScalerId = dataReader.GetInt32(0);
            dataReader.Dispose();
            dataReader.Close();

            NpgsqlCommand updateScalerIdCmd = connection.CreateCommand();
            updateScalerIdCmd.CommandText = String.Format("UPDATE {0} SET scalerId=@scalerId WHERE ctid=@csum", tableName);
            updateScalerIdCmd.Parameters.Add("@scalerId", NpgsqlDbType.Integer);
            updateScalerIdCmd.Parameters.Add("@csum", NpgsqlDbType.Tid);

            NpgsqlCommand selectEmptyScalerId = connection.CreateCommand();
            selectEmptyScalerId.CommandText = String.Format("SELECT CTID FROM {0} WHERE scalerid IS NULL LIMIT 1",tableName);
            while (true)
            {
                dataReader = selectEmptyScalerId.ExecuteReader();
                bool hasData = dataReader.Read();
                if (!hasData)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                    break;
                }
                NpgsqlTid tid = (NpgsqlTid) dataReader.GetValue(0);
                dataReader.Close();
                dataReader.Dispose();

                int scalerId = ++maxScalerId;
                updateScalerIdCmd.Parameters["@scalerId"].Value = scalerId;
                updateScalerIdCmd.Parameters["@csum"].Value = tid;
                int result = updateScalerIdCmd.ExecuteNonQuery();
                if (result != 1)
                    throw new NotImplementedException();
                if (scalerId % 1000 == 0)
                {
                    Console.WriteLine("Setting Scaler IDs... ({0})", scalerId);
                }
            }
            
            
            NpgsqlCommand selectCmd = connection.CreateCommand();
            selectCmd.CommandText = String.Format("SELECT {0}, scalerId FROM {1} WHERE length({0}) > {2} LIMIT 1", columName, tableName, maxSize);

            NpgsqlCommand updateCmd = connection.CreateCommand();
            updateCmd.CommandText = String.Format("UPDATE {0} SET {1}=@scaled WHERE scalerId=@scalerId", tableName,columName);
            updateCmd.Parameters.Add("@scaled", NpgsqlDbType.Bytea);
            updateCmd.Parameters.Add("@scalerId", NpgsqlDbType.Integer);

            while (true)
            {
                dataReader = selectCmd.ExecuteReader();
                bool hasData = dataReader.Read();
                if (!hasData)
                {
                    dataReader.Close();
                    dataReader.Dispose();
                    break;
                }

                byte[] inputData = dataReader.GetByteArray(0);
                int currentScalerId = dataReader.GetInt32(1);
                dataReader.Close();
                dataReader.Dispose();
                Console.Write("Compress {0}", currentScalerId);

                byte[] outputData = inputData;
                MemoryStream inputStream = new MemoryStream(inputData, false);
                Image originalImage = Image.FromStream(inputStream);
                int width = originalImage.Width;
                int height = originalImage.Height;
                Image scaledImage = null;
                MemoryStream outputStream;

                do
                {
                    width = (int) (width * 0.99d);
                    height = (int) (height * 0.99d);
                    scaledImage = originalImage.GetThumbnailImage(width, height, null, IntPtr.Zero);
                    outputStream = new MemoryStream();
                    scaledImage.Save(outputStream, ImageFormat.Jpeg);
                    outputData = outputStream.ToArray();
                    Console.Write(".");
                } while (outputData.Length > maxSize);

                outputStream.Dispose();
                scaledImage.Dispose();
                originalImage.Dispose();
                inputStream.Dispose();

                int percents = (int)(((double) outputData.Length / (double) inputData.Length) * 100.0);

                updateCmd.Parameters["@scaled"].Value = outputData;
                updateCmd.Parameters["@scalerId"].Value = currentScalerId;
                int updateResult = updateCmd.ExecuteNonQuery();
                if (updateResult != 1)
                    throw new Exception("update failed");

                string result = String.Format("OK! - {0} -> {1} ({2}%)", inputData.Length, outputData.Length, percents);
                Console.WriteLine(result);

                grabSite.messageData.bytes_downloaded += (inputData.Length - outputData.Length);
                grabSite.messageData.delay_max = 0;
                grabSite.messageData.delay_min = 0;
                grabSite.messageData.items_downloaded++;
                grabSite.messageData.url = args[5] + "." + args[6];
                grabSite.messageData.video = false;
                grabSite.messageData.concurrency = 1;
                grabSite.messageData.items_queued = maxScalerId - currentScalerId;
                grabSite.SendGrabsiteMessage(String.Format("{0} {1}", currentScalerId, result));
            }


            Console.WriteLine("Completed!");
        }
    }
}
