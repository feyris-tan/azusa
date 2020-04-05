using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AzusaERP;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Licensing;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.dex.Schema.Enums;
using moe.yo3explorer.azusa.DexcomHistory.Entity;
using moe.yo3explorer.azusa.Dumping.Entity;
using moe.yo3explorer.azusa.Gelbooru.Entity;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.MyFigureCollection.Entity;
using moe.yo3explorer.azusa.Notebook.Entity;
using moe.yo3explorer.azusa.Properties;
using moe.yo3explorer.azusa.PsxDatacenter.Entity;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;
using moe.yo3explorer.azusa.VgmDb.Entity;
using moe.yo3explorer.azusa.VnDb.Entity;
using moe.yo3explorer.azusa.VocaDB.Entity;
using moe.yo3explorer.azusa.WarWalking.Entity;
using Npgsql;
using Npgsql.Logging;
using NpgsqlTypes;
using Country = moe.yo3explorer.azusa.MediaLibrary.Entity.Country;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Drivers
{
    class PostgresDriver : IDatabaseDriver
    {
        private NpgsqlConnection connection;
        private NpgsqlTransaction transaction;

        public PostgresDriver()
        {
            NpgsqlLogManager.Provider = new PostgresLogProvider();
            
            IniSection iniSection = AzusaContext.GetInstance().Ini["postgresql"];
            NpgsqlConnectionStringBuilder ncsb = new NpgsqlConnectionStringBuilder();
            ncsb.ApplicationName = "Azusa";
            ncsb.Database = iniSection["database"];
            ncsb.Host = iniSection["server"];
            ncsb.KeepAlive = 30;
            ncsb.Password = iniSection["password"];
            ncsb.Port = Convert.ToUInt16(iniSection["port"]);
            ncsb.Username = iniSection["username"];

            connection = new NpgsqlConnection(ncsb.ToString());
            connection.Open();
        }

        public void SedgeTree_InsertVersion(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public int? SedgeTree_GetLatestVersion()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM sedgetree.versioning";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
            {
                result = dataReader.GetInt32(0);
            }
            dataReader.Dispose();
            dataReader.Close();
            return result;
        }

        public byte[] SedgeTree_GetDataByVersion(int version)
        {
            throw new NotImplementedException();
        }

        public bool SedgeTree_TestForPhoto(string toString)
        {
            throw new NotImplementedException();
        }

        public bool WarWalking_IsTourKnown(long hash)
        {
            throw new NotImplementedException();
        }

        public int WarWalking_InsertTourAndReturnId(long hash, int recordStart, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tour> WarWalking_GetAllTours()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM warwalking.tours";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Tour child = new Tour();
                child.Name = dataReader.GetString(3);
                child.DateAdded = dataReader.GetDateTime(4);
                child.Hash = dataReader.GetInt64(1);
                child.ID = dataReader.GetInt32(0);
                child.RecordingStarted = UnixTimeConverter.FromUnixTime(dataReader.GetInt64(2));
                yield return child;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public bool WarWalking_IsAccessPointKnown(string bssid)
        {
            throw new NotImplementedException();
        }

        public Discovery WarWalking_GetByBssid(string bssid)
        {
            throw new NotImplementedException();
        }

        public void WarWalking_UpdateDiscovery(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public void WarWalking_AddAccessPoint(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Discovery> WarWalking_GetDiscoveriesByTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public byte[] SedgeTree_GetPhotoByPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_UpdatePhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_ErasePhoto(string personId)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_InsertPhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
        }

        public bool ConnectionIsValid()
        {
            if (connection == null)
                return false;

            try
            {
                NpgsqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT version()";
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                    reader.GetString(0);
                reader.Dispose();
                cmd.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetAllPublicTableNames()
        {
            List<string> result = new List<string>();

            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema='public'";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                result.Add(reader.GetString(0));
            reader.Dispose();
            cmd.Dispose();

            return result;
        }

        public List<string> GetAllTableNames()
        {
            List<string> result = new List<string>();

            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT table_schema || '.' || table_name FROM information_schema.tables";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                result.Add(reader.GetString(0));
            reader.Dispose();
            cmd.Dispose();

            return result;
        }

        public List<string> GetAllSchemas()
        {
            List<string> result = new List<string>();

            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT table_schema FROM information_schema.tables";
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                result.Add(reader.GetString(0));
            reader.Dispose();
            cmd.Dispose();

            return result;
        }

        public bool Statistics_TestForDate(DateTime today)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT \"totalProducts\" FROM azusa.statistics WHERE date=@date";
            cmd.Parameters.Add("@date", NpgsqlDbType.Date);
            cmd.Parameters["@date"].Value = today.Date;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            bool result = reader.Read();
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public int SelectCountStarFrom(string tableName)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = String.Format("SELECT COUNT(*) FROM {0}", tableName);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int result = reader.GetInt32(0);
            reader.Dispose();
            cmd.Dispose();
            return result;

        }

        public int Statistics_GetTotalProducts()
        {
            return SelectCountStarFrom("azusa.products");
        }

        public int Statistics_GetTotalMedia()
        {
            return SelectCountStarFrom("azusa.media");
        }

        public int Statistics_GetTotalMissingCovers()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetTotalMissingCovers_Postgre;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int result = reader.GetInt32(0);
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public int Statistics_GetTotalMissingGraphData()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetTotalMissingGraphData_Postgre;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int result = reader.GetInt32(0);
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public int Statistics_GetTotalUndumpedMedia()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetTotalUndumpedMedia_Postgre;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int result = reader.GetInt32(0);
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public int Statistics_GetTotalMissingScreenshots()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetTotalMissingScreenshots_Postgre;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int result = reader.GetInt32(0);
            reader.Dispose();
            cmd.Dispose();
            return result;
        }

        public void Statistics_Insert(DateTime today, int totalProducts, int totalMedia, int missingCover, int missingGraph,
            int undumped, int missingScreenshots)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText =
                "INSERT INTO azusa.statistics\r\n       (date, \"totalProducts\",\"totalMedia\",\"missingCover\",\"missingGraph\",undumped,\"missingScreenshots\")\r\nVALUES (@date,@totalProducts, @totalMedia, @missingCover, @missingGraph,@undumped,@missingScreenshots)";
            cmd.Parameters.Add("@date", NpgsqlDbType.Date);
            cmd.Parameters.Add("@totalProducts", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@totalMedia", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@missingCover", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@missingGraph", NpgsqlDbType.Integer);
            cmd.Parameters.Add("undumped", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@missingScreenshots", NpgsqlDbType.Integer);
            cmd.Parameters["@date"].Value = today.Date;
            cmd.Parameters["@totalProducts"].Value = totalProducts;
            cmd.Parameters["@totalMedia"].Value = totalMedia;
            cmd.Parameters["missingCover"].Value = missingCover;
            cmd.Parameters["@missingGraph"].Value = missingGraph;
            cmd.Parameters["@undumped"].Value = undumped;
            cmd.Parameters["@missingScreenshots"].Value = missingScreenshots;
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<Shop> GetAllShops()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.shops";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Shop child = new Shop();
                child.Id = dataReader.GetInt32(0);
                child.Name = dataReader.GetString(1);
                child.IsPeriodicEvent = dataReader.GetBoolean(2);
                if (!dataReader.IsDBNull(3))
                    child.URL = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    child.DateAdded = dataReader.GetDateTime(4);
                yield return child;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<Shelf> GetAllShelves()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.shelves";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Shelf child = new Shelf();
                child.Id = dataReader.GetInt32(0);
                child.Name = dataReader.GetString(1);
                child.ShowSku = dataReader.GetBoolean(2);
                child.ShowRegion = dataReader.GetBoolean(3);
                child.ShowPlatform = dataReader.GetBoolean(4);
                child.IgnoreForStatistics = dataReader.GetBoolean(5);
                child.ScreenshotRequired = dataReader.GetBoolean(6);
                yield return child;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetProductsInShelf_Postgre;
            cmd.Parameters.Add("@shelf", NpgsqlDbType.Integer);
            cmd.Parameters["@shelf"].Value = shelf.Id;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                ProductInShelf product = new ProductInShelf();
                product.Id = dataReader.GetInt32(0);
                product.Name = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    product.Price = dataReader.GetDouble(2);
                if (!dataReader.IsDBNull(3))
                    product.BoughtOn = dataReader.GetDateTime(3);
                if (!dataReader.IsDBNull(4))
                    product.ScreenshotSize = dataReader.GetInt32(4);
                if (!dataReader.IsDBNull(5))
                    product.CoverSize = dataReader.GetInt32(5);
                if (!dataReader.IsDBNull(6))
                    product.NSFW = dataReader.GetBoolean(6);
                if (!dataReader.IsDBNull(7))
                    product.IconId = dataReader.GetInt32(7);
                product.NumberOfDiscs = dataReader.GetInt32(8);
                product.ContainsUndumped = dataReader.GetInt32(9) > 0;
                product.MissingGraphData = dataReader.GetInt32(10);
                yield return product;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public int CreateProductAndReturnId(Shelf shelf, string name)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO azusa.products (inshelf,name,consistent,complete) VALUES (@inShelf,@name,FALSE,FALSE) RETURNING id";
            cmd.Parameters.Add("@inShelf", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@name", NpgsqlDbType.Varchar);
            cmd.Parameters["@inShelf"].Value = shelf.Id;
            cmd.Parameters["@name"].Value = name;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            int result = dataReader.GetInt32(0);
            dataReader.Dispose();
            cmd.Dispose();
            return result;
        }

        public Product GetProductById(int id)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.products WHERE id = @id";
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer);
            cmd.Parameters["@id"].Value = id;
            NpgsqlDataReader ndr = cmd.ExecuteReader();
            Product result = null;
            if (ndr.Read())
            {
                result = new Product();
                result.Id = ndr.GetInt32(0);
                result.InShelf = ndr.GetInt32(1);
                result.Name = ndr.GetString(2);
                result.Picture = ndr.GetByteArray(3);

                if (!ndr.IsDBNull(4))
                    result.Price = ndr.GetDouble(4);

                if (!ndr.IsDBNull(5))
                    result.BoughtOn = ndr.GetDateTime(5);

                if (!ndr.IsDBNull(6))
                    result.Sku = ndr.GetString(6);

                if (!ndr.IsDBNull(7))
                    result.PlatformId = ndr.GetInt32(7);

                if (!ndr.IsDBNull(8))
                    result.SupplierId = ndr.GetInt32(8);

                if (!ndr.IsDBNull(9))
                    result.CountryOfOriginId = ndr.GetInt32(9);

                result.Screenshot = ndr.GetByteArray(10);

                if (!ndr.IsDBNull(11))
                    result.DateAdded = ndr.GetDateTime(11);

                result.Consistent = ndr.GetBoolean(12);

                if (!ndr.IsDBNull(13))
                    result.NSFW = ndr.GetBoolean(13);

                result.Complete = ndr.GetBoolean(14);
            }
            ndr.Dispose();
            cmd.Dispose();
            return result;
        }

        public void UpdateProduct(Product product)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE azusa.products SET name=@name, price=@price, \"boughtOn\"=@boughtOn, sku=@sku, platform=@platform, " +
                              "supplier=@supplier, " +
                              "\"countryOfOrigin\"=@countryOfOrigin, consistent=@consistent, nsfw=@nsfw, complete=@complete, " +
                              "dateupdated=@dateUpdated " +
                              "WHERE id=@id";
            cmd.Parameters.Add("@name", NpgsqlDbType.Varchar);
            cmd.Parameters.Add("@price", NpgsqlDbType.Double);
            cmd.Parameters.Add("@boughtOn", NpgsqlDbType.Date);
            cmd.Parameters.Add("@sku", NpgsqlDbType.Varchar);
            cmd.Parameters.Add("@platform", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@supplier", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@countryOfOrigin", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@consistent", NpgsqlDbType.Boolean);
            cmd.Parameters.Add("@nsfw", NpgsqlDbType.Boolean);
            cmd.Parameters.Add("@complete", NpgsqlDbType.Boolean);
            cmd.Parameters.Add("@dateUpdated", NpgsqlDbType.Timestamp);
            cmd.Parameters.Add("@id", NpgsqlDbType.Integer);
            cmd.Parameters["@name"].Value = product.Name;
            cmd.Parameters["@price"].Value = product.Price;
            cmd.Parameters["@boughtOn"].Value = product.BoughtOn;
            cmd.Parameters["@sku"].Value = product.Sku;
            cmd.Parameters["@platform"].Value = product.PlatformId;
            cmd.Parameters["@supplier"].Value = product.SupplierId;
            cmd.Parameters["@countryOfOrigin"].Value = product.CountryOfOriginId;
            cmd.Parameters["@consistent"].Value = product.Consistent;
            cmd.Parameters["@nsfw"].Value = product.NSFW;
            cmd.Parameters["@complete"].Value = product.Complete;
            cmd.Parameters["@dateUpdated"].Value = DateTime.Now;
            cmd.Parameters["@id"].Value = product.Id;
            int result = cmd.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("update failed");
        }

        private NpgsqlCommand setCoverCommand;
        public void SetCover(Product product)
        {
            if (setCoverCommand == null)
            {
                setCoverCommand = connection.CreateCommand();
                setCoverCommand.CommandText = "UPDATE azusa.products SET picture = @picture WHERE id=@id";
                setCoverCommand.Parameters.Add("@picture", NpgsqlDbType.Bytea);
                setCoverCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            if (product.Picture != null)
                setCoverCommand.Parameters["@picture"].Value = product.Picture;
            else
                setCoverCommand.Parameters["@picture"].Value = DBNull.Value;

            setCoverCommand.Parameters["@id"].Value = product.Id;
            if (setCoverCommand.ExecuteNonQuery() != 1)
                throw new Exception("update failed");
        }

        private NpgsqlCommand setScreenshotCommand;
        public void SetScreenshot(Product product)
        {
            if (setScreenshotCommand == null)
            {
                setScreenshotCommand = connection.CreateCommand();
                setScreenshotCommand.CommandText = "UPDATE azusa.products SET screenshot = @screenshot WHERE id=@id";
                setScreenshotCommand.Parameters.Add("@screenshot", NpgsqlDbType.Bytea);
                setScreenshotCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            setScreenshotCommand.Parameters["@screenshot"].Value = product.Screenshot;
            setScreenshotCommand.Parameters["@id"].Value = product.Id;
            if (setScreenshotCommand.ExecuteNonQuery() != 1)
                throw new Exception("update failed");
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM azusa.platforms";
            NpgsqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                Platform platform = new Platform();
                platform.Id = dataReader.GetInt32(0);
                platform.ShortName = dataReader.GetString(1);
                platform.LongName = dataReader.GetString(2);
                platform.IsSoftware = dataReader.GetBoolean(3);
                if (!dataReader.IsDBNull(4))
                    platform.DateAdded = dataReader.GetDateTime(4);
                yield return platform;
            }
            dataReader.Dispose();
            command.Dispose();
        }

        public IEnumerable<MediaType> GetMediaTypes()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.mediaTypes";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                MediaType mt = new MediaType();
                mt.Id = dataReader.GetInt32(0);
                mt.ShortName = dataReader.GetString(1);
                mt.LongName = dataReader.GetString(2);
                mt.GraphData = dataReader.GetBoolean(3);
                mt.DateAdded = dataReader.GetDateTime(4);
                mt.Icon = dataReader.GetByteArray(5);
                mt.IgnoreForStatistics = dataReader.GetBoolean(6);
                if (!dataReader.IsDBNull(7))
                    mt.VnDbKey = dataReader.GetString(7);
                mt.HasFilesystem = dataReader.GetBoolean(8);
                yield return mt;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<MediaInProduct> GetMediaByProduct(Product prod)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetMediaByProduct_Postgre;
            cmd.Parameters.Add("@productId", NpgsqlDbType.Integer);
            cmd.Parameters["@productId"].Value = prod.Id;
            NpgsqlDataReader ndr = cmd.ExecuteReader();
            while (ndr.Read())
            {
                MediaInProduct child = new MediaInProduct();
                child.MediaId = ndr.GetInt32(0);
                child.MediaName = ndr.GetString(1);
                child.MediaTypeId = ndr.GetInt32(2);
                yield return child;
            }
            ndr.Dispose();
            cmd.Dispose();
        }

        private NpgsqlCommand getMediaByIdCommand;
        public Media GetMediaById(int o)
        {
            if (getMediaByIdCommand == null)
            {
                getMediaByIdCommand = connection.CreateCommand();
                getMediaByIdCommand.CommandText = "SELECT * FROM azusa.media WHERE id=@id";
                getMediaByIdCommand.Parameters.Add(new NpgsqlParameter("@id", DbType.Int32));
            }

            getMediaByIdCommand.Parameters["@id"].Value = o;
            NpgsqlDataReader ndr = getMediaByIdCommand.ExecuteReader();
            if (!ndr.Read())
            {
                ndr.Dispose();
                return null;
            }
            Media m = new Media();
            m.Id = ndr.GetInt32(0);
            m.RelatedProductId = ndr.GetInt32(1);
            m.Name = ndr.GetString(2);
            m.MediaTypeId = ndr.GetInt32(3);

            if (!ndr.IsDBNull(4))
                m.SKU = ndr.GetString(4);

            if (!ndr.IsDBNull(5))
                m.DumpStorageSpaceId = ndr.GetInt32(5);

            if (!ndr.IsDBNull(6))
                m.DumpStorageSpacePath = ndr.GetString(6);

            if (!ndr.IsDBNull(7))
                m.MetaFileContent = ndr.GetString(7);

            if (!ndr.IsDBNull(8))
                m.DateAdded = ndr.GetDateTime(8);

            if (!ndr.IsDBNull(9))
                m.GraphDataContent = ndr.GetString(9);

            if (!ndr.IsDBNull(10))
                m.CueSheetContent = ndr.GetString(10);

            if (!ndr.IsDBNull(11))
                m.ChecksumContent = ndr.GetString(11);

            if (!ndr.IsDBNull(12))
                m.PlaylistContent = ndr.GetString(12);

            m.CdTextContent = ndr.GetByteArray(13);

            if (!ndr.IsDBNull(14))
                m.LogfileContent = ndr.GetString(14);

            m.MdsContent = ndr.GetByteArray(15);
            m.isSealed = ndr.GetBoolean(16);

            if (!ndr.IsDBNull(17))
                m.DateUpdated = ndr.GetDateTime(17);

            if (!ndr.IsDBNull(18))
                m.FauxHash = ndr.GetInt64(18);

            if (!ndr.IsDBNull(19))
                m.DiscId = ndr.GetInt64(19);

            if (!ndr.IsDBNull(20))
                m.CICM = ndr.GetString(20);

            if (!ndr.IsDBNull(21))
                m.MHddLog = ndr.GetByteArray(21);

            if (!ndr.IsDBNull(22))
                m.ScsiInfo = ndr.GetString(22);

            if (!ndr.IsDBNull(23))
                m.Priv = ndr.GetByteArray(23);

            if (!ndr.IsDBNull(24))
                m.JedecId = ndr.GetByteArray(24);

            ndr.Dispose();
            return m;
        }

        private void ClearAllParameters(NpgsqlCommand updateMediaCommand)
        {
            foreach (NpgsqlParameter parameter in updateMediaCommand.Parameters)
                parameter.Value = DBNull.Value;
        }

        private NpgsqlCommand updateMediaCommand;
        public void UpdateMedia(Media media)
        {
            if (updateMediaCommand == null)
            {
                updateMediaCommand = connection.CreateCommand();
                updateMediaCommand.CommandText = "UPDATE azusa.media " +
                                                 "SET name=@name, mediaTypeId=@mediaTypeId, sku=@sku," +
                                                 "    dumpstoragespace=@dumpstoragespace, dumppath=@dumppath," +
                                                 "    metafile=@metafile, graphdata=@graphdata," +
                                                 "    untouchedcuesheet=@untouchedcuesheet," +
                                                 "    untouchedchecksum=@untouchedchecksum," +
                                                 "    untouchedplaylist=@untouchedplaylist," +
                                                 "    cdtext=@cdtext, logfile=@logfile," +
                                                 "    mediadescriptorsidecar=@mediadescriptorsidecar," +
                                                 "    issealed=@issealed, dateupdated=@dateupdated," +
                                                 "    fauxhash=@fauxhash, discid=@discid," +
                                                 "    cicm=@cicm, mhddlog=@mhddlog, scsiinfo=@scsiinfo, priv=@priv, jedecid=@jedecId " +
                                                 "WHERE id=@id";
                updateMediaCommand.Parameters.Add("@name", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@mediaTypeId", NpgsqlDbType.Integer);
                updateMediaCommand.Parameters.Add("@sku", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@dumpstoragespace", NpgsqlDbType.Integer);
                updateMediaCommand.Parameters.Add("@dumppath", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@metafile", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@graphdata", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@untouchedcuesheet", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@untouchedchecksum", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@untouchedplaylist", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@cdtext", NpgsqlDbType.Bytea);
                updateMediaCommand.Parameters.Add("@logfile", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@mediadescriptorsidecar", NpgsqlDbType.Bytea);
                updateMediaCommand.Parameters.Add("@issealed", NpgsqlDbType.Boolean);
                updateMediaCommand.Parameters.Add("@dateupdated", NpgsqlDbType.Timestamp);
                updateMediaCommand.Parameters.Add("@fauxhash", NpgsqlDbType.Bigint);
                updateMediaCommand.Parameters.Add("@discid", NpgsqlDbType.Bigint);
                updateMediaCommand.Parameters.Add("@cicm", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@mhddlog", NpgsqlDbType.Bytea);
                updateMediaCommand.Parameters.Add("@scsiinfo", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@priv", NpgsqlDbType.Bytea);
                updateMediaCommand.Parameters.Add("@jedecId", NpgsqlDbType.Bytea);
                updateMediaCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            ClearAllParameters(updateMediaCommand);

            updateMediaCommand.Parameters["@name"].Value = media.Name;
            updateMediaCommand.Parameters["@mediaTypeId"].Value = media.MediaTypeId;

            if (!string.IsNullOrEmpty(media.SKU))
                updateMediaCommand.Parameters["@sku"].Value = media.SKU;
            else
                updateMediaCommand.Parameters["@sku"].Value = DBNull.Value;

            if (media.DumpStorageSpaceId != 0)
                updateMediaCommand.Parameters["@dumpstoragespace"].Value = media.DumpStorageSpaceId;

            if (!string.IsNullOrEmpty(media.DumpStorageSpacePath))
                updateMediaCommand.Parameters["@dumppath"].Value = media.DumpStorageSpacePath;

            if (!string.IsNullOrEmpty(media.MetaFileContent))
                updateMediaCommand.Parameters["@metafile"].Value = media.MetaFileContent;

            if (!string.IsNullOrEmpty(media.GraphDataContent))
                updateMediaCommand.Parameters["@graphdata"].Value = media.GraphDataContent;

            if (!string.IsNullOrEmpty(media.CueSheetContent))
                updateMediaCommand.Parameters["@untouchedcuesheet"].Value = media.CueSheetContent;

            if (!string.IsNullOrEmpty(media.ChecksumContent))
                updateMediaCommand.Parameters["@untouchedchecksum"].Value = media.ChecksumContent;

            if (!string.IsNullOrEmpty(media.PlaylistContent))
                updateMediaCommand.Parameters["@untouchedplaylist"].Value = media.PlaylistContent;

            if (media.CdTextContent != null)
                updateMediaCommand.Parameters["@cdtext"].Value = media.CdTextContent;

            if (!string.IsNullOrEmpty(media.LogfileContent))
                updateMediaCommand.Parameters["@logfile"].Value = media.LogfileContent;

            if (media.MdsContent != null)
                updateMediaCommand.Parameters["@mediadescriptorsidecar"].Value = media.MdsContent;
            
            updateMediaCommand.Parameters["@issealed"].Value = media.isSealed;
            updateMediaCommand.Parameters["@dateupdated"].Value = DateTime.Now;
            updateMediaCommand.Parameters["@fauxhash"].Value = media.FauxHash;

            if (media.DiscId.HasValue)
                updateMediaCommand.Parameters["@discid"].Value = media.DiscId.Value;

            if (!string.IsNullOrEmpty(media.CICM))
                updateMediaCommand.Parameters["@cicm"].Value = media.CICM;

            if (media.MHddLog != null)
                updateMediaCommand.Parameters["@mhddlog"].Value = media.MHddLog;

            if (!string.IsNullOrEmpty(media.ScsiInfo))
                updateMediaCommand.Parameters["@scsiinfo"].Value = media.ScsiInfo;

            if (media.Priv != null)
                updateMediaCommand.Parameters["@priv"].Value = media.Priv;

            if (media.JedecId != null)
                updateMediaCommand.Parameters["@jedecId"].Value = media.JedecId;

            updateMediaCommand.Parameters["@id"].Value = media.Id;
            int result = updateMediaCommand.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("unexpected update result");
        }

        public int CreateMediaAndReturnId(int productId, string name)
        {
            int mediaType = GetFirstMediaTypeId();

            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO azusa.media (relatedProduct,name,issealed,mediaTypeId) VALUES (@productId,@name,FALSE,@mediaTypeId) RETURNING id";
            cmd.Parameters.Add("@productId", NpgsqlDbType.Integer);
            cmd.Parameters.Add("@name", NpgsqlDbType.Varchar);
            cmd.Parameters.Add("@mediaTypeId", NpgsqlDbType.Integer);
            cmd.Parameters["@productId"].Value = productId;
            cmd.Parameters["@name"].Value = name;
            cmd.Parameters["@mediaTypeId"].Value = mediaType;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            int result = dataReader.GetInt32(0);
            dataReader.Dispose();
            cmd.Dispose();
            return result;
        }

        public int GetFirstMediaTypeId()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id FROM azusa.mediatypes ORDER BY id ASC LIMIT 1";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            int result = dataReader.GetInt32(0);
            dataReader.Dispose();
            cmd.Dispose();
            return result;
        }

        public IEnumerable<Media> Sync_GetAllMedia()
        {
            throw new NotImplementedException();
        }

        public DateTime Sync_GetLatestCountryDate()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> Sync_GetAllCountriesAfter(DateTime latest)
        {
            throw new NotImplementedException();
        }

        public void Sync_InsertCountry(Country country)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetAllCountries()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.countries";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Country country = new Country();
                country.ID = dataReader.GetInt32(0);
                country.DisplayName = dataReader.GetString(1);
                country.BroadcastTelevisionSystem = dataReader.GetString(2);
                country.CurrencyName = dataReader.GetString(3);
                country.CurrencyConversion = dataReader.GetDouble(4);
                country.LanguageId = dataReader.GetInt32(5);
                if (!dataReader.IsDBNull(6))
                    country.PowerVoltage = dataReader.GetInt16(6);
                if (!dataReader.IsDBNull(7))
                    country.PowerFrequency = dataReader.GetByte(7);
                if (!dataReader.IsDBNull(8))
                    country.DvdRegion = dataReader.GetByte(8);
                if (!dataReader.IsDBNull(9))
                    country.BlurayRegion = dataReader.GetChar(9);
                if (!dataReader.IsDBNull(10))
                    country.DateAdded = dataReader.GetDateTime(10);
                yield return country;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<DateTime> Dexcom_GetDates()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT date FROM dexcom.history ORDER BY date ASC";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                DateTime day = dataReader.GetDateTime(0);
                if (day <= DateTime.Today.Date)
                    yield return day;
            }

            dataReader.Dispose();
            cmd.Dispose();
        }

        private NpgsqlCommand dexcomInsertTimestamp;
        public bool Dexcom_InsertTimestamp(DexTimelineEntry entry)
        {
            if (dexcomInsertTimestamp == null)
            {
                dexcomInsertTimestamp = connection.CreateCommand();
                dexcomInsertTimestamp.CommandText =
                    "INSERT INTO dexcom.history " +
                    "(date, time, filtered, unfiltered, rssi, glucose, trend, \"sessionState\", \"meterGlucose\", \"eventType\", carbs, insulin, \"eventSubType\", \"specialGlucoseValue\") " +
                    "VALUES " +
                    "(@date, @time, @filtered, @unfiltered, @rssi, @glucose, @trend, @sessionState, @meterGlucose, @eventType, @carbs, " +
                    " @insulin, @eventSubType, @specialGlucoseValue)";
                dexcomInsertTimestamp.Parameters.Add("@date", NpgsqlDbType.Date);
                dexcomInsertTimestamp.Parameters.Add("@time", NpgsqlDbType.Time);
                dexcomInsertTimestamp.Parameters.Add("@filtered", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@unfiltered", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@rssi", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@glucose", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@trend", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@sessionState", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@meterGlucose", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@eventType", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@carbs", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@insulin", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@eventSubType", NpgsqlDbType.Integer);
                dexcomInsertTimestamp.Parameters.Add("@specialGlucoseValue", NpgsqlDbType.Integer);
            }

            dexcomInsertTimestamp.Parameters["@date"].Value = entry.Timestamp.Date;
            dexcomInsertTimestamp.Parameters["@time"].Value = entry.Timestamp.TimeOfDay;

            if (entry.SensorFilteredSpecified)
                dexcomInsertTimestamp.Parameters["@filtered"].Value = (int)entry.SensorFiltered.Value;
            else
                dexcomInsertTimestamp.Parameters["@filtered"].Value = DBNull.Value;

            if (entry.SensorUnfilteredSpecified)
                dexcomInsertTimestamp.Parameters["@unfiltered"].Value = (int)entry.SensorUnfiltered.Value;
            else
                dexcomInsertTimestamp.Parameters["@unfiltered"].Value = DBNull.Value;

            if (entry.RssiSpecified)
                dexcomInsertTimestamp.Parameters["@rssi"].Value = (int)entry.Rssi.Value;
            else
                dexcomInsertTimestamp.Parameters["@rssi"].Value = DBNull.Value;

            if (entry.GlucoseSpecified)
                dexcomInsertTimestamp.Parameters["@glucose"].Value = (int)entry.Glucose.Value;
            else
                dexcomInsertTimestamp.Parameters["@glucose"].Value = DBNull.Value;

            if (entry.TrendArrowSpecified)
                dexcomInsertTimestamp.Parameters["@trend"].Value = (int)entry.TrendArrow.Value;
            else
                dexcomInsertTimestamp.Parameters["@trend"].Value = DBNull.Value;

            if (entry.SessionStateSpecified)
                dexcomInsertTimestamp.Parameters["@sessionState"].Value = (int) entry.SessionState.Value;
            else
                dexcomInsertTimestamp.Parameters["@sessionState"].Value = DBNull.Value;

            if (entry.MeterGlucoseSpecified)
                dexcomInsertTimestamp.Parameters["@meterGlucose"].Value = (int)entry.MeterGlucose.Value;
            else
                dexcomInsertTimestamp.Parameters["@meterGlucose"].Value = DBNull.Value;

            if (entry.EventTypeSpecified)
                dexcomInsertTimestamp.Parameters["@eventType"].Value = (int)entry.EventType.Value;
            else
                dexcomInsertTimestamp.Parameters["@eventType"].Value = DBNull.Value;

            dexcomInsertTimestamp.Parameters["@carbs"].Value = DBNull.Value;
            dexcomInsertTimestamp.Parameters["@eventSubType"].Value = DBNull.Value;
            dexcomInsertTimestamp.Parameters["@insulin"].Value = DBNull.Value;
            switch (entry.EventType)
            {
                case EventType.Carbs:
                    dexcomInsertTimestamp.Parameters["@carbs"].Value = entry.Carbs.Value;
                    break;
                case EventType.Exercise:
                    dexcomInsertTimestamp.Parameters["@eventSubType"].Value = (int)entry.ExerciseEvent.Value;
                    break;
                case EventType.Health:
                    dexcomInsertTimestamp.Parameters["@eventSubType"].Value = (int) entry.HealthEvent.Value;
                    break;
                case EventType.Insulin:
                    dexcomInsertTimestamp.Parameters["@insulin"].Value = entry.Insulin;
                    break;
                case null:
                    break;
                default:
                    throw new NotImplementedException(String.Format(entry.EventType.Value.ToString()));
            }

            if (entry.SpecialGlucoseValueSpecified)
                dexcomInsertTimestamp.Parameters["@specialGlucoseValue"].Value = (int) entry.SpecialGlucoseValue.Value;
            else
                dexcomInsertTimestamp.Parameters["@specialGlucoseValue"].Value = DBNull.Value;

            return dexcomInsertTimestamp.ExecuteNonQuery() == 1;
        }

        private NpgsqlCommand testForTimestamp;
        public bool Dexcom_TestForTimestamp(DateTime theDate, DateTime theTime)
        {
            if (testForTimestamp == null)
            {
                testForTimestamp = connection.CreateCommand();
                testForTimestamp.CommandText = "SELECT dateAdded FROM dexcom.history WHERE date=@date AND time=@time";
                testForTimestamp.Parameters.Add("@date", NpgsqlDbType.Date);
                testForTimestamp.Parameters.Add("@time", NpgsqlDbType.Time);
            }

            testForTimestamp.Parameters["@date"].Value = theDate.Date;
            testForTimestamp.Parameters["@time"].Value = theTime.TimeOfDay;
            NpgsqlDataReader dataReader = testForTimestamp.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Close();
            return result;
        }

        public IEnumerable<DexTimelineEntry> Dexcom_GetTimelineEntries(DateTime day)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM dexcom.history WHERE date=@date";
            cmd.Parameters.Add("@date", NpgsqlDbType.Date);
            cmd.Parameters["@date"].Value = day.Date;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                DateTime date = dataReader.GetDateTime(0);
                TimeSpan time = dataReader.GetTimeSpan(1);
                DateTime timestamp = new DateTime(date.Ticks + time.Ticks);
                DexTimelineEntry timelineEntry = new DexTimelineEntry();
                timelineEntry.Timestamp = timestamp;
                if (!dataReader.IsDBNull(2))
                    timelineEntry.SensorFiltered = (uint)dataReader.GetInt32(2);
                if (!dataReader.IsDBNull(3))
                    timelineEntry.SensorUnfiltered = (uint) dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    timelineEntry.Rssi = (uint) dataReader.GetInt32(4);
                if (!dataReader.IsDBNull(5))
                    timelineEntry.Glucose = (uint) dataReader.GetInt32(5);
                if (!dataReader.IsDBNull(6))
                    timelineEntry.TrendArrow = (TrendArrow)dataReader.GetInt16(6);
                if (!dataReader.IsDBNull(7))
                    timelineEntry.SessionState = (SessionState) dataReader.GetInt16(7);
                if (!dataReader.IsDBNull(8))
                    timelineEntry.MeterGlucose = (uint)dataReader.GetInt16(8);
                if (!dataReader.IsDBNull(9))
                    timelineEntry.EventType = (EventType) dataReader.GetInt16(9);
                if (!dataReader.IsDBNull(10))
                    timelineEntry.Carbs = dataReader.GetDouble(10);
                if (!dataReader.IsDBNull(11))
                    timelineEntry.Insulin = dataReader.GetDouble(11);

                if (!dataReader.IsDBNull(12))
                {
                    byte temp = dataReader.GetByte(12);
                    if (timelineEntry.EventType == EventType.Exercise)
                        timelineEntry.ExerciseEvent = (ExerciseSubType) temp;
                    else if (timelineEntry.EventType == EventType.Health)
                        timelineEntry.HealthEvent = (HealthSubType) temp;
                }

                if (!dataReader.IsDBNull(13))
                    timelineEntry.SpecialGlucoseValue = (SpecialGlucoseValue)dataReader.GetInt32(13);
                yield return timelineEntry;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public int MailArchive_GetLatestMessageId()
        {
            throw new NotImplementedException();
        }

        public void MailArchive_StoreMessage(Mail mail)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public Mail MailArchive_GetSpecificMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForFolder(long folderId)
        {
            throw new NotImplementedException();
        }

        public void MailArchive_InsertFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public int MailArchive_CountItemsInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public long MailArchive_GetHighestMessageUTimeInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        private NpgsqlCommand getAllManualCluseValesCommand;
        public IEnumerable<ManualDataEntity> Dexcom_GetAllManualGlucoseValues()
        {
            if (getAllManualCluseValesCommand == null)
            {
                getAllManualCluseValesCommand = connection.CreateCommand();
                getAllManualCluseValesCommand.CommandText = "SELECT * FROM dexcom.manualdata ORDER BY ts ASC";
            }

            NpgsqlDataReader dataReader = getAllManualCluseValesCommand.ExecuteReader();
            while (dataReader.Read())
            {
                ManualDataEntity mde = new ManualDataEntity();
                mde.pid = dataReader.GetInt64(0);
                mde.dateAdded = dataReader.GetDateTime(1);
                mde.ts = dataReader.GetDateTime(2);
                mde.messwert = dataReader.GetInt16(3);
                mde.einheit = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    mde.be = dataReader.GetByte(5);
                if (!dataReader.IsDBNull(6))
                    mde.novorapid = dataReader.GetByte(6);
                if (!dataReader.IsDBNull(7))
                    mde.levemir = dataReader.GetByte(7);
                mde.hide = dataReader.GetBoolean(8);
                mde.minuteCorrection = dataReader.GetInt32(9);
                mde.notice = dataReader.GetString(10);
                yield return mde;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand manualGlucoseValueTestForTimestampCommand;
        public bool Dexcom_ManualGlucoseValueTestForTimestamp(DateTime dt)
        {
            if (manualGlucoseValueTestForTimestampCommand == null)
            {
                manualGlucoseValueTestForTimestampCommand = connection.CreateCommand();
                manualGlucoseValueTestForTimestampCommand.CommandText =
                    "SELECT pid FROM dexcom.manualdata WHERE ts=@ts";
                manualGlucoseValueTestForTimestampCommand.Parameters.Add("@ts", NpgsqlDbType.Timestamp);
            }

            manualGlucoseValueTestForTimestampCommand.Parameters["@ts"].Value = dt;
            NpgsqlDataReader ndr = manualGlucoseValueTestForTimestampCommand.ExecuteReader();
            bool result = ndr.Read();
            ndr.Dispose();
            return result;
        }

        private NpgsqlCommand manualGlucoseValueStoreCommand;
        public void Dexcom_ManualGlucoseValueStore(DateTime timestamp, short value, string unit)
        {
            if (manualGlucoseValueStoreCommand == null)
            {
                manualGlucoseValueStoreCommand = connection.CreateCommand();
                manualGlucoseValueStoreCommand.CommandText = "INSERT INTO dexcom.manualdata (pid,ts,messwert,einheit,hide,minuteModifier,remark) VALUES (@pid,@timestamp,@value,@unit,FALSE,0,'')";
                manualGlucoseValueStoreCommand.Parameters.Add("@timestamp", NpgsqlDbType.Timestamp);
                manualGlucoseValueStoreCommand.Parameters.Add("@value", NpgsqlDbType.Integer);
                manualGlucoseValueStoreCommand.Parameters.Add("@unit", NpgsqlDbType.Varchar);
                manualGlucoseValueStoreCommand.Parameters.Add("@pid", NpgsqlDbType.Bigint);
            }

            manualGlucoseValueStoreCommand.Parameters["@timestamp"].Value = timestamp;
            manualGlucoseValueStoreCommand.Parameters["@value"].Value = value;
            manualGlucoseValueStoreCommand.Parameters["@unit"].Value = unit;
            manualGlucoseValueStoreCommand.Parameters["@pid"].Value = DateTime.Now.Ticks;
            Thread.Sleep(1);
            int result = manualGlucoseValueStoreCommand.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("insert failed");
        }

        public void Dexcom_ManualGlucoseValueUpdate(int id, byte be, byte novorapid, byte levemir, string note)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }

        public bool TransactionSupported { get; }
        public void EndTransaction(bool sucessful)
        {
            if (sucessful)
                transaction.Commit();
            else
                transaction.Rollback();

            transaction.Dispose();
            transaction = null;
        }

        private NpgsqlCommand defineTableCommand;
        public List<DatabaseColumn> Sync_DefineTable(string tableName)
        {
            string[] args = tableName.Split('.');
            if (defineTableCommand == null)
            {
                defineTableCommand = connection.CreateCommand();
                defineTableCommand.CommandText =
                    "SELECT table_name, column_name, udt_name\r\nFROM information_schema.columns\r\n" +
                    "WHERE table_schema = @tableSchema\r\n AND table_name = @tableName";
                defineTableCommand.Parameters.Add("@tableName", NpgsqlDbType.Varchar);
                defineTableCommand.Parameters.Add("@tableSchema", NpgsqlDbType.Varchar);
            }

            defineTableCommand.Parameters["@tableSchema"].Value = args[0];
            defineTableCommand.Parameters["@tableName"].Value = args[1];
            NpgsqlDataReader ndr = defineTableCommand.ExecuteReader();
            List<DatabaseColumn> columns = new List<DatabaseColumn>();
            int ordinal = 0;
            while (ndr.Read())
            {
                DatabaseColumn child = new DatabaseColumn();
                child.Ordingal = ordinal++;
                child.ColumnName = ndr.GetString(1);
                child.DbType = GuessDbType(ndr.GetString(2));
                columns.Add(child);
            }
            ndr.Close();
            ndr.Dispose();
            return columns;
        }

        private DbType GuessDbType(string udtName)
        {
            switch (udtName)
            {
                case "int2":
                    return DbType.Int16;
                case "varchar":
                    return DbType.String;
                case "timestamp":
                    return DbType.DateTime;
                case "int4":
                    return DbType.Int32;
                case "int8":
                    return DbType.Int64;
                case "text":
                    return DbType.String;
                case "bool":
                    return DbType.Boolean;
                case "date":
                    return DbType.Date;
                case "bytea":
                    return DbType.Binary;
                case "float8":
                    return DbType.Double;
                case "time":
                    return DbType.Time;
                case "bpchar":
                    return DbType.StringFixedLength;
                case "inet":
                    return DbType.AnsiString;
                case "oid":
                    return DbType.Int64;
                case "_text":
                    return DbType.String;
                case "name":
                    return DbType.AnsiStringFixedLength;
                case "timestamptz":
                    return DbType.DateTimeOffset;
                case "regproc":
                    return DbType.AnsiString;
                case "char":
                    return DbType.StringFixedLength;
                case "pg_node_tree":
                    return DbType.String;
                case "_aclitem":
                    return DbType.String;
                case "anyarray":
                    return DbType.Object;
                case "_name":
                    return DbType.String;
                case "float4":
                    return DbType.Double;
                case "xid":
                    return DbType.Int64;
                case "_int2":
                    return DbType.Int16;
                case "_oid":
                    return DbType.Int64;
                case "int2vector":
                    return DbType.Object;
                case "oidvector":
                    return DbType.Object;
                case "_regtype":
                    return DbType.Object;
                case "_char":
                    return DbType.StringFixedLength;
                case "pg_lsn":
                    return DbType.Object;
                case "regtype":
                    return DbType.Object;
                case "interval":
                    return DbType.Object;
                case "numeric":
                    return DbType.VarNumeric;
                case "_float4":
                    return DbType.Double;
                case "pg_ndistinct":
                    return DbType.Object;
                case "pg_dependencies":
                    return DbType.Object;
                case "pg_mcv_list":
                    return DbType.Object;
                case "_bool":
                    return DbType.Boolean;
                case "_float8":
                    return DbType.Double;
                default:
                    throw new NotImplementedException(udtName);
            }
        }

        public bool Sync_DoesTableExist(string tableName)
        {
            throw new NotImplementedException();
        }

        public void Sync_CreateTable(string tableName, List<DatabaseColumn> columns)
        {
            throw new NotImplementedException();
        }

        public DateTime? Sync_GetLastSyncDateForTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbDataReader Sync_GetSyncReader(string tableName, DateTime? latestSynced)
        {
            tableName = tableName.MakeFullyQualifiedTableName();
            NpgsqlCommand cmd;
            if (latestSynced == null)
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM {0}", tableName);
                return cmd.ExecuteReader();
            }
            else
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = String.Format("SELECT * FROM {0} WHERE dateAdded > @dateAdded", tableName);
                cmd.Parameters.Add("@dateAdded", NpgsqlDbType.Timestamp);
                cmd.Parameters["@dateAdded"].Value = latestSynced.Value;
                return cmd.ExecuteReader();
            }
        }

        public DbCommand Sync_GetWriteCommand(string tableName, List<DatabaseColumn> columns)
        {
            throw new NotImplementedException();
        }

        public void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage)
        {
            throw new NotImplementedException();
        }

        private NpgsqlCommand checkLicense;
        private NpgsqlCommand insertProbableLicense;
        public LicenseState CheckLicenseStatus(byte[] uid)
        {
            string val = BitConverter.ToString(uid);

            if (checkLicense == null)
            {
                checkLicense = connection.CreateCommand();
                checkLicense.CommandText = "SELECT state FROM licensing.fatclient_machines WHERE uid=@uid";
                checkLicense.Parameters.Add("@uid", NpgsqlDbType.Varchar);
            }

            checkLicense.Parameters["@uid"].Value = val;
            NpgsqlDataReader dataReader = checkLicense.ExecuteReader();
            bool known = dataReader.Read();
            if (known)
            {
                int licenseState = dataReader.GetInt32(0);
                dataReader.Dispose();
                switch (licenseState)
                {
                    case 0:
                        return LicenseState.LicenseNotActivated;
                    case 1:
                        return LicenseState.Valid;
                    default:
                        return LicenseState.UnknownLicenseState;
                }
            }
            else
            {
                dataReader.Dispose();
                if (insertProbableLicense == null)
                {
                    insertProbableLicense = connection.CreateCommand();
                    insertProbableLicense.CommandText = 
                        "INSERT INTO licensing.fatclient_machines " +
                        "(uid,machinename,state,osversion,amd64cpu,amd64task,cpus,systemdir,pagesize," +
                        " userdomainname,username,clrversion,path,debuggee) " +
                        "VALUES " +
                        "(@uid,@machinename,0,@osversion,@amd64cpu,@amd64task,@cpus,@systemdir,@pagesize," +
                        " @userdomainname,@username,@clrversion,@path,@debuggee)";
                    insertProbableLicense.Parameters.Add("@uid", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@machinename", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@osversion", NpgsqlDbType.Text);
                    insertProbableLicense.Parameters.Add("@amd64cpu", NpgsqlDbType.Boolean);
                    insertProbableLicense.Parameters.Add("@amd64task", NpgsqlDbType.Boolean);
                    insertProbableLicense.Parameters.Add("@cpus", NpgsqlDbType.Integer);
                    insertProbableLicense.Parameters.Add("@systemdir", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@pagesize", NpgsqlDbType.Integer);
                    insertProbableLicense.Parameters.Add("@userdomainname", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@username", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@clrversion", NpgsqlDbType.Varchar);
                    insertProbableLicense.Parameters.Add("@path", NpgsqlDbType.Text);
                    insertProbableLicense.Parameters.Add("@debuggee", NpgsqlDbType.Boolean);
                }

                insertProbableLicense.Parameters["@uid"].Value = val;
                insertProbableLicense.Parameters["@machinename"].Value = Environment.MachineName;
                insertProbableLicense.Parameters["@osversion"].Value = Environment.OSVersion.VersionString;
                insertProbableLicense.Parameters["@amd64cpu"].Value = Environment.Is64BitOperatingSystem;
                insertProbableLicense.Parameters["@amd64task"].Value = Environment.Is64BitProcess;
                insertProbableLicense.Parameters["@cpus"].Value = Environment.ProcessorCount;
                insertProbableLicense.Parameters["@systemdir"].Value = Environment.SystemDirectory;
                insertProbableLicense.Parameters["@pagesize"].Value = Environment.SystemPageSize;
                insertProbableLicense.Parameters["@userdomainname"].Value = Environment.UserDomainName;
                insertProbableLicense.Parameters["@username"].Value = Environment.UserName;
                insertProbableLicense.Parameters["@clrversion"].Value = Environment.Version.ToString();
                insertProbableLicense.Parameters["@path"].Value = Environment.GetEnvironmentVariable("PATH");
                insertProbableLicense.Parameters["@debuggee"].Value = Debugger.IsAttached;
                insertProbableLicense.ExecuteNonQuery();
                return LicenseState.MachineHasNoLicense;
            }
        }

        public DateTime? Sync_GetLatestUpdateForTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbDataReader Sync_GetUpdateSyncReader(string tableName, DateTime? latestUpdate)
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = String.Format("SELECT * FROM {0} WHERE dateupdated >= @arg",tableName);
            cmd.Parameters.Add("@arg", NpgsqlDbType.Timestamp);
            if (latestUpdate.HasValue)
                cmd.Parameters["@arg"].Value = latestUpdate.Value;
            else
                cmd.Parameters["@arg"].Value = DateTime.MinValue;
            return cmd.ExecuteReader();
        }

        public void Sync_CopyUpdatesFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage, Queue<object> leftovers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> Notebook_GetAllNotes()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, iscategory, parent, name FROM notebook.notes";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                Note note = new Note();
                note.id = dataReader.GetInt32(0);
                note.isCategory = dataReader.GetBoolean(1);
                if (!dataReader.IsDBNull(2))
                    note.parent = dataReader.GetInt32(2);
                note.name = dataReader.GetString(3);
                yield return note;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand createNote;
        public Note Notebook_CreateNote(string name, bool isCategory, int? parent)
        {
            if (createNote == null)
            {
                createNote = connection.CreateCommand();
                createNote.CommandText = "INSERT INTO notebook_notes (iscategory,parent,name) VALUES (@iscategory,@parent,@name) RETURNING id";
                createNote.Parameters.Add("@iscategory", NpgsqlDbType.Boolean);
                createNote.Parameters.Add("@parent", NpgsqlDbType.Integer);
                createNote.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }
            
            createNote.Parameters["@iscategory"].Value = isCategory;

            if (parent.HasValue)
                createNote.Parameters["@parent"].Value = parent.Value;
            else
                createNote.Parameters["@parent"].Value = DBNull.Value;

            createNote.Parameters["@name"].Value = name;
            
            Note result = new Note();
            result.id = (int)createNote.ExecuteScalar();
            result.isCategory = isCategory;
            result.name = name;
            result.parent = parent;
            return result;
        }

        private NpgsqlCommand notebookGetText;
        public string Notebook_GetRichText(int noteId)
        {
            if (notebookGetText == null)
            {
                notebookGetText = connection.CreateCommand();
                notebookGetText.CommandText = "SELECT richtext FROM notebook.notes WHERE id=@id";
                notebookGetText.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            notebookGetText.Parameters["@id"].Value = noteId;
            NpgsqlDataReader dataReader = notebookGetText.ExecuteReader();
            dataReader.Read();
            string result;
            if (dataReader.IsDBNull(0))
                result = "";
            else
                result = dataReader.GetString(0);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand updateNotebookNote;
        public void Notebook_UpdateNote(int currentNoteId, string text)
        {
            if (updateNotebookNote == null)
            {
                updateNotebookNote = connection.CreateCommand();
                updateNotebookNote.CommandText =
                    "UPDATE notebook_notes SET richText=@richText, dateUpdated=@dateUpdated WHERE id=@id";
                updateNotebookNote.Parameters.Add("@richText", NpgsqlDbType.Text);
                updateNotebookNote.Parameters.Add("@dateUpdated", NpgsqlDbType.Timestamp);
                updateNotebookNote.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            updateNotebookNote.Parameters["@richText"].Value = text;
            updateNotebookNote.Parameters["@dateUpdated"].Value = DateTime.Now;
            updateNotebookNote.Parameters["@id"].Value = currentNoteId;
            if (updateNotebookNote.ExecuteNonQuery() != 1)
                throw new Exception("update failed");
        }

        private NpgsqlCommand vgmdbSearchTrackTranslation;
        public IEnumerable<int> Vgmdb_FindAlbumsByTrackMask(string text)
        {
            if (vgmdbSearchTrackTranslation == null)
            {
                vgmdbSearchTrackTranslation = connection.CreateCommand();
                vgmdbSearchTrackTranslation.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_disc_track_translation WHERE name LIKE @name";
                vgmdbSearchTrackTranslation.Parameters.Add("@name", NpgsqlDbType.Text);
            }

            vgmdbSearchTrackTranslation.Parameters["@name"].Value = text;
            NpgsqlDataReader ndr = vgmdbSearchTrackTranslation.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private NpgsqlCommand vgmFindAlbumsByArbituraryProducts;
        public IEnumerable<int> Vgmdb_FindAlbumsByArbituraryProducts(string text)
        {
            if (vgmFindAlbumsByArbituraryProducts == null)
            {
                vgmFindAlbumsByArbituraryProducts = connection.CreateCommand();
                vgmFindAlbumsByArbituraryProducts.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_arbituaryproducts WHERE name LIKE @name";
                vgmFindAlbumsByArbituraryProducts.Parameters.Add("@name", NpgsqlDbType.Text);
            }

            vgmFindAlbumsByArbituraryProducts.Parameters["@name"].Value = text;
            NpgsqlDataReader ndr = vgmFindAlbumsByArbituraryProducts.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private NpgsqlCommand vgmFindAlbumsByAlbumTitle;
        public IEnumerable<int> Vgmdb_FindAlbumsByAlbumTitle(string text)
        {
            if (vgmFindAlbumsByAlbumTitle == null)
            {
                vgmFindAlbumsByAlbumTitle = connection.CreateCommand();
                vgmFindAlbumsByAlbumTitle.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_titles WHERE title LIKE @name";
                vgmFindAlbumsByAlbumTitle.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            vgmFindAlbumsByAlbumTitle.Parameters["@name"].Value = text;
            NpgsqlDataReader ndr = vgmFindAlbumsByAlbumTitle.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private NpgsqlCommand vgmFindAlbumForList;
        public AlbumListEntry Vgmdb_FindAlbumForList(int id)
        {
            if (vgmFindAlbumForList == null)
            {
                vgmFindAlbumForList = connection.CreateCommand();
                vgmFindAlbumForList.CommandText = Resources.VgmDbFindAlbumForList_Postgre;
                vgmFindAlbumForList.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmFindAlbumForList.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = vgmFindAlbumForList.ExecuteReader();
            AlbumListEntry result = null;
            if (dataReader.Read())
            {
                result = new AlbumListEntry();
                result.id = dataReader.GetInt32(0);
                result.catalog = dataReader.GetString(1);
                if (!dataReader.IsDBNull(2))
                    result.date = dataReader.GetDateTime(2);
                else
                    result.date = null;

                if (!dataReader.IsDBNull(3))
                    result.typeName = dataReader.GetString(3);

                if (!dataReader.IsDBNull(4))
                    result.classificationName = dataReader.GetString(4);

                if (!dataReader.IsDBNull(5))
                    result.mediaformatName = dataReader.GetString(5);

                if (!dataReader.IsDBNull(6))
                    result.name = dataReader.GetString(6);

                if (!dataReader.IsDBNull(7))
                    result.publishformatName = dataReader.GetString(7);

                if (!dataReader.IsDBNull(8))
                    result.notes = dataReader.GetString(8);

                if (!dataReader.IsDBNull(9))
                    result.publisher = dataReader.GetString(9);
            }
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vgmdbGetAlbumCover;
        public Bitmap Vgmdb_GetAlbumCover(int entryId)
        {
            if (vgmdbGetAlbumCover == null)
            {
                vgmdbGetAlbumCover = connection.CreateCommand();
                vgmdbGetAlbumCover.CommandText = "SELECT picture_full FROM dump_vgmdb.albums WHERE id=@id";
                vgmdbGetAlbumCover.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbGetAlbumCover.Parameters["@id"].Value = entryId;
            NpgsqlDataReader dataReader = vgmdbGetAlbumCover.ExecuteReader();
            Bitmap result = null;
            if (dataReader.Read())
                result = dataReader.GetPicture(0);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vgmdbGetArbitraryProductNamesByAlbumId;
        private NpgsqlCommand vgmdbGetProductNamesByAlbumId;
        public IEnumerable<string> Vgmdb_FindProductNamesByAlbumId(int entryId)
        {
            if (vgmdbGetArbitraryProductNamesByAlbumId == null)
            {
                vgmdbGetArbitraryProductNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArbitraryProductNamesByAlbumId.CommandText = "SELECT name FROM dump_vgmdb.album_arbituaryproducts WHERE albumid = @id";
                vgmdbGetArbitraryProductNamesByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);

                vgmdbGetProductNamesByAlbumId = connection.CreateCommand();
                vgmdbGetProductNamesByAlbumId.CommandText =
                    "SELECT prod.name FROM dump_vgmdb.product_albums root JOIN dump_vgmdb.products prod ON root.productid = prod.id WHERE root.albumid = @id";
                vgmdbGetProductNamesByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbGetArbitraryProductNamesByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbGetProductNamesByAlbumId.Parameters["@id"].Value = entryId;

            NpgsqlDataReader dataReader = vgmdbGetArbitraryProductNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetString(0);
            dataReader.Dispose();

            dataReader = vgmdbGetProductNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetString(0);
            dataReader.Dispose();

        }

        private NpgsqlCommand vgmdbGetArbitraryArtistNamesByAlbumId;
        private NpgsqlCommand vgmdbGetArtistNamesByAlbumId;
        public IEnumerable<string> Vgmdb_FindArtistNamesByAlbumId(int entryId)
        {
            if (vgmdbGetArbitraryArtistNamesByAlbumId == null)
            {
                vgmdbGetArbitraryArtistNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArbitraryArtistNamesByAlbumId.CommandText = Resources.VgmdbGetArbitraryArtistsByAlbumId_Postgre;
                vgmdbGetArbitraryArtistNamesByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);

                vgmdbGetArtistNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArtistNamesByAlbumId.CommandText = Resources.VgmdbGetArtistNamesByAlbumId_Postgre;
                vgmdbGetArtistNamesByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbGetArbitraryArtistNamesByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbGetArtistNamesByAlbumId.Parameters["@id"].Value = entryId;

            NpgsqlDataReader dataReader = vgmdbGetArbitraryArtistNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{0} ({1})", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();

            dataReader = vgmdbGetArtistNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{0} ({1})", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindArtistsByName;
        public IEnumerable<int> Vgmdb_FindArtistIdsByName(string escaped)
        {
            if (vgmdbFindArtistsByName == null)
            {
                vgmdbFindArtistsByName = connection.CreateCommand();
                vgmdbFindArtistsByName.CommandText = "SELECT id FROM dump_vgmdb.artist WHERE name LIKE @name";
                vgmdbFindArtistsByName.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            vgmdbFindArtistsByName.Parameters["@name"].Value = escaped;
            NpgsqlDataReader dataReader = vgmdbFindArtistsByName.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindAlbumIdsByArtistId;
        public IEnumerable<int> Vgmdb_FindAlbumIdsByArtistId(int possibleArtist)
        {
            if (vgmdbFindAlbumIdsByArtistId == null)
            {
                vgmdbFindAlbumIdsByArtistId = connection.CreateCommand();
                vgmdbFindAlbumIdsByArtistId.CommandText = "SELECT albumid FROM dump_vgmdb.album_artists WHERE artistid=@id";
                vgmdbFindAlbumIdsByArtistId.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbFindAlbumIdsByArtistId.Parameters["@id"].Value = possibleArtist;
            NpgsqlDataReader dataReader = vgmdbFindAlbumIdsByArtistId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindCoversByAlbumId;
        public IEnumerable<Image> FindCoversByAlbumId(int entryId)
        {
            if (vgmdbFindCoversByAlbumId == null)
            {
                vgmdbFindCoversByAlbumId = connection.CreateCommand();
                vgmdbFindCoversByAlbumId.CommandText = "SELECT buffer FROM dump_vgmdb.album_cover WHERE albumid=@id";
                vgmdbFindCoversByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbFindCoversByAlbumId.Parameters["@id"].Value = entryId;
            NpgsqlDataReader dataReader = vgmdbFindCoversByAlbumId.ExecuteReader();
            while (dataReader.Read())
            {
                Bitmap bmp = dataReader.GetPicture(0);
                if (bmp != null)
                {
                    yield return (Image) bmp;
                }
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindAlbumsBySkuPart;
        public IEnumerable<int> Vgmdb_FindAlbumsBySkuPart(string startswith)
        {
            if (vgmdbFindAlbumsBySkuPart == null)
            {
                vgmdbFindAlbumsBySkuPart = connection.CreateCommand();
                vgmdbFindAlbumsBySkuPart.CommandText = "SELECT id FROM dump_vgmdb.albums WHERE catalog LIKE @key";
                vgmdbFindAlbumsBySkuPart.Parameters.Add("@key", NpgsqlDbType.Varchar);
            }

            vgmdbFindAlbumsBySkuPart.Parameters["@key"].Value = startswith;
            NpgsqlDataReader dataReader = vgmdbFindAlbumsBySkuPart.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindTrackDataByAlbum;
        public IEnumerable<Tuple<string, int, int, string, int>> Vgmdb_FindTrackDataByAlbum(int entryId)
        {
            if (vgmdbFindTrackDataByAlbum == null)
            {
                vgmdbFindTrackDataByAlbum = connection.CreateCommand();
                vgmdbFindTrackDataByAlbum.CommandText = Resources.VgmdbGetTracksByAlbum_Postgre;
                vgmdbFindTrackDataByAlbum.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbFindTrackDataByAlbum.Parameters["@id"].Value = entryId;
            NpgsqlDataReader dataReader = vgmdbFindTrackDataByAlbum.ExecuteReader();
            string currentLanguage = null;
            while (dataReader.Read())
            {
                string discname = dataReader.GetString(0);
                int discIndex = dataReader.GetInt32(1);
                int trackIndex = dataReader.GetInt32(2);
                string trackName = dataReader.GetString(3);
                int trackLength = dataReader.GetInt32(4);
                string language = dataReader.GetString(5);
                if (currentLanguage == null)
                    currentLanguage = language;
                else if (!currentLanguage.Equals(language))
                    break;
                yield return new Tuple<string, int, int, string, int>(discname, discIndex, trackIndex, trackName, trackLength);
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindArbituraryLabelsByAlbumId, vgmdbFindLabelsByAlbumId;
        public IEnumerable<string> Vgmdb_FindLabelNamesByAlbumId(int entryId)
        {
            if (vgmdbFindArbituraryLabelsByAlbumId == null)
            {
                vgmdbFindArbituraryLabelsByAlbumId = connection.CreateCommand();
                vgmdbFindArbituraryLabelsByAlbumId.CommandText = Resources.VgmdbFindArbituraryLabelNamesByAlbumId_Postgre;
                vgmdbFindArbituraryLabelsByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);

                vgmdbFindLabelsByAlbumId = connection.CreateCommand();
                vgmdbFindLabelsByAlbumId.CommandText = Resources.VgmdbFindLabelsByAlbumId_Postgre;
                vgmdbFindLabelsByAlbumId.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vgmdbFindArbituraryLabelsByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbFindLabelsByAlbumId.Parameters["@id"].Value = entryId;

            NpgsqlDataReader dataReader = vgmdbFindArbituraryLabelsByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{1}: {0}", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();

            dataReader = vgmdbFindLabelsByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{1}: {0}", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbFindRelatedAlbumsCommand;
        public IEnumerable<string> Vgmdb_FindRelatedAlbums(int albumId)
        {
            if (vgmdbFindRelatedAlbumsCommand == null)
            {
                vgmdbFindRelatedAlbumsCommand = connection.CreateCommand();
                vgmdbFindRelatedAlbumsCommand.CommandText = Resources.VgmdbGetRelatedAlbums_Postgre;
                vgmdbFindRelatedAlbumsCommand.Parameters.Add("@albumid", NpgsqlDbType.Integer);
            }

            vgmdbFindRelatedAlbumsCommand.Parameters["@albumid"].Value = albumId;
            NpgsqlDataReader dataReader = vgmdbFindRelatedAlbumsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string sku = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                yield return String.Format("{0}, {1}", sku, name);
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbGetReleaseEvent;
        public string Vgmdb_GetReleaseEvent(int albumId)
        {
            if (vgmdbGetReleaseEvent == null)
            {
                vgmdbGetReleaseEvent = connection.CreateCommand();
                vgmdbGetReleaseEvent.CommandText = Resources.VgmDbGetReleaseEvent;
                vgmdbGetReleaseEvent.Parameters.Add("@albumid", NpgsqlDbType.Integer);
            }

            vgmdbGetReleaseEvent.Parameters["@albumid"].Value = albumId;
            NpgsqlDataReader dataReader = vgmdbGetReleaseEvent.ExecuteReader();
            string result = null;
            if (dataReader.Read())
                if (!dataReader.IsDBNull(0))
                    result = dataReader.GetString(0);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vgmdbFindReprintCommand;
        public IEnumerable<string> Vgmdb_FindReprints(int albumId)
        {
            if (vgmdbFindReprintCommand == null)
            {
                vgmdbFindReprintCommand = connection.CreateCommand();
                vgmdbFindReprintCommand.CommandText = Resources.VgmdbGetReprints_Postgre;
                vgmdbFindReprintCommand.Parameters.Add("@albumid", NpgsqlDbType.Integer);
            }

            vgmdbFindReprintCommand.Parameters["@albumid"].Value = albumId;
            NpgsqlDataReader dataReader = vgmdbFindReprintCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string sku = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                yield return String.Format("{0}, {1}", sku, name);
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand vgmdbGetWebsitesCommand;
        public IEnumerable<Uri> Vgmdb_GetWebsites(int albumId)
        {
            if (vgmdbGetWebsitesCommand == null)
            {
                vgmdbGetWebsitesCommand = connection.CreateCommand();
                vgmdbGetWebsitesCommand.CommandText = "SELECT link FROM dump_vgmdb.album_websites WHERE albumid=@albumid";
                vgmdbGetWebsitesCommand.Parameters.Add("@albumid", NpgsqlDbType.Integer);
            }

            vgmdbGetWebsitesCommand.Parameters["@albumid"].Value = albumId;
            NpgsqlDataReader dataReader = vgmdbGetWebsitesCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string url = dataReader.GetString(0);
                if (!url.StartsWith("http"))
                    continue;
                Uri uri = new Uri(url);
                yield return uri;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand psxdcSearchCommand;
        public IEnumerable<PsxDatacenterPreview> PsxDc_Search(string textBox)
        {
            if (psxdcSearchCommand == null)
            {
                psxdcSearchCommand = connection.CreateCommand();
                psxdcSearchCommand.CommandText =
                    "SELECT id, platform,sku,title,additionals FROM dump_psxdatacenter.games WHERE title LIKE @p1 OR commontitle LIKE @p1 OR sku LIKE @p2";
                psxdcSearchCommand.Parameters.Add("@p1", NpgsqlDbType.Varchar);
                psxdcSearchCommand.Parameters.Add("@p2", NpgsqlDbType.Varchar);
            }

            psxdcSearchCommand.Parameters["@p1"].Value = String.Format("%{0}%", textBox);
            psxdcSearchCommand.Parameters["@p2"].Value = String.Format("{0}%", textBox);
            NpgsqlDataReader dataReader = psxdcSearchCommand.ExecuteReader();
            while (dataReader.Read())
            {
                PsxDatacenterPreview child = new PsxDatacenterPreview();
                child.Id = dataReader.GetInt32(0);
                child.Platform = dataReader.GetString(1);
                child.SKU = dataReader.GetString(2);
                child.Title = dataReader.GetString(3);
                child.HasAdditionalData = dataReader.GetBoolean(4);
                yield return child;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand psxdatacenterGetGameCommand;
        public PsxDatacenterGame PsxDc_GetSpecificGame(int previewId)
        {
            if (psxdatacenterGetGameCommand == null)
            {
                psxdatacenterGetGameCommand = connection.CreateCommand();
                psxdatacenterGetGameCommand.CommandText = Resources.PsxDatacenterGetGame_Postgre;
                psxdatacenterGetGameCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            psxdatacenterGetGameCommand.Parameters["@id"].Value = previewId;
            NpgsqlDataReader dataReader = psxdatacenterGetGameCommand.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Dispose();
                return null;
            }
            PsxDatacenterGame result = new PsxDatacenterGame();
            result.Platform = dataReader.GetString(0);
            result.SKU = dataReader.GetString(1);
            result.Title = dataReader.GetString(2);
            result.Languages = dataReader.GetString(3);
            result.CommonTitle = dataReader.GetString(4);
            result.Region = dataReader.GetString(5);
            result.Genre = dataReader.GetString(6);
            result.DeveloperId = dataReader.GetString(7);
            result.PublisherId = dataReader.GetString(8);
            result.DateRelease = dataReader.GetDateTime(9);
            result.Cover = dataReader.GetByteArray(10);
            result.Description = dataReader.GetString(11);
            result.Barcode = dataReader.GetString(12);
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand psxDatacenterGetScreenshots;
        public IEnumerable<byte[]> PsxDc_GetScreenshots(int previewId)
        {
            if (psxDatacenterGetScreenshots == null)
            {
                psxDatacenterGetScreenshots = connection.CreateCommand();
                psxDatacenterGetScreenshots.CommandText =
                    "SELECT screenshot.buffer FROM dump_psxdatacenter.game_screenshots root JOIN dump_psxdatacenter.screenshots screenshot ON root.screenshotid = screenshot.id WHERE root.gameid=@id";
                psxDatacenterGetScreenshots.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            psxDatacenterGetScreenshots.Parameters["@id"].Value = previewId;
            NpgsqlDataReader dataReader = psxDatacenterGetScreenshots.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetByteArray(0);
            dataReader.Dispose();
        }

        public IEnumerable<SqlIndex> GetSqlIndexes()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM pg_catalog.pg_indexes WHERE schemaname != 'pg_catalog'";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                SqlIndex child = new SqlIndex();
                child.Timestamp = DateTime.Now;
                child.TableName = dataReader.GetString(1);
                child.IndexName = dataReader.GetString(2);
                child.SchemaName = dataReader.GetString(0);

                string definition = dataReader.GetString(4);
                definition = definition.Replace("\"", "");
                if (definition.Contains("CREATE UNIQUE INDEX"))
                    child.Unique = true;

                int substringStart = definition.IndexOf('(') + 1;
                int substringEnd = definition.IndexOf(')');
                substringEnd -= substringStart;

                string columnsBody = definition.Substring(substringStart, substringEnd);
                string[] columns = columnsBody.Split(',');
                child.Columns = new string[columns.Length];
                for (int i = 0; i < columns.Length; i++)
                    child.Columns[i] = columns[i].Trim();

                yield return child;
            }
            dataReader.Dispose();
        }

        public void CreateIndex(SqlIndex index)
        {
            throw new NotImplementedException();
        }

        private NpgsqlCommand forgetFilesystemCommand;
        public void ForgetFilesystemContents(int currentMediaId)
        {
            if (forgetFilesystemCommand == null)
            {
                forgetFilesystemCommand = connection.CreateCommand();
                forgetFilesystemCommand.CommandText = "DELETE FROM azusa.filesysteminfo WHERE mediaId=@mediaId";
                forgetFilesystemCommand.Parameters.Add("@mediaId", NpgsqlDbType.Integer);
            }

            forgetFilesystemCommand.Parameters["@mediaId"].Value = currentMediaId;
            forgetFilesystemCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand addFilesystemInfoCommand;
        public void AddFilesystemInfo(FilesystemMetadataEntity dirEntity)
        {
            if (addFilesystemInfoCommand == null)
            {
                addFilesystemInfoCommand = connection.CreateCommand();
                addFilesystemInfoCommand.CommandText =
                    "INSERT INTO azusa.filesysteminfo " +
                    "(mediaid,isdirectory,fullname,size,modified,head,parent) " +
                    "VALUES " +
                    "(@mediaid,@isDir,@fullname,@size,@modified,@head,@parent) " +
                    "RETURNING id";
                addFilesystemInfoCommand.Parameters.Add("@mediaId", NpgsqlDbType.Integer);
                addFilesystemInfoCommand.Parameters.Add("@isDir", NpgsqlDbType.Boolean);
                addFilesystemInfoCommand.Parameters.Add("@fullname", NpgsqlDbType.Varchar);
                addFilesystemInfoCommand.Parameters.Add("@size", NpgsqlDbType.Bigint);
                addFilesystemInfoCommand.Parameters.Add("@modified", NpgsqlDbType.Timestamp);
                addFilesystemInfoCommand.Parameters.Add("@head", NpgsqlDbType.Bytea);
                addFilesystemInfoCommand.Parameters.Add("@parent", NpgsqlDbType.Bigint);
            }

            Debug.WriteLine(dirEntity.FullName);

            addFilesystemInfoCommand.Parameters["@mediaid"].Value = dirEntity.MediaId;
            addFilesystemInfoCommand.Parameters["@isDir"].Value = dirEntity.IsDirectory;
            addFilesystemInfoCommand.Parameters["@fullname"].Value = dirEntity.FullName;
            if (dirEntity.Size.HasValue)
                addFilesystemInfoCommand.Parameters["@size"].Value = dirEntity.Size;
            else
                addFilesystemInfoCommand.Parameters["@size"].Value = DBNull.Value;

            if (dirEntity.Modified.HasValue)
                addFilesystemInfoCommand.Parameters["@modified"].Value = dirEntity.Modified;
            else
                addFilesystemInfoCommand.Parameters["@modified"].Value = DBNull.Value;

            if (dirEntity.Header != null && dirEntity.Header.Length > 0)
                addFilesystemInfoCommand.Parameters["@head"].Value = dirEntity.Header;
            else
                addFilesystemInfoCommand.Parameters["@head"].Value = DBNull.Value;

            addFilesystemInfoCommand.Parameters["@parent"].Value = dirEntity.ParentId;

            NpgsqlDataReader ndr = addFilesystemInfoCommand.ExecuteReader();
            ndr.Read();
            dirEntity.Id = ndr.GetInt64(0);
            dirEntity.DateAdded = DateTime.Now;
            ndr.Close();
        }

        private NpgsqlCommand getFilesystemMetadataCommand;
        public IEnumerable<FilesystemMetadataEntity> GetFilesystemMetadata(int currentMediaId, bool dirs)
        {
            if (getFilesystemMetadataCommand == null)
            {
                getFilesystemMetadataCommand = connection.CreateCommand();
                getFilesystemMetadataCommand.CommandText =
                    "SELECT * FROM azusa.filesysteminfo " +
                    "WHERE mediaid = @mediaid " +
                    "AND isdirectory = @isdirectory " +
                    "ORDER BY parent ASC";
                getFilesystemMetadataCommand.Parameters.Add("@mediaid", NpgsqlDbType.Integer);
                getFilesystemMetadataCommand.Parameters.Add("@isdirectory", NpgsqlDbType.Boolean);
            }

            getFilesystemMetadataCommand.Parameters["@mediaid"].Value = currentMediaId;
            getFilesystemMetadataCommand.Parameters["@isdirectory"].Value = dirs;
            NpgsqlDataReader dataReader = getFilesystemMetadataCommand.ExecuteReader();
            while (dataReader.Read())
            {
                FilesystemMetadataEntity child = new FilesystemMetadataEntity();
                child.Id = dataReader.GetInt64(0);
                child.MediaId = dataReader.GetInt32(1);
                child.DateAdded = dataReader.GetDateTime(2);
                child.IsDirectory = dataReader.GetBoolean(3);
                child.FullName = dataReader.GetString(4);
                if (!dataReader.IsDBNull(5))
                    child.Size = dataReader.GetInt64(5);
                if (!dataReader.IsDBNull(6))
                    child.Modified = dataReader.GetDateTime(6);
                if (!dataReader.IsDBNull(7))
                    child.Header = dataReader.GetByteArray(7);
                child.ParentId = dataReader.GetInt64(8);
                yield return child;
            }
            dataReader.Close();
        }

        private NpgsqlCommand vndbSearchCommand;
        public IEnumerable<VndbSearchResult> Vndb_Search(string searchquery)
        {
            if (vndbSearchCommand == null)
            {
                vndbSearchCommand = connection.CreateCommand();
                vndbSearchCommand.CommandText = Resources.VndbSearch_Postgre;
                vndbSearchCommand.Parameters.Add("@query", NpgsqlDbType.Varchar);
            }

            vndbSearchCommand.Parameters["@query"].Value = searchquery;
            NpgsqlDataReader reader = vndbSearchCommand.ExecuteReader();
            while (reader.Read())
            {
                VndbSearchResult result = new VndbSearchResult();
                result.RID = reader.GetInt32(0);
                result.Title = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    result.GTIN = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    result.SKU = reader.GetString(3);
                yield return result;
            }
            reader.Dispose();
        }

        private NpgsqlCommand getVnByReleaseCommand;
        public IEnumerable<VndbVnResult> Vndb_GetVnsByRelease(int searchResultRid)
        {
            if (getVnByReleaseCommand == null)
            {
                getVnByReleaseCommand = connection.CreateCommand();
                getVnByReleaseCommand.CommandText = "SELECT * FROM dump_vndb.release_vns root WHERE rid=@id";
                getVnByReleaseCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            getVnByReleaseCommand.Parameters["@id"].Value = searchResultRid;
            NpgsqlDataReader dataReader = getVnByReleaseCommand.ExecuteReader();
            while (dataReader.Read())
            {
                VndbVnResult vnResult = new VndbVnResult();
                vnResult.RID = dataReader.GetInt32(0);
                vnResult.VNID = dataReader.GetInt32(1);
                vnResult.Title = dataReader.GetString(2);
                if (!dataReader.IsDBNull(3))
                    vnResult.Original = dataReader.GetString(3);
                vnResult.DateAdded = dataReader.GetDateTime(4);
                yield return vnResult;
            }

            dataReader.Dispose();
        }

        private NpgsqlCommand vndbGetReleaseByIdCommand;
        private NpgsqlCommand vndbGetReleaseLanguageById;
        private NpgsqlCommand vndbGetReleaseMediaById;
        private NpgsqlCommand vndbGetReleasePlatformById;
        private NpgsqlCommand vndbGetReleaseProducersById;
        public VndbRelease Vndb_GetReleaseById(int releaseResultRid)
        {
            if (vndbGetReleaseByIdCommand == null)
            {
                vndbGetReleaseByIdCommand = connection.CreateCommand();
                vndbGetReleaseByIdCommand.CommandText = "SELECT * FROM dump_vndb.release WHERE id=@id";
                vndbGetReleaseByIdCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetReleaseByIdCommand.Parameters["@id"].Value = releaseResultRid;
            NpgsqlDataReader dataReader = vndbGetReleaseByIdCommand.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Dispose();
                return null;
            }
            VndbRelease result = new VndbRelease();
            result.Title = dataReader.GetString(1);

            if (!dataReader.IsDBNull(2))
                result.OriginalTitle = dataReader.GetString(2);

            result.Released = dataReader.GetDate(3).ToString();
            result.Type = dataReader.GetString(4);
            result.IsPatch = dataReader.GetBoolean(5);
            result.IsFreeware = dataReader.GetBoolean(6);
            result.IsDoujin = dataReader.GetBoolean(7);

            if (!dataReader.IsDBNull(8))
                result.Website = dataReader.GetString(8);

            if (!dataReader.IsDBNull(9))
                result.Notes = dataReader.GetString(9);

            if (!dataReader.IsDBNull(10))
                result.AgeRestriction = dataReader.GetInt32(10);

            if (!dataReader.IsDBNull(11))
                result.GTIN = dataReader.GetString(11);

            if (!dataReader.IsDBNull(12))
                result.SKU = dataReader.GetString(12);

            if (!dataReader.IsDBNull(13))
                result.Resolution = dataReader.GetString(13);
            dataReader.Dispose();

            if (vndbGetReleaseLanguageById == null)
            {
                vndbGetReleaseLanguageById = connection.CreateCommand();
                vndbGetReleaseLanguageById.CommandText = "SELECT lang FROM dump_vndb.release_languages WHERE rid=@id";
                vndbGetReleaseLanguageById.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetReleaseLanguageById.Parameters["@id"].Value = releaseResultRid;
            dataReader = vndbGetReleaseLanguageById.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0},", dataReader.GetString(0));
            dataReader.Dispose();
            result.Language = sb.ToString();

            if (vndbGetReleaseMediaById == null)
            {
                vndbGetReleaseMediaById = connection.CreateCommand();
                vndbGetReleaseMediaById.CommandText =
                    "SELECT medium, qty FROM dump_vndb.release_media WHERE rid=@id AND qty IS NOT NULL";
                vndbGetReleaseMediaById.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetReleaseMediaById.Parameters["@id"].Value = releaseResultRid;
            dataReader = vndbGetReleaseMediaById.ExecuteReader();
            sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0}x {1},", dataReader.GetInt32(1), dataReader.GetString(0));
            dataReader.Dispose();
            result.Media = sb.ToString();

            if (vndbGetReleasePlatformById == null)
            {
                vndbGetReleasePlatformById = connection.CreateCommand();
                vndbGetReleasePlatformById.CommandText = "SELECT platform FROM dump_vndb.release_platforms WHERE rid=@id";
                vndbGetReleasePlatformById.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetReleasePlatformById.Parameters["@id"].Value = releaseResultRid;
            dataReader = vndbGetReleasePlatformById.ExecuteReader();
            sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0},", dataReader.GetString(0));
            dataReader.Dispose();
            result.Platforms = sb.ToString();

            if (vndbGetReleaseProducersById == null)
            {
                vndbGetReleaseProducersById = connection.CreateCommand();
                vndbGetReleaseProducersById.CommandText =
                    "SELECT DISTINCT name FROM dump_vndb.release_producers WHERE rid=@id";
                vndbGetReleaseProducersById.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetReleaseProducersById.Parameters["@id"].Value = releaseResultRid;
            dataReader = vndbGetReleaseProducersById.ExecuteReader();
            sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0},", dataReader.GetString(0));
            dataReader.Dispose();
            result.Producers = sb.ToString();

            return result;
        }

        private NpgsqlCommand vndbGetVn;
        private NpgsqlCommand vndbGetVnAnime;
        private NpgsqlCommand vndbGetVnPlatforms;
        private NpgsqlCommand vndbGetVnRelations;
        private NpgsqlCommand vndbGetVnScreens;
        private NpgsqlCommand vndbGetVnLanguages;
        public VndbVn Vndb_GetVnById(int vnResultVnid)
        {
            if (vndbGetVn == null)
            {
                vndbGetVn = connection.CreateCommand();
                vndbGetVn.CommandText = "SELECT * FROM dump_vndb.vn WHERE id=@id";
                vndbGetVn.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVn.Parameters["@id"].Value = vnResultVnid;
            NpgsqlDataReader dataReader = vndbGetVn.ExecuteReader();
            dataReader.Read();
            VndbVn result = new VndbVn();
            result.Title = dataReader.GetString(3);

            if (!dataReader.IsDBNull(4))
                result.OriginalTitle = dataReader.GetString(4);

            if (!dataReader.IsDBNull(5))
                result.ReleaseDate = dataReader.GetDate(5).ToString();

            if (!dataReader.IsDBNull(6))
                result.Alias = dataReader.GetString(6);

            if (!dataReader.IsDBNull(7))
                result.Length = dataReader.GetInt32(7);

            if (!dataReader.IsDBNull(8))
                result.Description = dataReader.GetString(8);

            if (!dataReader.IsDBNull(9))
                result.WikipediaLink = dataReader.GetString(9);

            if (!dataReader.IsDBNull(12))
                result.Image = dataReader.GetPicture(12);

            result.ImageNSFW = dataReader.GetBoolean(13);
            result.Popularity = dataReader.GetDouble(14);
            result.Rating = dataReader.GetDouble(15);
            dataReader.Close();

            if (vndbGetVnAnime == null)
            {
                vndbGetVnAnime = connection.CreateCommand();
                vndbGetVnAnime.CommandText = "SELECT * FROM dump_vndb.vn_anime WHERE vnid=@id";
                vndbGetVnAnime.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVnAnime.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnAnime.ExecuteReader();
            if (dataReader.Read())
            {
                VndbVnAnime anime = new VndbVnAnime();
                anime.TitleRomanji = dataReader.GetString(5);
                anime.TitleKanji = dataReader.GetString(6);
                anime.Year = dataReader.GetInt32(7);
                result.Anime = anime;
            }
            dataReader.Close();

            if (vndbGetVnPlatforms == null)
            {
                vndbGetVnPlatforms = connection.CreateCommand();
                vndbGetVnPlatforms.CommandText = "SELECT platform FROM dump_vndb.vn_platforms WHERE vnid=@id";
                vndbGetVnPlatforms.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVnPlatforms.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnPlatforms.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0} ", dataReader.GetString(0));
            dataReader.Close();
            result.Platform = sb.ToString();

            if (vndbGetVnRelations == null)
            {
                vndbGetVnRelations = connection.CreateCommand();
                vndbGetVnRelations.CommandText = "SELECT relation,title FROM dump_vndb.vn_relation WHERE srcid=@id";
                vndbGetVnRelations.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVnRelations.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnRelations.ExecuteReader();
            List<string> relations = new List<string>();
            while (dataReader.Read())
                relations.Add(String.Format("{0}: {1}", dataReader.GetString(0), dataReader.GetString(1)));
            if (relations.Count > 0)
                result.Relations = relations.ToArray();
            dataReader.Close();

            if (vndbGetVnScreens == null)
            {
                vndbGetVnScreens = connection.CreateCommand();
                vndbGetVnScreens.CommandText = "SELECT image FROM dump_vndb.vn_screens WHERE vnid=@id";
                vndbGetVnScreens.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVnScreens.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnScreens.ExecuteReader();
            result.Screens = new List<Image>();
            while (dataReader.Read())
                if (!dataReader.IsDBNull(0))
                    result.Screens.Add(dataReader.GetPicture(0));
            dataReader.Close();

            if (vndbGetVnLanguages == null)
            {
                vndbGetVnLanguages = connection.CreateCommand();
                vndbGetVnLanguages.CommandText = "SELECT language FROM dump_vndb.vn_languages WHERE vnid=@id";
                vndbGetVnLanguages.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vndbGetVnLanguages.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnLanguages.ExecuteReader();
            sb = new StringBuilder();
            while (dataReader.Read())
                sb.AppendFormat("{0} ", dataReader.GetString(0));
            result.Languages = sb.ToString();
            dataReader.Close();

            return result;
        }

        public DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query)
        {
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, column.ColumnName,
                column.ParameterName);
            command.Parameters.Add(new NpgsqlParameter(column.ParameterName, column.DbType));
            command.Parameters[column.ParameterName].Value = query;
            return command.ExecuteReader();
        }

        private NpgsqlCommand myFigureCollectionSearchCommand;
        public IEnumerable<Figure> MyFigureCollection_Search(string query)
        {
            if (myFigureCollectionSearchCommand == null)
            {
                myFigureCollectionSearchCommand = connection.CreateCommand();
                myFigureCollectionSearchCommand.CommandText = Resources.MyFigureCollectionSearch_Postgre;
                myFigureCollectionSearchCommand.Parameters.Add("@query", NpgsqlDbType.Varchar);
            }
            query = "%" + query + "%";
            myFigureCollectionSearchCommand.Parameters["@query"].Value = query;
            NpgsqlDataReader dataReader = myFigureCollectionSearchCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Figure child = new Figure();
                child.ID = dataReader.GetInt32(0);
                if (!dataReader.IsDBNull(1))
                    child.RootName = dataReader.GetString(1);

                if (!dataReader.IsDBNull(2))
                    child.CategoryName = dataReader.GetString(2);

                if (!dataReader.IsDBNull(3))
                    child.Barcode = dataReader.GetString(3);

                if (!dataReader.IsDBNull(4))
                    child.Name = dataReader.GetString(4);

                if (!dataReader.IsDBNull(5))
                    child.ReleaseDate = dataReader.GetDateTime(5);

                if (!dataReader.IsDBNull(6))
                    child.Price = dataReader.GetDouble(6);

                if (!dataReader.IsDBNull(7))
                    child.Thumbnail = dataReader.GetByteArray(7);

                yield return child;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand myFigureCollectionGetPhotoCommand;
        public Image MyFigureCollection_GetPhoto(int wrappedFigureId)
        {
            if (myFigureCollectionGetPhotoCommand == null)
            {
                myFigureCollectionGetPhotoCommand = connection.CreateCommand();
                myFigureCollectionGetPhotoCommand.CommandText =
                    "SELECT image FROM dump_myfigurecollection.figurephotos WHERE id = @id";
                myFigureCollectionGetPhotoCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            myFigureCollectionGetPhotoCommand.Parameters["@id"].Value = wrappedFigureId;
            NpgsqlDataReader npgsqlDataReader = myFigureCollectionGetPhotoCommand.ExecuteReader();
            Image result = null;
            if (npgsqlDataReader.Read())
            {
                result = npgsqlDataReader.GetPicture(0);
            }
            npgsqlDataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vocadbSearchCommand;
        public IEnumerable<VocadbSearchResult> VocaDb_Search(string text)
        {
            text = "%" + text + "%";
            if (vocadbSearchCommand == null)
            {
                vocadbSearchCommand = connection.CreateCommand();
                vocadbSearchCommand.CommandText = Resources.Vocadb_Search_Postgre;
                vocadbSearchCommand.Parameters.Add("@query", NpgsqlDbType.Varchar);
            }
            vocadbSearchCommand.Parameters["@query"].Value = text;
            NpgsqlDataReader dataReader = vocadbSearchCommand.ExecuteReader();
            while (dataReader.Read())
            {
                VocadbSearchResult result = new VocadbSearchResult();
                result.Id = dataReader.GetInt32(0);
                result.Name = dataReader.GetString(1);
                result.ArtistString = dataReader.GetString(2);
                result.DiscType = dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    result.ReleaseDate = dataReader.GetDateTime(4);

                if (!dataReader.IsDBNull(5))
                    result.CatalogNumber = dataReader.GetString(5);

                yield return result;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand vocadbGetAlbumCoverCommand;
        public Image Vocadb_GetAlbumCover(int id)
        {
            if (vocadbGetAlbumCoverCommand == null)
            {
                vocadbGetAlbumCoverCommand = connection.CreateCommand();
                vocadbGetAlbumCoverCommand.CommandText = "SELECT cover FROM dump_vocadb.albums WHERE id=@id";
                vocadbGetAlbumCoverCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vocadbGetAlbumCoverCommand.Parameters["@id"].Value = id;
            NpgsqlDataReader dataReader = vocadbGetAlbumCoverCommand.ExecuteReader();
            Bitmap result = null;
            if (dataReader.Read())
            {
                if (!dataReader.IsDBNull(0))
                    result = dataReader.GetPicture(0);
            }

            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vocadbFindAlbumNamesBySongNamesCommand;
        public List<string> VocaDb_FindAlbumNamesBySongNames(string text)
        {
            if (vocadbFindAlbumNamesBySongNamesCommand == null)
            {
                vocadbFindAlbumNamesBySongNamesCommand = connection.CreateCommand();
                vocadbFindAlbumNamesBySongNamesCommand.CommandText = Resources.Vocadb_FindSongRelatedAlbum;
                vocadbFindAlbumNamesBySongNamesCommand.Parameters.Add("@query", NpgsqlDbType.Varchar);
            }
            vocadbFindAlbumNamesBySongNamesCommand.Parameters["@query"].Value = "%" + text + "%";
            var dataReader = vocadbFindAlbumNamesBySongNamesCommand.ExecuteReader();
            List<string> result = new List<string>();
            while (dataReader.Read())
                result.Add(dataReader.GetString(0));
            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand vocadbGetTracksByAlbumCommand;
        public IEnumerable<VocadbTrackEntry> VocaDb_GetTracksByAlbum(int wrappedId)
        {
            if (vocadbGetTracksByAlbumCommand == null)
            {
                vocadbGetTracksByAlbumCommand = connection.CreateCommand();
                vocadbGetTracksByAlbumCommand.CommandText =
                    "SELECT * FROM dump_vocadb.albumtracks WHERE albumid = @id ORDER BY albumid ASC, discnumber ASC, tracknumber ASC";
                vocadbGetTracksByAlbumCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }

            vocadbGetTracksByAlbumCommand.Parameters["@id"].Value = wrappedId;
            NpgsqlDataReader dataReader = vocadbGetTracksByAlbumCommand.ExecuteReader();
            while (dataReader.Read())
            {
                VocadbTrackEntry child = new VocadbTrackEntry();
                child.Id = dataReader.GetInt32(0);
                child.DateAdded = dataReader.GetDateTime(1);
                child.Name = dataReader.GetString(2);
                child.DiscNumber = dataReader.GetInt32(3);
                child.SongId = dataReader.GetInt32(4);
                child.TrackNumber = dataReader.GetInt32(5);
                child.AlbumId = dataReader.GetInt32(6);
                yield return child;
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand getAllTagsCommand;
        public IEnumerable<GelbooruTag> Gelbooru_GetAllTags()
        {
            if (getAllTagsCommand == null)
            {
                getAllTagsCommand = connection.CreateCommand();
                getAllTagsCommand.CommandText = Resources.Gelbooru_GetTags_Postgre;
            }

            NpgsqlDataReader dataReader = getAllTagsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int numImages = dataReader.GetInt32(1);
                if (numImages > 0)
                {
                    GelbooruTag child = new GelbooruTag();
                    child.Tag = dataReader.GetString(0);
                    if (char.IsLetter(child.Tag[0]))
                    {
                        child.NumberOfImages = numImages;
                        child.Id = dataReader.GetInt32(2);
                        yield return child;
                    }
                }
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand gelbooruGetPostsByTagCommand;
        public IEnumerable<int> Gelbooru_GetPostsByTag(int tagId)
        {
            if (gelbooruGetPostsByTagCommand == null)
            {
                gelbooruGetPostsByTagCommand = connection.CreateCommand();
                gelbooruGetPostsByTagCommand.CommandText =
                    "SELECT DISTINCT postid FROM dump_gb.posttags WHERE tagid = @tagid";
                gelbooruGetPostsByTagCommand.Parameters.Add("@tagid", NpgsqlDbType.Integer);
            }

            gelbooruGetPostsByTagCommand.Parameters["@tagid"].Value = tagId;
            NpgsqlDataReader dataReader = gelbooruGetPostsByTagCommand.ExecuteReader();
            while (dataReader.Read())
            {
                yield return dataReader.GetInt32(0);
            }
            dataReader.Dispose();
        }

        private NpgsqlCommand dexcomGetLatestGlucose;
        public DexTimelineEntry Dexcom_GetLatestGlucoseEntry()
        {
            if (dexcomGetLatestGlucose == null)
            {
                dexcomGetLatestGlucose = connection.CreateCommand();
                dexcomGetLatestGlucose.CommandText = "SELECT * FROM dexcom.history WHERE glucose IS NOT NULL ORDER BY date DESC, time DESC";
            }

            NpgsqlDataReader dataReader = dexcomGetLatestGlucose.ExecuteReader();
            DexTimelineEntry result = null;
            if (dataReader.Read())
            {
                DateTime date = dataReader.GetDateTime(0);
                TimeSpan time = dataReader.GetTimeSpan(1);
                DateTime timestamp = new DateTime(date.Ticks + time.Ticks);
                result = new DexTimelineEntry();
                result.Timestamp = timestamp;
                result.SensorFiltered = (uint)dataReader.GetInt32(2);
                result.SensorUnfiltered = (uint) dataReader.GetInt32(3);
                result.Rssi = (uint) dataReader.GetInt32(4);
                result.Glucose = (uint) dataReader.GetInt32(5);
            }

            dataReader.Dispose();
            return result;
        }

        private NpgsqlCommand dexcomGetGlucoseEntriesAfterCommand;
        public IEnumerable<DexTimelineEntry> Dexcom_GetGlucoseEntriesAfter(DateTime scope)
        {
            if (dexcomGetGlucoseEntriesAfterCommand == null)
            {
                dexcomGetGlucoseEntriesAfterCommand = connection.CreateCommand();
                dexcomGetGlucoseEntriesAfterCommand.CommandText =
                    "SELECT * FROM dexcom.history WHERE glucose IS NOT NULL AND date>=@scope";
                dexcomGetGlucoseEntriesAfterCommand.Parameters.Add("@scope", NpgsqlDbType.Date);
            }

            dexcomGetGlucoseEntriesAfterCommand.Parameters["@scope"].Value = scope.Date;
            NpgsqlDataReader dataReader = dexcomGetGlucoseEntriesAfterCommand.ExecuteReader();
            while (dataReader.Read())
            {
                DateTime date = dataReader.GetDateTime(0);
                TimeSpan time = dataReader.GetTimeSpan(1);
                DateTime timestamp = new DateTime(date.Ticks + time.Ticks);
                DexTimelineEntry timelineEntry = new DexTimelineEntry();
                timelineEntry.Timestamp = timestamp;

                if (!dataReader.IsDBNull(2))
                    timelineEntry.SensorFiltered = (uint)dataReader.GetInt32(2);

                if(!dataReader.IsDBNull(3))
                    timelineEntry.SensorUnfiltered = (uint)dataReader.GetInt32(3);

                if (!dataReader.IsDBNull(4))
                    timelineEntry.Rssi = (uint)dataReader.GetInt32(4);

                timelineEntry.Glucose = (uint)dataReader.GetInt32(5);
                yield return timelineEntry;
            }
            dataReader.Dispose();
        }

        private void AssertNoEvilString(string schemaName)
        {
            bool[] hits = new bool[10];
            hits[0] = schemaName.Contains("'");
            hits[1] = schemaName.Contains("\"");
            hits[2] = schemaName.Contains("`");
            hits[3] = schemaName.Contains("´");
            hits[4] = schemaName.Contains("$");
            hits[5] = schemaName.Contains("@");
            if (hits.Contains(true))
                throw new ArgumentException("No sir, I don't like SQL-Injections.");
        }

        public void CreateSchema(string schemaName)
        {
            AssertNoEvilString(schemaName);

            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = String.Format("CREATE SCHEMA IF NOT EXISTS {0}", schemaName);
            Console.WriteLine(command.CommandText);
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void MoveAndRenameTable(string oldSchemaName, string oldTableName, string schemaName, string newTableName)
        {
            AssertNoEvilString(oldSchemaName);
            AssertNoEvilString(oldTableName);
            AssertNoEvilString(schemaName);
            AssertNoEvilString(newTableName);

            NpgsqlCommand command1 = connection.CreateCommand();
            command1.CommandText = String.Format("ALTER TABLE \"{0}\".\"{1}\" SET SCHEMA \"{2}\"", oldSchemaName, oldTableName, schemaName);
            Console.WriteLine(command1.CommandText);
            command1.ExecuteNonQuery();
            command1.Dispose();
            NpgsqlCommand command2 = connection.CreateCommand();
            command2.CommandText = String.Format("ALTER TABLE \"{0}\".\"{1}\" RENAME TO \"{2}\"", schemaName, oldTableName, newTableName);
            Console.WriteLine(command2.CommandText);
            command2.ExecuteNonQuery();
            command2.Dispose();
        }

        public bool IsAllowedSyncSource()
        {
            return true;
        }

        public bool IsAllowedSyncTarget()
        {
            return false;
        }

        public object GetConnectionObject()
        {
            return connection;
        }

        public string GetConnectionString()
        {
            return connection.ConnectionString;
        }

        private NpgsqlCommand insertDiscArchivatorDiscCommand;
        public void InsertDiscArchivatorDisc(long discid, string path, string name)
        {
            if (insertDiscArchivatorDiscCommand == null)
            {
                insertDiscArchivatorDiscCommand = connection.CreateCommand();
                insertDiscArchivatorDiscCommand.CommandText =
                    "INSERT INTO discarchivator.discs (discid,path,name) VALUES (@discid,@path,@name)";
                insertDiscArchivatorDiscCommand.Parameters.Add("@discid", NpgsqlDbType.Bigint);
                insertDiscArchivatorDiscCommand.Parameters.Add("@path", NpgsqlDbType.Text);
                insertDiscArchivatorDiscCommand.Parameters.Add("@name", NpgsqlDbType.Varchar);
            }

            insertDiscArchivatorDiscCommand.Parameters["@discid"].Value = discid;
            insertDiscArchivatorDiscCommand.Parameters["@path"].Value = path;
            insertDiscArchivatorDiscCommand.Parameters["@name"].Value = name;
            insertDiscArchivatorDiscCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand getDiscArchivatorDiscCommand;
        public DiscStatus GetDiscArchivatorDisc(long discid)
        {
            if (getDiscArchivatorDiscCommand == null)
            {
                getDiscArchivatorDiscCommand = connection.CreateCommand();
                getDiscArchivatorDiscCommand.CommandText = "SELECT * FROM discarchivator.discs WHERE discid=@discid";
                getDiscArchivatorDiscCommand.Parameters.Add("@discid", NpgsqlDbType.Bigint);
            }

            getDiscArchivatorDiscCommand.Parameters["@discid"].Value = discid;

            DiscStatus discStatus = null;
            NpgsqlDataReader dataReader = getDiscArchivatorDiscCommand.ExecuteReader();
            if (dataReader.Read())
            {
                discStatus = ReadDiscStatusRow(dataReader);
            }
            dataReader.Close();
            return discStatus;
        }

        private static DiscStatus ReadDiscStatusRow(NpgsqlDataReader dataReader)
        {
            DiscStatus discStatus;
            discStatus = new DiscStatus();
            discStatus.DiscId = dataReader.GetInt64(0);
            discStatus.DateAdded = dataReader.GetDateTime(1);
            discStatus.PgSerial = dataReader.GetInt32(2);
            discStatus.Path = new DirectoryInfo(dataReader.GetString(3));
            discStatus.Dumped = dataReader.GetBoolean(4);
            discStatus.Ripped = dataReader.GetBoolean(5);
            discStatus.Name = dataReader.GetString(6);
            discStatus.Completed = dataReader.GetBoolean(7);
            discStatus.AzusaLinked = dataReader.GetBoolean(8);
            return discStatus;
        }

        private NpgsqlCommand setDiscArchivatorPropertyCommand;
        public void SetDiscArchivatorProperty(long discid, DiscStatusProperty property, bool value)
        {
            if (setDiscArchivatorPropertyCommand == null)
            {
                setDiscArchivatorPropertyCommand = connection.CreateCommand();
                setDiscArchivatorPropertyCommand.CommandText = "UPDATE discarchivator.discs " +
                                                               "SET dumped = @dumped, " +
                                                               "    ripped = @ripped, " +
                                                               "    completed = @completed " +
                                                               "WHERE discid=@discid";
                setDiscArchivatorPropertyCommand.Parameters.Add("@dumped", NpgsqlDbType.Boolean);
                setDiscArchivatorPropertyCommand.Parameters.Add("@ripped", NpgsqlDbType.Boolean);
                setDiscArchivatorPropertyCommand.Parameters.Add("@completed", NpgsqlDbType.Boolean);
                setDiscArchivatorPropertyCommand.Parameters.Add("@discid", NpgsqlDbType.Bigint);
            }

            setDiscArchivatorPropertyCommand.Parameters["@dumped"].Value = false;
            setDiscArchivatorPropertyCommand.Parameters["@ripped"].Value = false;
            setDiscArchivatorPropertyCommand.Parameters["@completed"].Value = false;
            setDiscArchivatorPropertyCommand.Parameters["@discid"].Value = discid;
            switch (property)
            {
                case DiscStatusProperty.Completed:
                    setDiscArchivatorPropertyCommand.Parameters["@completed"].Value = true;
                    setDiscArchivatorPropertyCommand.Parameters["@dumped"].Value = true;
                    setDiscArchivatorPropertyCommand.Parameters["@ripped"].Value = true;
                    break;
                case DiscStatusProperty.Dumped:
                    setDiscArchivatorPropertyCommand.Parameters["@dumped"].Value = true;
                    setDiscArchivatorPropertyCommand.Parameters["@ripped"].Value = true;
                    break;
                case DiscStatusProperty.Ripped:
                    setDiscArchivatorPropertyCommand.Parameters["@ripped"].Value = true;
                    break;
                default:
                    throw new NotImplementedException();
            }

            setDiscArchivatorPropertyCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand setDiscArchivatorAzusaLinkCommand;
        public void SetDiscArchivatorAzusaLink(long discid, int mediumId)
        {
            Media media = GetMediaById(mediumId);
            media.DiscId = discid;
            UpdateMedia(media);

            if (setDiscArchivatorAzusaLinkCommand == null)
            {
                setDiscArchivatorAzusaLinkCommand = connection.CreateCommand();
                setDiscArchivatorAzusaLinkCommand.CommandText =
                    "UPDATE discarchivator.discs " +
                    "SET azusalinked = TRUE, " +
                    "    linkdate = @linkdate " +
                    "WHERE discid=@discid";
                setDiscArchivatorAzusaLinkCommand.Parameters.Add("@discid", NpgsqlDbType.Bigint);
                setDiscArchivatorAzusaLinkCommand.Parameters.Add("@linkdate", NpgsqlDbType.Timestamp);
            }

            setDiscArchivatorAzusaLinkCommand.Parameters["@discid"].Value = discid;
            setDiscArchivatorAzusaLinkCommand.Parameters["@linkdate"].Value = DateTime.Now;
            int result = setDiscArchivatorAzusaLinkCommand.ExecuteNonQuery();
            if (result != 1)
                throw new Exception("unexpected update result");
        }

        private NpgsqlCommand getDiscArchivatorEntiresCommand;
        public IEnumerable<DiscStatus> GetDiscArchivatorEntries()
        {
            if (getDiscArchivatorEntiresCommand == null)
            {
                getDiscArchivatorEntiresCommand = connection.CreateCommand();
                getDiscArchivatorEntiresCommand.CommandText = "SELECT * FROM discarchivator.discs ORDER BY pgserial";
            }

            NpgsqlDataReader dataReader = getDiscArchivatorEntiresCommand.ExecuteReader();
            while (dataReader.Read())
            {
                yield return ReadDiscStatusRow(dataReader);
            }
            dataReader.Close();
        }

        public Media[] findBrokenBandcampImports()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id FROM azusa.media WHERE dumppath IS NOT NULL AND dumppath LIKE '%disc.m3u' AND metafile IS NULL";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            List<int> results = new List<int>();
            while (dataReader.Read())
                results.Add(dataReader.GetInt32(0));
            dataReader.Close();

            int[] idArray = results.ToArray();
            Media[] mediaArray = Array.ConvertAll(idArray, x => GetMediaById(x));
            return mediaArray;
        }

        public Media[] FindAutofixableMetafiles()
        {
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.media WHERE dumppath IS NOT NULL AND metafile IS NULL";
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            List<int> results = new List<int>();
            while (dataReader.Read())
                results.Add(dataReader.GetInt32(0));
            dataReader.Close();

            int[] idArray = results.ToArray();
            Media[] mediaArray = Array.ConvertAll(idArray, x => GetMediaById(x));
            return mediaArray;
        }

        public void Sync_AlterTable(string tableName, DatabaseColumn missingColumn)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private NpgsqlCommand activateLicenseCommand;
        public void ActivateLicense(byte[] contextLicenseSeed)
        {
            string val = BitConverter.ToString(contextLicenseSeed);

            if (activateLicenseCommand == null)
            {
                activateLicenseCommand = connection.CreateCommand();
                activateLicenseCommand.CommandText = "UPDATE licensing.fatclient_machines SET state = 1 WHERE uid = @uid";
                activateLicenseCommand.Parameters.Add("@uid", NpgsqlDbType.Varchar);
            }

            activateLicenseCommand.Parameters["@uid"].Value = val;
            activateLicenseCommand.ExecuteNonQuery();
            
        }
    }
}
