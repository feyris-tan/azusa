﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using AzusaERP;
using libeuroexchange.Model;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Setup;
using moe.yo3explorer.azusa.DatabaseTasks;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.Properties;
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

            if (string.IsNullOrEmpty(product.Sku))
                cmd.Parameters["@sku"].Value = DBNull.Value;
            else
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

            if (product.Screenshot == null)
                setScreenshotCommand.Parameters["@screenshot"].Value = DBNull.Value;
            else
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
            
            m.isSealed = ndr.GetBoolean(10);

            if (!ndr.IsDBNull(11))
                m.DateUpdated = ndr.GetDateTime(11);
            
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
                                                 "    issealed=@issealed, dateupdated=@dateupdated " +
                                                 "WHERE id=@id";
                updateMediaCommand.Parameters.Add("@name", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@mediaTypeId", NpgsqlDbType.Integer);
                updateMediaCommand.Parameters.Add("@sku", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@dumpstoragespace", NpgsqlDbType.Integer);
                updateMediaCommand.Parameters.Add("@dumppath", NpgsqlDbType.Varchar);
                updateMediaCommand.Parameters.Add("@metafile", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@graphdata", NpgsqlDbType.Text);
                updateMediaCommand.Parameters.Add("@issealed", NpgsqlDbType.Boolean);
                updateMediaCommand.Parameters.Add("@dateupdated", NpgsqlDbType.Timestamp);
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
            
            updateMediaCommand.Parameters["@issealed"].Value = media.isSealed;
            updateMediaCommand.Parameters["@dateupdated"].Value = DateTime.Now;

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
        
        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }
        
        public bool CanActivateLicense
        {
            get
            {
                return true;
            }
        }

        public bool CanUpdateExchangeRates => true;

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
                case "uuid":
                    return DbType.String;
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
        
        public void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage)
        {
            throw new NotImplementedException();
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
        
        public DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query)
        {
            NpgsqlCommand command = connection.CreateCommand();
            command.CommandText = String.Format("SELECT * FROM {0} WHERE {1} = {2}", tableName, column.ColumnName,
                column.ParameterName);
            command.Parameters.Add(new NpgsqlParameter(column.ParameterName, column.DbType));
            command.Parameters[column.ParameterName].Value = query;
            return command.ExecuteReader();
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
        
        private NpgsqlCommand removeMediaCommand;
        public void RemoveMedia(Media currentMedia)
        {
            if (removeMediaCommand == null)
            {
                removeMediaCommand = connection.CreateCommand();
                removeMediaCommand.CommandText = "DELETE FROM azusa.media WHERE id = @id";
                removeMediaCommand.Parameters.Add("@id", NpgsqlDbType.Integer);
            }
            removeMediaCommand.Parameters["@id"].Value = currentMedia.Id;
            removeMediaCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand checkLicenseStatusCommand;
        public StartupFailReason CheckLicenseStatus(string contextLicenseKey)
        {
            if (checkLicenseStatusCommand == null)
            {
                checkLicenseStatusCommand = connection.CreateCommand();
                checkLicenseStatusCommand.CommandText = "SELECT * FROM web.rest_licenses WHERE license = @key";
                checkLicenseStatusCommand.Parameters.Add("@key", NpgsqlDbType.Text);
            }

            checkLicenseStatusCommand.Parameters["@key"].Value = contextLicenseKey;
            NpgsqlDataReader dataReader = checkLicenseStatusCommand.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Close();
                return StartupFailReason.LicenseNotInDatabase;
            }

            int id = dataReader.GetInt32(0);
            string license = dataReader.GetString(1);
            string owner = dataReader.GetString(2);
            DateTime dateAdded = dataReader.GetDateTime(3);
            bool banned = dataReader.GetBoolean(4);
            

            bool multipleRows = dataReader.Read();
            dataReader.Close();
            if (multipleRows)
            {
                return StartupFailReason.LicenseHasMultipleMatches;
            }
            if (banned)
            {
                return StartupFailReason.LicenseRevoked;
            }

            return StartupFailReason.NoError;
        }

        private NpgsqlCommand activateLicenseCommand;
        public void ActivateLicense(string contextLicenseKey)
        {
            if (activateLicenseCommand == null)
            {
                activateLicenseCommand = connection.CreateCommand();
                activateLicenseCommand.CommandText =
                    "INSERT INTO web.rest_licenses (id,license,owner) VALUES ((SELECT MAX(id) FROM web.rest_licenses) + 1,@key,@owner)";
                activateLicenseCommand.Parameters.Add("@key", NpgsqlDbType.Text);
                activateLicenseCommand.Parameters.Add("@owner", NpgsqlDbType.Text);
            }

            activateLicenseCommand.Parameters["@key"].Value = contextLicenseKey;
            activateLicenseCommand.Parameters["@owner"].Value = String.Format("{0}", Environment.MachineName);
            activateLicenseCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand getAllMediaAttachmentTypesCommand;
        public IEnumerable<AttachmentType> GetAllMediaAttachmentTypes()
        {
            if (getAllMediaAttachmentTypesCommand == null)
            {
                getAllMediaAttachmentTypesCommand = connection.CreateCommand();
                getAllMediaAttachmentTypesCommand.CommandText = "SELECT * FROM azusa.media_attachment_types WHERE hidden = FALSE";
            }

            List<AttachmentType> result = new List<AttachmentType>();
            NpgsqlDataReader dataReader = getAllMediaAttachmentTypesCommand.ExecuteReader();
            while (dataReader.Read())
            {
                AttachmentType child = new AttachmentType();
                child.id = dataReader.GetInt32(0);
                child.dateadded = dataReader.GetDateTime(1);
                child.name = dataReader.GetString(2);
                child.displayControlUuid = dataReader.GetGuid(3);
                result.Add(child);
            }
            dataReader.Close();
            foreach (AttachmentType attachmentType in result)
                yield return attachmentType;
        }

        private NpgsqlCommand getAllMediaAttachmentsCommand;
        public IEnumerable<Attachment> GetAllMediaAttachments(Media currentMedia)
        {
            if (getAllMediaAttachmentsCommand == null)
            {
                getAllMediaAttachmentsCommand = connection.CreateCommand();
                getAllMediaAttachmentsCommand.CommandText = "SELECT * FROM azusa.media_attachments WHERE mediaid = @mid";
                getAllMediaAttachmentsCommand.Parameters.Add("@mid", NpgsqlDbType.Integer);
            }

            getAllMediaAttachmentsCommand.Parameters["@mid"].Value = currentMedia.Id;

            List<Attachment> result = new List<Attachment>();
            NpgsqlDataReader dataReader = getAllMediaAttachmentsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                Attachment child = new Attachment();
                child._MediaId = dataReader.GetInt32(0);
                child._TypeId = dataReader.GetInt32(1);
                child._Buffer = dataReader.GetByteArray(2);
                child._DateAdded = dataReader.GetDateTime(3);

                if (!dataReader.IsDBNull(4))
                    child._DateUpdated = dataReader.GetDateTime(4);

                child._Complete = dataReader.GetBoolean(5);
                child._Serial = dataReader.GetInt64(6);
                result.Add(child);
            }
            dataReader.Close();
            foreach (Attachment attachment in result)
                yield return attachment;
        }

        private NpgsqlCommand updateAttachmentCommand;
        public void UpdateAttachment(Attachment attachment)
        {
            if (updateAttachmentCommand == null)
            {
                updateAttachmentCommand = connection.CreateCommand();
                updateAttachmentCommand.CommandText =
                    "UPDATE azusa.media_attachments " +
                    "SET buffer = @buffer, completed = @completed, dateupdated = @dateupdated " +
                    "WHERE mediaid = @mediaid AND typeid = @typeid";
                updateAttachmentCommand.Parameters.Add("@buffer", NpgsqlDbType.Bytea);
                updateAttachmentCommand.Parameters.Add("@completed", NpgsqlDbType.Boolean);
                updateAttachmentCommand.Parameters.Add("@dateupdated", NpgsqlDbType.Timestamp);
                updateAttachmentCommand.Parameters.Add("@mediaid", NpgsqlDbType.Integer);
                updateAttachmentCommand.Parameters.Add("@typeid", NpgsqlDbType.Integer);
            }

            updateAttachmentCommand.Parameters["@buffer"].Value = attachment._Buffer;
            updateAttachmentCommand.Parameters["@completed"].Value = attachment._Complete;
            updateAttachmentCommand.Parameters["@dateupdated"].Value = attachment._DateUpdated;
            updateAttachmentCommand.Parameters["@mediaid"].Value = attachment._MediaId;
            updateAttachmentCommand.Parameters["@typeid"].Value = attachment._TypeId;
            updateAttachmentCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand insertAttachmentCommand;
        public void InsertAttachment(Attachment attachment)
        {
            if (insertAttachmentCommand == null)
            {
                insertAttachmentCommand = connection.CreateCommand();
                insertAttachmentCommand.CommandText =
                    "INSERT INTO azusa.media_attachments (mediaid,typeid,buffer,completed) VALUES (@mediaid,@typeid,@buffer,@completed)";
                insertAttachmentCommand.Parameters.Add("@buffer", NpgsqlDbType.Bytea);
                insertAttachmentCommand.Parameters.Add("@completed", NpgsqlDbType.Boolean);
                insertAttachmentCommand.Parameters.Add("@mediaid", NpgsqlDbType.Integer);
                insertAttachmentCommand.Parameters.Add("@typeid", NpgsqlDbType.Integer);
            }
            insertAttachmentCommand.Parameters["@buffer"].Value = attachment._Buffer;
            insertAttachmentCommand.Parameters["@completed"].Value = attachment._Complete;
            insertAttachmentCommand.Parameters["@mediaid"].Value = attachment._MediaId;
            insertAttachmentCommand.Parameters["@typeid"].Value = attachment._TypeId;
            insertAttachmentCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand deleteAttachmentCommand;
        public void DeleteAttachment(Attachment attachment)
        {
            if (deleteAttachmentCommand == null)
            {
                deleteAttachmentCommand = connection.CreateCommand();
                deleteAttachmentCommand.CommandText =
                    "DELETE FROM azusa.media_attachments WHERE mediaid = @mediaid AND typeid = @typeid";
                deleteAttachmentCommand.Parameters.Add("@mediaid", NpgsqlDbType.Integer);
                deleteAttachmentCommand.Parameters.Add("@typeid", NpgsqlDbType.Integer);
            }
            deleteAttachmentCommand.Parameters["@mediaid"].Value = attachment._MediaId;
            deleteAttachmentCommand.Parameters["@typeid"].Value = attachment._TypeId;
            deleteAttachmentCommand.ExecuteNonQuery();
        }
        
        private NpgsqlCommand getLatestEuroExchangeRatesCommand;
        public AzusifiedCube GetLatestEuroExchangeRates()
        {
            if (getLatestEuroExchangeRatesCommand == null)
            {
                getLatestEuroExchangeRatesCommand = connection.CreateCommand();
                getLatestEuroExchangeRatesCommand.CommandText = "SELECT * FROM azusa.euro_exchange_rates ORDER BY dateadded DESC";
            }

            NpgsqlDataReader dataReader = getLatestEuroExchangeRatesCommand.ExecuteReader();
            if (dataReader.Read())
            {
                AzusifiedCube cube = new AzusifiedCube();
                cube.DateAdded = dataReader.GetDateTime(0);
                cube.CubeDate = dataReader.GetDateTime(1);
                cube.USD = dataReader.GetDouble(2);
                cube.JPY = dataReader.GetDouble(3);
                cube.GBP = dataReader.GetDouble(4);
                dataReader.Close();
                return cube;
            }
            else
            {
                dataReader.Close();
                return null;
            }
        }

        private NpgsqlCommand insertEuroExchangeRateCommand;
        public void InsertEuroExchangeRate(AzusifiedCube cube)
        {
            if (insertEuroExchangeRateCommand == null)
            {
                insertEuroExchangeRateCommand = connection.CreateCommand();
                insertEuroExchangeRateCommand.CommandText =
                    "INSERT INTO azusa.euro_exchange_rates (cubedate, usd, jpy, gbp) VALUES (@cubedate,@usd,@jpy,@gbp)";
                insertEuroExchangeRateCommand.Parameters.Add("@cubedate", NpgsqlDbType.Date);
                insertEuroExchangeRateCommand.Parameters.Add("@usd", NpgsqlDbType.Double);
                insertEuroExchangeRateCommand.Parameters.Add("@jpy", NpgsqlDbType.Double);
                insertEuroExchangeRateCommand.Parameters.Add("@gbp", NpgsqlDbType.Double);
            }

            insertEuroExchangeRateCommand.Parameters["@cubedate"].Value = cube.CubeDate;
            insertEuroExchangeRateCommand.Parameters["@usd"].Value = cube.USD;
            insertEuroExchangeRateCommand.Parameters["@jpy"].Value = cube.JPY;
            insertEuroExchangeRateCommand.Parameters["@gbp"].Value = cube.GBP;
            insertEuroExchangeRateCommand.ExecuteNonQuery();
        }

        private NpgsqlCommand getLatestCryptoExchangeDateCommand;

        public DateTime GetLatestCryptoExchangeRateUpdateDate()
        {
            if (getLatestCryptoExchangeDateCommand == null)
            {
                getLatestCryptoExchangeDateCommand = connection.CreateCommand();
                getLatestCryptoExchangeDateCommand.CommandText = "SELECT dateadded FROM azusa.euro_exchange_rates_crypto ORDER BY dateadded DESC";
            }

            NpgsqlDataReader npgsqlDataReader = getLatestCryptoExchangeDateCommand.ExecuteReader();
            DateTime result = DateTime.MinValue;
            if (npgsqlDataReader.Read())
            {
                result = npgsqlDataReader.GetDateTime(0);
            }
            npgsqlDataReader.Close();
            return result;
        }

        private NpgsqlCommand insertCryptoExcangesCommand;
        public void InsertCryptoExchangeRate(CryptoExchangeRates exchangeRates)
        {
            if (insertCryptoExcangesCommand == null)
            {
                insertCryptoExcangesCommand = connection.CreateCommand();
                insertCryptoExcangesCommand.CommandText =
                    "INSERT INTO azusa.euro_exchange_rates_crypto (btc,ltc,doge,xch,eth) VALUES (@btc,@ltc,@doge,@xch,@eth)";
                insertCryptoExcangesCommand.Parameters.Add("@btc", NpgsqlDbType.Double);
                insertCryptoExcangesCommand.Parameters.Add("@ltc", NpgsqlDbType.Double);
                insertCryptoExcangesCommand.Parameters.Add("@doge", NpgsqlDbType.Double);
                insertCryptoExcangesCommand.Parameters.Add("@xch", NpgsqlDbType.Double);
                insertCryptoExcangesCommand.Parameters.Add("@eth", NpgsqlDbType.Double);
            }

            insertCryptoExcangesCommand.Parameters["@btc"].Value = exchangeRates.btc;
            insertCryptoExcangesCommand.Parameters["@ltc"].Value = exchangeRates.ltc;
            insertCryptoExcangesCommand.Parameters["@doge"].Value = exchangeRates.doge;
            insertCryptoExcangesCommand.Parameters["@xch"].Value = exchangeRates.xch;
            insertCryptoExcangesCommand.Parameters["@eth"].Value = exchangeRates.eth;
            insertCryptoExcangesCommand.ExecuteNonQuery();
        }
    }
}
