using System;
using System.Data;
using System.IO;
using AzusaERP;
using log4net;
using Npgsql;
using NpgsqlTypes;

namespace myfigurecollectionDumper
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Run(args);
        }


        private NpgsqlConnection connection;
        private ILog logger;
        private MyFigureCollectionClient client;

        private void Run(string[] args)
        {
            FileInfo fi = new FileInfo("azusa.ini");
            if (!fi.Exists)
            {
                Console.WriteLine("azusa.ini not found");
                return;
            }

            Ini ini = new Ini(fi.FullName);
            IniSection pgSection = ini["postgresql"];

            logger = LogManager.GetLogger(GetType());
            logger.Info("MyFigureCollection Dumper, starting up!");

            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "My Figure Collection Dumper";
            ncsb.Host = pgSection["server"];
            ncsb.Username = pgSection["username"];
            ncsb.Password = pgSection["password"];
            ncsb.Database = pgSection["database"];
            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();

            client = new MyFigureCollectionClient();
            
            bool stuffLeft = true;

            while (stuffLeft)
            {
                logger.Info("New Transaction");
                NpgsqlTransaction transaction = connection.BeginTransaction();
                stuffLeft = DumpPass();
                transaction.Commit();
            }
            logger.Info("All done!");
            connection.Close();
            client.Dispose();
        }

        bool DumpPass()
        {
            for (short i = 1; i < 500; i++)
            {
                if (!TestForDumpMetadata("search", "", i))
                {
                    HandleSearchPage(i);
                    SetDumpMetadata("search", "", i);
                    return true;
                }
            }

            DateTime now = DateTime.Now;
            DateTime nowHour = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
            if (!TestForDumpMetadata("hourly", "", nowHour))
            {
                HandleSearchPage(1);
                SetDumpMetadata("hourly", "", nowHour);
                return true;
            }

            int? unscrapedId = FindUnscrapedFigureId();
            if (unscrapedId.HasValue)
            {
                ScrapeImages(unscrapedId.Value);
                return true;
            }

            int numKnownFigures = GetNumKnownFigures();
            for (int i = 1; i < numKnownFigures; i++)
            {
                if (!TestForFigureRow(i))
                {
                    myfigurecollection byId = client.QueryById(i);
                    int contained = Convert.ToInt32(byId.items[0].count);
                    if (contained > 0)
                    {
                        HandleFigure(byId.items[0].item[0]);
                    }
                    else
                    {
                        WriteDisabledFigureRow(i);
                    }
                    return true;
                }
            }

            return false;
        }

        void HandleSearchPage(short i)
        {
            myfigurecollection data = client.GetSearchPage(i);
            foreach (myfigurecollectionItems page in data.items)
            {
                int knownFiguresInDb = GetNumKnownFigures();
                int knownFiguresOnSite = Convert.ToInt32(page.count);
                if (knownFiguresOnSite > knownFiguresInDb)
                {
                    InsertNumKnownFigures(knownFiguresOnSite);
                }
                foreach (myfigurecollectionItemsItem item in page.item)
                {
                    int id = Convert.ToInt32(item.id);
                    if (!TestForFigureRow(id))
                    {
                        HandleFigure(item);
                    }
                }
            }
        }

        private NpgsqlCommand getImageUrls;
        private NpgsqlCommand insertPhotos;
        private NpgsqlCommand markFigureAsScraped;
        void ScrapeImages(int i)
        {
            if (getImageUrls == null)
            {
                getImageUrls = connection.CreateCommand();
                getImageUrls.CommandText =
                    "SELECT thumbnailurl, fullurl FROM dump_myfigurecollection.figures WHERE id=@id";
                getImageUrls.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            getImageUrls.Parameters["@id"].Value = i;
            NpgsqlDataReader dataReader = getImageUrls.ExecuteReader();
            dataReader.Read();
            byte[] thumbnailBuffer = null;
            byte[] fullBuffer = null;
            if (!dataReader.IsDBNull(0))
            {
                thumbnailBuffer = client.DownloadPhoto(dataReader.GetString(0));
            }

            if (!dataReader.IsDBNull(1))
            {
                fullBuffer = client.DownloadPhoto(dataReader.GetString(1));
            }
            dataReader.Dispose();

            if (insertPhotos == null)
            {
                insertPhotos = connection.CreateCommand();
                insertPhotos.CommandText =
                    "INSERT INTO dump_myfigurecollection.figurephotos (id,image,thumbnail) VALUES (@id,@image,@thumbnail)";
                insertPhotos.Parameters.Add("@id", NpgsqlDbType.Integer);
                insertPhotos.Parameters.Add("@image", NpgsqlDbType.Bytea);
                insertPhotos.Parameters.Add("@thumbnail", NpgsqlDbType.Bytea);
            }

            insertPhotos.Parameters["@id"].Value = i;

            insertPhotos.Parameters["@image"].Value = DBNull.Value;
            if (fullBuffer != null)
            {
                insertPhotos.Parameters["@image"].Value = fullBuffer;
            }

            insertPhotos.Parameters["@thumbnail"].Value = DBNull.Value;
            if (thumbnailBuffer != null)
            {
                insertPhotos.Parameters["@thumbnail"].Value = thumbnailBuffer;
            }

            insertPhotos.ExecuteNonQuery();

            if (markFigureAsScraped == null)
            {
                markFigureAsScraped = connection.CreateCommand();
                markFigureAsScraped.CommandText =
                    "UPDATE dump_myfigurecollection.figures SET scraped = TRUE WHERE id=@id";
                markFigureAsScraped.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            markFigureAsScraped.Parameters["@id"].Value = i;

            int updateResult = markFigureAsScraped.ExecuteNonQuery();
            if (updateResult != 1)
                throw new Exception("update result was " + updateResult);
        }

        private NpgsqlCommand findUnscrapedFigure;
        private int? FindUnscrapedFigureId()
        {
            if (findUnscrapedFigure == null)
            {
                findUnscrapedFigure = connection.CreateCommand();
                findUnscrapedFigure.CommandText = "SELECT id FROM dump_myfigurecollection.figures WHERE scraped=FALSE AND enabled=TRUE";
            }

            NpgsqlDataReader dataReader = findUnscrapedFigure.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
                result = dataReader.GetInt32(0);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand writeDisabledFigureRow;
        private void WriteDisabledFigureRow(int id)
        {
            if (writeDisabledFigureRow == null)
            {
                writeDisabledFigureRow = connection.CreateCommand();
                writeDisabledFigureRow.CommandText =
                    "INSERT INTO dump_myfigurecollection.figures (id,enabled) VALUES (@id,FALSE)";
                writeDisabledFigureRow.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            writeDisabledFigureRow.Parameters["@id"].Value = id;
            writeDisabledFigureRow.ExecuteNonQuery();
        }

        private NpgsqlCommand handleFigure;
        private void HandleFigure(myfigurecollectionItemsItem figure)
        {
            if (handleFigure == null)
            {
                handleFigure = connection.CreateCommand();
                handleFigure.CommandText =
                    "INSERT INTO dump_myfigurecollection.figures" +
                    "(id,thumbnailurl,fullurl,rootid,categoryid,barcode,name,release_date,price) " +
                    "VALUES" +
                    "(@id,@thumbnailUrl,@fullUrl,@rootid,@categoryId,@barcode,@name,@releaseDate,@price)";
                handleFigure.Parameters.Add("@id", NpgsqlDbType.Integer);
                handleFigure.Parameters.Add("@thumbnailUrl", NpgsqlDbType.Varchar);
                handleFigure.Parameters.Add("@fullUrl", NpgsqlDbType.Varchar);
                handleFigure.Parameters.Add("@rootid", NpgsqlDbType.Integer);
                handleFigure.Parameters.Add("@categoryId", NpgsqlDbType.Integer);
                handleFigure.Parameters.Add("@barcode", NpgsqlDbType.Varchar);
                handleFigure.Parameters.Add("@name", NpgsqlDbType.Text);
                handleFigure.Parameters.Add("@releaseDate", NpgsqlDbType.Date);
                handleFigure.Parameters.Add("@price", NpgsqlDbType.Double);
            }

            handleFigure.Parameters["@id"].Value = Convert.ToInt32(figure.id);
            handleFigure.Parameters["@thumbnailUrl"].Value = figure.thumbnail;
            handleFigure.Parameters["@fullUrl"].Value = figure.full;

            handleFigure.Parameters["@rootid"].Value = DBNull.Value;
            if (figure.root != null)
            {
                if (figure.root.Length > 0)
                {
                    handleFigure.Parameters["@rootid"].Value = GetRootId(figure.root[0]);
                }
            }

            handleFigure.Parameters["@categoryId"].Value = DBNull.Value;
            if (figure.category != null)
            {
                if (figure.category.Length > 0)
                {
                    handleFigure.Parameters["@categoryId"].Value = GetCategoryId(figure.category[0]);
                }
            }

            handleFigure.Parameters["@barcode"].Value = figure.barcode;
            handleFigure.Parameters["@name"].Value = figure.name;

            try
            {
                handleFigure.Parameters["@releaseDate"].Value = DateTime.Parse(figure.release_date);
            }
            catch (Exception e)
            {
                handleFigure.Parameters["@releaseDate"].Value = DBNull.Value;
                logger.WarnFormat("{0} is not a valid date.", figure.release_date);
            }

            handleFigure.Parameters["@price"].Value = Convert.ToDouble(figure.price);
            handleFigure.ExecuteNonQuery();
        }

        private NpgsqlCommand insertCategory;
        private NpgsqlCommand findCategoryId;
        private int GetCategoryId(myfigurecollectionItemsItemCategory category)
        {
            if (findCategoryId == null)
            {
                findCategoryId = connection.CreateCommand();
                findCategoryId.CommandText = "SELECT id FROM dump_myfigurecollection.categories WHERE name=@name";
                findCategoryId.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            findCategoryId.Parameters["@name"].Value = category.name;
            NpgsqlDataReader dataReader = findCategoryId.ExecuteReader();
            if (dataReader.Read())
            {
                int resultA = dataReader.GetInt32(0);
                dataReader.Dispose();
                return resultA;
            }
            dataReader.Dispose();

            if (insertCategory == null)
            {
                insertCategory = connection.CreateCommand();
                insertCategory.CommandText =
                    "INSERT INTO dump_myfigurecollection.categories (originalid,name,color) VALUES (@oid,@name,@color) RETURNING id";
                insertCategory.Parameters.Add("@oid", NpgsqlDbType.Integer);
                insertCategory.Parameters.Add("@name", NpgsqlDbType.Varchar);
                insertCategory.Parameters.Add("@color", NpgsqlDbType.Varchar);
            }

            insertCategory.Parameters["@oid"].Value = Convert.ToInt32(category.id);
            insertCategory.Parameters["@name"].Value = category.name;
            insertCategory.Parameters["@color"].Value = category.color;
            dataReader = insertCategory.ExecuteReader();
            dataReader.Read();
            int resultB = dataReader.GetInt32(0);
            dataReader.Dispose();
            return resultB;
        }

        private NpgsqlCommand findRootId;
        private NpgsqlCommand insertRoot;
        private int GetRootId(myfigurecollectionItemsItemRoot root)
        {
            if (findRootId == null)
            {
                findRootId = connection.CreateCommand();
                findRootId.CommandText = "SELECT id FROM dump_myfigurecollection.roots WHERE name=@name";
                findRootId.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            findRootId.Parameters["@name"].Value = root.name;
            NpgsqlDataReader dataReader = findRootId.ExecuteReader();
            if (dataReader.Read())
            {
                int result = dataReader.GetInt32(0);
                dataReader.Dispose();
                return result;
            }
            dataReader.Dispose();

            if (insertRoot == null)
            {
                insertRoot = connection.CreateCommand();
                insertRoot.CommandText =
                    "INSERT INTO dump_myfigurecollection.roots (name,originalid) VALUES (@name,@oid) RETURNING id";
                insertRoot.Parameters.Add("@name", NpgsqlDbType.Varchar);
                insertRoot.Parameters.Add("@oid", NpgsqlDbType.Integer);
            }
            insertRoot.Parameters["@name"].Value = root.name;
            insertRoot.Parameters["@oid"].Value = Convert.ToInt32(root.id);
            dataReader = insertRoot.ExecuteReader();
            dataReader.Read();
            int resultB = dataReader.GetInt32(0);
            dataReader.Dispose();
            return resultB;
        }

        private NpgsqlCommand testForFigure;
        private bool TestForFigureRow(int id)
        {
            if (testForFigure == null)
            {
                testForFigure = connection.CreateCommand();
                testForFigure.CommandText =
                    "SELECT dateAdded FROM dump_myfigurecollection.figures WHERE id=@id";
                testForFigure.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            testForFigure.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = testForFigure.ExecuteReader();
            bool known = dataReader.Read();
            dataReader.Dispose();
            return known;
        }

        private NpgsqlCommand insertNumKnownFigures;
        private void InsertNumKnownFigures(int amount)
        {
            if (insertNumKnownFigures == null)
            {
                insertNumKnownFigures = connection.CreateCommand();
                insertNumKnownFigures.CommandText =
                    "INSERT INTO dump_myfigurecollection.\"0statistics\" (numfigures) VALUES (@numfigures)";
                insertNumKnownFigures.Parameters.Add("@numfigures", NpgsqlDbType.Integer);
            }

            insertNumKnownFigures.Parameters["@numfigures"].Value = amount;
            insertNumKnownFigures.ExecuteNonQuery();
        }

        private NpgsqlCommand getNumKnownFigures;
        private int GetNumKnownFigures()
        {
            if (getNumKnownFigures == null)
            {
                getNumKnownFigures = connection.CreateCommand();
                getNumKnownFigures.CommandText = "SELECT MAX(numfigures) FROM dump_myfigurecollection.\"0statistics\"";
            }

            NpgsqlDataReader reader = getNumKnownFigures.ExecuteReader();
            int result = 0;
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    result = reader.GetInt32(0);
                }
            }
            reader.Dispose();
            return result;
        }

        private NpgsqlCommand setMetadata;
        private NpgsqlCommand testForDumpMetadataWithoutDate;
        private NpgsqlCommand testForDumpMetadata;
        private void SetDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
            if (setMetadata == null)
            {
                setMetadata = connection.CreateCommand();
                setMetadata.CommandText = "INSERT INTO dump_myfigurecollection.\"0dumpmeta\" (key1,key2,keyutime) VALUES (@key1,@key2,@keyutime)";
                setMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));

            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";
            if (!keyutime.HasValue)
                keyutime = DateTime.MinValue;

            setMetadata.Parameters["@key1"].Value = key1;
            setMetadata.Parameters["@key2"].Value = key2;
            setMetadata.Parameters["@keyutime"].Value = keyutime.Value.ToUnixTime();
            setMetadata.ExecuteNonQuery();
        }

        private void SetDumpMetadata(string key1, string key2, int keyutime)
        {
            if (setMetadata == null)
            {
                setMetadata = connection.CreateCommand();
                setMetadata.CommandText = "INSERT INTO dump_myfigurecollection.0dumpmeta (key1,key2,keyutime) VALUES (@key1,@key2,@keyutime)";
                setMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                setMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));

            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";

            setMetadata.Parameters["@key1"].Value = key1;
            setMetadata.Parameters["@key2"].Value = key2;
            setMetadata.Parameters["@keyutime"].Value = keyutime;
            setMetadata.ExecuteNonQuery();
        }

        private bool TestForDumpMetadata(string key1, string key2)
        {
            if (testForDumpMetadataWithoutDate == null)
            {
                testForDumpMetadataWithoutDate = connection.CreateCommand();
                testForDumpMetadataWithoutDate.CommandText =
                    "SELECT id FROM dump_myfigurecollection.0dumpmeta WHERE key1=@key1 AND key2=@key2";
                testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadataWithoutDate.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
            }
            if (string.IsNullOrEmpty(key2))
                key2 = "";

            testForDumpMetadataWithoutDate.Parameters["@key1"].Value = key1;
            testForDumpMetadataWithoutDate.Parameters["@key2"].Value = key2;

            NpgsqlDataReader reader = testForDumpMetadataWithoutDate.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

        private bool TestForDumpMetadata(string key1, string key2, Nullable<DateTime> keyutime)
        {
            if (testForDumpMetadata == null)
            {
                testForDumpMetadata = connection.CreateCommand();
                testForDumpMetadata.CommandText =
                    "SELECT id FROM dump_myfigurecollection.0dumpmeta WHERE key1=@key1 AND key2=@key2 AND keyutime=@keyutime";
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }

            if (string.IsNullOrEmpty(key2))
                key2 = "";
            if (!keyutime.HasValue)
                keyutime = DateTime.MinValue;

            testForDumpMetadata.Parameters["@key1"].Value = key1;
            testForDumpMetadata.Parameters["@key2"].Value = key2;
            testForDumpMetadata.Parameters["@keyutime"].Value = keyutime.Value.ToUnixTime();

            NpgsqlDataReader reader = testForDumpMetadata.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }

        private bool TestForDumpMetadata(string key1, string key2, int keyutime)
        {
            if (testForDumpMetadata == null)
            {
                testForDumpMetadata = connection.CreateCommand();
                testForDumpMetadata.CommandText =
                    "SELECT id FROM dump_myfigurecollection.\"0dumpmeta\" WHERE key1=@key1 AND key2=@key2 AND keyutime=@keyutime";
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key1", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@key2", DbType.String));
                testForDumpMetadata.Parameters.Add(new NpgsqlParameter("@keyutime", DbType.Int64));
            }

            if (string.IsNullOrEmpty(key2))
                key2 = "";

            testForDumpMetadata.Parameters["@key1"].Value = key1;
            testForDumpMetadata.Parameters["@key2"].Value = key2;
            testForDumpMetadata.Parameters["@keyutime"].Value = keyutime;

            NpgsqlDataReader reader = testForDumpMetadata.ExecuteReader();
            bool result = reader.Read();
            reader.Close();
            return result;
        }
    }
}
