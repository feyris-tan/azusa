using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using libeuroexchange.Model;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Setup;
using moe.yo3explorer.azusa.DatabaseTasks;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.Properties;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Drivers
{
    class OfflineDriver : IDatabaseDriver
    {
        public OfflineDriver()
        {
            rootDirectory = new DirectoryInfo("azusaOffline");
            Initialize();
        }

        private void Initialize()
        {
            if (!rootDirectory.Exists)
                rootDirectory.Create();
            
            connectionBuilder = new SQLiteConnectionStringBuilder();
            connectionBuilder.DataSource = ":memory:";

            connections = new Dictionary<string, SQLiteConnection>();
        }

        private DirectoryInfo rootDirectory;
        private SQLiteConnectionStringBuilder connectionBuilder;
        private Dictionary<string, SQLiteConnection> connections;

        private SQLiteConnection GetConnectionForTable(string tableName)
        {
            string[] args = tableName.Split('.');
            if (!connections.ContainsKey(args[0]))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionBuilder.ToString());
                connection.Open();
                bool passworded = args[0].Equals("web");
                string sql = null;
                if (!passworded)
                    sql = String.Format("ATTACH DATABASE '{0}' AS {1}", Path.Combine(rootDirectory.FullName, args[0] + ".db"),
                        args[0]);
                else
                    sql = String.Format("ATTACH DATABASE '{0}' AS {1} KEY '{2}'", Path.Combine(rootDirectory.FullName, args[0] + ".db"), args[0], Environment.MachineName);
                SQLiteCommand command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                connections[args[0]] = connection;
            }

            return connections[args[0]];
        }
        
        public bool CanActivateLicense => false;
        public bool CanUpdateExchangeRates => false;

        public void BeginTransaction()
        {
            throw new NotSupportedException();
        }

        public bool ConnectionIsValid()
        {
            return true;
        }

        public int CreateMediaAndReturnId(int productId, string name)
        {
            throw new NotSupportedException();
        }

        public int CreateProductAndReturnId(Shelf shelf, string name)
        {
            throw new NotSupportedException();
        }
        
        public void Dispose()
        {
            foreach (KeyValuePair<string, SQLiteConnection> sqLiteConnection in connections)
            {
                sqLiteConnection.Value.Dispose();
            }
            connections.Clear();
        }

        public void EndTransaction(bool sucessful)
        {
            throw new NotSupportedException();
        }

        private Random rngInternal;
        
        private DbType GuessSqliteDbType(string s)
        {
            switch (s)
            {
                case "INTEGER":
                    return DbType.Int32;
                case "TIMESTAMP":
                    return DbType.DateTime;
                case "BOOLEAN":
                    return DbType.Boolean;
                case "TEXT":
                    return DbType.String;
                case "DOUBLE":
                    return DbType.Double;
                case "SMALLINT":
                    return DbType.Int16;
                case "BIGINT":
                    return DbType.Int64;
                case "BLOB":
                    return DbType.Binary;
                case "DATE":
                    return DbType.Date;
                default:
                    throw new NotImplementedException();
            }
        }

        private string GetTableNameForPragma(string tableName)
        {
            while (tableName.Contains("."))
                tableName = tableName.Substring(1);

            if (tableName.StartsWith("0"))
                tableName = String.Format("\"{0}\"", tableName);
            return tableName;
        }

        public List<DatabaseColumn> Sync_DefineTable(string tableName)
        {
            List<DatabaseColumn> columns = new List<DatabaseColumn>();
            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = String.Format("PRAGMA table_info({0})", GetTableNameForPragma(tableName));
            SQLiteDataReader sqLiteDataReader = command.ExecuteReader();
            while (sqLiteDataReader.Read())
            {
                DatabaseColumn outColumn = new DatabaseColumn();
                outColumn.Ordingal = sqLiteDataReader.GetInt32(0);
                outColumn.ColumnName = sqLiteDataReader.GetString(1);
                outColumn.DbType = GuessSqliteDbType(sqLiteDataReader.GetString(2));
                outColumn.ParameterName = String.Format("@{0}", Sync.randomString(outColumn.Ordingal + 1));
                columns.Add(outColumn);
            }
            sqLiteDataReader.Close();
            return columns;
        }

        public bool Sync_DoesTableExist(string tableName)
        {
            string[] args = tableName.Split('.');
            SQLiteConnection connection = GetConnectionForTable(tableName);

            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = String.Format("SELECT rootpage FROM {0}.sqlite_master WHERE tbl_name = @tblName;", args[0]);
            command.Parameters.Add("@tblName", DbType.String);
            command.Parameters["@tblName"].Value = args[1];
            SQLiteDataReader dataReader = command.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Close();
            dataReader.Dispose();
            command.Dispose();
            return result;
        }

        public void Sync_CreateTable(string tableName, List<DatabaseColumn> columns)
        {
            tableName = tableName.MakeFullyQualifiedTableName();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("CREATE TABLE {0} (", tableName);
            for (int i = 0; i < columns.Count; i++)
            {
                sb.AppendFormat("{0} {1}", columns[i].ColumnName, GuessSqliteDbType(columns[i].DbType));
                if (i != (columns.Count - 1))
                    sb.Append(",");
            }

            sb.Append(")");

            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = sb.ToString();
            command.ExecuteNonQuery();
        }

        public string GuessSqliteDbType(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Int16:
                    return "SMALLINT";
                case DbType.String:
                    return "TEXT";
                case DbType.DateTime:
                    return "TIMESTAMP";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.Int64:
                    return "BIGINT";
                case DbType.Boolean:
                    return "BOOLEAN";
                case DbType.Date:
                    return "DATE";
                case DbType.Binary:
                    return "BLOB";
                case DbType.Double:
                    return "DOUBLE";
                case DbType.Time:
                    return "BIGINT";
                case DbType.StringFixedLength:
                    return "TEXT";
                case DbType.AnsiString:
                    return "TEXT";
                default:
                    throw new NotImplementedException(dbType.ToString());
            }
        }

        public DateTime? Sync_GetLastSyncDateForTable(string tableName)
        {
            tableName = tableName.MakeFullyQualifiedTableName();
            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = String.Format("SELECT MAX(dateAdded) FROM {0}", tableName);
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            dataReader.Read();
            DateTime? result;
            if (dataReader.IsDBNull(0))
                result = null;
            else
                result = dataReader.GetDateTime(0);
            dataReader.Close();
            dataReader.Dispose();
            cmd.Dispose();
            return result;
        }

        public DbDataReader Sync_GetSyncReader(string tableName, DateTime? latestSynced)
        {
            throw new NotImplementedException();
        }
        
        public void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader,
            SyncLogMessageCallback onMessage)
        {
            tableName = tableName.MakeFullyQualifiedTableName();
            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand cmd = connection.CreateCommand();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("INSERT INTO {0} (", tableName);
            for (int i = 0; i < columns.Count; i++)
            {
                sb.AppendFormat("{0}", columns[i].ColumnName);
                if (i != columns.Count - 1)
                    sb.AppendFormat(", ");
            }

            sb.Append(") VALUES (");
            for (int i = 0; i < columns.Count; i++)
            {
                sb.AppendFormat("{0}", columns[i].ParameterName);
                cmd.Parameters.Add(columns[i].ParameterName, columns[i].DbType);
                if (i != columns.Count - 1)
                    sb.AppendFormat(", ");
            }
            sb.Append(")");
            cmd.CommandText = sb.ToString();

            SQLiteTransaction transaction = connection.BeginTransaction();
            DatabaseColumn idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("id"));
            if (idColumn == null)
            {
                idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("screenid"));
            }
            if (idColumn == null)
            {
                idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("uid"));
            }
            if (idColumn == null)
            {
                idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("scalerid"));
            }
            if (idColumn == null)
            {
                idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("char_id"));
            }
            if (idColumn == null)
            {
                idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("serial"));
            }
            int successful = 0;

            while (syncReader.Read())
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    DatabaseColumn currentColumn = columns[i];
                    if (currentColumn.DbType == DbType.Time)
                    {
                        TimeSpan copyTimespan = (TimeSpan)syncReader.GetValue(i);
                        DateTime copyTimespan2 = DateTime.MinValue;
                        copyTimespan2 += copyTimespan;
                        cmd.Parameters[currentColumn.ParameterName].Value = copyTimespan2;
                    }
                    else if (currentColumn.DbType == DbType.Binary)
                    {
                        cmd.Parameters[currentColumn.ParameterName].Value = DBNull.Value;
                        byte[] inBuffer = syncReader.GetByteArray(i);
                        if (inBuffer != null && inBuffer.Length > 0)
                        {
                            cmd.Parameters[currentColumn.ParameterName].Value = inBuffer;
                        }
                    }
                    else
                    {
                        object copyObject = syncReader.GetValue(i);
                        cmd.Parameters[currentColumn.ParameterName].Value = copyObject;
                    }
                    
                }
                int result = cmd.ExecuteNonQuery();
                if (result == 0)
                    throw new SyncException("insert failed");

                if (successful++ % 100 == 0)
                {
                    onMessage.Invoke(String.Format("{0} inserts into {1}", successful, tableName));
                }
            }
            syncReader.Close();
            syncReader.Dispose();
            transaction.Commit();
            transaction.Dispose();
        }
        
        public DateTime? Sync_GetLatestUpdateForTable(string tableName)
        {
            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = String.Format("SELECT MAX(dateupdated) FROM {0}", tableName);
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            DateTime? result;
            if (dataReader.Read())
            {
                if (!dataReader.IsDBNull(0))
                    result = dataReader.GetDateTime(0);
                else
                    result = null;
            }
            else
            {
                result = null;
            }
            dataReader.Dispose();
            return result;
        }

        public DbDataReader Sync_GetUpdateSyncReader(string tableName, DateTime? latestUpdate)
        {
            throw new NotImplementedException();
        }

        public void Sync_CopyUpdatesFrom(string tableName, List<DatabaseColumn> originalColumns, DbDataReader syncReader, SyncLogMessageCallback onMessage,Queue<object> leftovers)
        {
            List<DatabaseColumn> usableColumns = new List<DatabaseColumn>();
            usableColumns.AddRange(originalColumns);

            DatabaseColumn idColumn = Sync.GetIdColumn(usableColumns);
            usableColumns.Remove(idColumn);

            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand cmd = connection.CreateCommand();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("UPDATE {0} SET ", tableName);
            for (int i = 0; i < usableColumns.Count; i++)
            {
                cmd.Parameters.Add(usableColumns[i].ParameterName, usableColumns[i].DbType);
                sb.AppendFormat("{0} = {1}", usableColumns[i].ColumnName, usableColumns[i].ParameterName);
                if (i != usableColumns.Count - 1)
                    sb.AppendFormat(", ");
            }

            int updatesDone = 0;
            int nextProgress = 1;

            cmd.Parameters.Add(idColumn.ParameterName, idColumn.DbType);
            sb.AppendFormat(" WHERE {0} = {1}", idColumn.ColumnName, idColumn.ParameterName);
            cmd.CommandText = sb.ToString();

            while (syncReader.Read())
            {
                foreach (DatabaseColumn column in usableColumns)
                {
                    if (column.DbType == DbType.Binary)
                    {
                        byte[] buffer = syncReader.GetByteArray(column.Ordingal);
                        if (buffer != null)
                        {
                            cmd.Parameters[column.ParameterName].Value = buffer;
                        }
                    }
                    else
                    {
                        object o = syncReader.GetValue(column.Ordingal);
                        cmd.Parameters[column.ParameterName].Value = o;
                    }
                    
                }

                cmd.Parameters[idColumn.ParameterName].Value = syncReader.GetValue(idColumn.Ordingal);
                int result = cmd.ExecuteNonQuery();
                switch (result)
                {
                    case 0:
                        leftovers.Enqueue(cmd.Parameters[idColumn.ParameterName].Value);
                        break;
                    case 1:
                        Console.WriteLine("update ok: {0} {1}", tableName, syncReader.GetValue(idColumn.Ordingal));
                        break;
                    default:
                        Console.WriteLine("update failed: {0} {1}", tableName, syncReader.GetValue(idColumn.Ordingal));
                        break;
                }

                if (updatesDone++ == nextProgress)
                {
                    onMessage.Invoke(String.Format("{0} Zeilen in {1} aktualisiert.", nextProgress, tableName));
                    nextProgress *= 2;
                }
            }
            cmd.Dispose();
            syncReader.Dispose();
        }
        
        
        private XmlSerializer indexXmlSerializer;
        private FileInfo indexXmlFileInfo;
        private List<SqlIndex> sqlIndexes;

        private void PrepareIndexXmlSerializer()
        {
            if (indexXmlSerializer == null)
            {
                indexXmlSerializer = new XmlSerializer(typeof(List<SqlIndex>));
                indexXmlFileInfo = new FileInfo(Path.Combine(this.rootDirectory.FullName, "indexers.xml"));
                if (indexXmlFileInfo.Exists)
                {
                    FileStream inStream = indexXmlFileInfo.OpenRead();
                    sqlIndexes = (List<SqlIndex>)indexXmlSerializer.Deserialize(inStream);
                    inStream.Dispose();
                }
                else
                {
                    sqlIndexes = new List<SqlIndex>();
                }
            }
        }

        public IEnumerable<SqlIndex> GetSqlIndexes()
        {
            PrepareIndexXmlSerializer();
            return sqlIndexes;
        }

        public void CreateIndex(SqlIndex index)
        {
            if (index.TableName.ContainsDigits())
            {
                index.TableName = String.Format("\"{0}\"", index.TableName);
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("CREATE ");
            if (index.Unique)
                stringBuilder.Append("UNIQUE ");
            stringBuilder.AppendFormat("INDEX IF NOT EXISTS {1}.{0} ", index.IndexName, index.SchemaName);
            stringBuilder.AppendFormat("ON {0} (", index.TableName);
            for (int i = 0; i < index.Columns.Length; i++)
            {
                if (i > 0)
                    stringBuilder.Append(",");
                stringBuilder.Append(index.Columns[i]);
            }
            stringBuilder.Append(")");

            SQLiteConnection connection = GetConnectionForTable(String.Format("{0}.{1}",index.SchemaName,index.TableName));
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = stringBuilder.ToString();
            int result = command.ExecuteNonQuery();
            Debug.WriteLine("CREATE INDEX affected {0} rows", result);

            PrepareIndexXmlSerializer();
            sqlIndexes.Add(index);
            FileStream outStream = indexXmlFileInfo.OpenWrite();
            indexXmlSerializer.Serialize(outStream, sqlIndexes);
            outStream.Flush(true);
            outStream.Close();
        }

        public void ForgetFilesystemContents(int currentMediaId)
        {
            throw new NotSupportedException();
        }

        public void AddFilesystemInfo(FilesystemMetadataEntity dirEntity)
        {
            throw new NotSupportedException();
        }

        private SQLiteCommand getFilesystemMetadataCommand;
        public IEnumerable<FilesystemMetadataEntity> GetFilesystemMetadata(int currentMediaId, bool dirs)
        {
            if (getFilesystemMetadataCommand == null)
            {
                getFilesystemMetadataCommand = GetConnectionForTable("azusa.filesysteminfo").CreateCommand();
                getFilesystemMetadataCommand.CommandText =
                    "SELECT * FROM azusa.filesysteminfo " +
                    "WHERE mediaid = @mediaid " +
                    "AND isdirectory = @isdirectory " +
                    "ORDER BY parent ASC";
                getFilesystemMetadataCommand.Parameters.Add("@mediaid", DbType.Int32);
                getFilesystemMetadataCommand.Parameters.Add("@isdirectory", DbType.Boolean);
            }

            getFilesystemMetadataCommand.Parameters["@mediaid"].Value = currentMediaId;
            getFilesystemMetadataCommand.Parameters["@isdirectory"].Value = dirs;
            SQLiteDataReader dataReader = getFilesystemMetadataCommand.ExecuteReader();
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
                {
                    child.Header = dataReader.GetByteArray(7);
                }

                child.ParentId = dataReader.GetInt64(8);
                yield return child;
            }
            dataReader.Close();
        }
        
        public DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query)
        {
            throw new NotImplementedException();
        }
        
        public void CreateSchema(string schemaName)
        {
            throw new NotImplementedException();
        }

        public void MoveAndRenameTable(string oldSchemaName, string oldTableName, string schemaName, string newTableName)
        {
            throw new NotImplementedException();
        }
        
        public Media[] findBrokenBandcampImports()
        {
            throw new NotImplementedException();
        }

        public Media[] FindAutofixableMetafiles()
        {
            throw new NotImplementedException();
        }

        public void Sync_AlterTable(string tableName, DatabaseColumn missingColumn)
        {
            /*alter table sqlite_master add yeet int;*/
            SQLiteConnection connection = GetConnectionForTable(tableName);
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = String.Format("ALTER TABLE {0} ADD {1} {2}", tableName, missingColumn.ColumnName, GuessSqliteDbType(missingColumn.DbType));
            command.ExecuteNonQuery();
        }

        public void RemoveMedia(Media currentMedia)
        {
            throw new NotImplementedException();
        }

        private SQLiteCommand sqliteCommand;
        public StartupFailReason CheckLicenseStatus(string contextLicenseKey)
        {
            if (sqliteCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("web.rest_licenses");
                sqliteCommand = connection.CreateCommand();
                sqliteCommand.CommandText = "SELECT * FROM web.rest_licenses WHERE license=@license";
                sqliteCommand.Parameters.Add("@license", DbType.String);
            }

            sqliteCommand.Parameters["@license"].Value = contextLicenseKey;
            SQLiteDataReader dataReader = sqliteCommand.ExecuteReader();
            if (dataReader.Read())
            {
                long licId = dataReader.GetInt64(0);
                string license = dataReader.GetString(1);
                string owner = dataReader.GetString(2);
                DateTime dateAdded = dataReader.GetDateTime(3);
                bool banned = dataReader.GetBoolean(4);
                dataReader.Close();
                return banned ? StartupFailReason.LicenseRevoked : StartupFailReason.NoError;
            }
            else
            {
                dataReader.Close();
                return StartupFailReason.LicenseNotInDatabase;
            }
        }

        public void ActivateLicense(string contextLicenseKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AttachmentType> GetAllMediaAttachmentTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Attachment> GetAllMediaAttachments(Media currentMedia)
        {
            throw new NotImplementedException();
        }

        public void UpdateAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public void InsertAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public void DeleteAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        private SQLiteCommand getLatestEuroExchangeRatesCommand;
        public AzusifiedCube GetLatestEuroExchangeRates()
        {
            if (getLatestEuroExchangeRatesCommand == null)
            {
                SQLiteConnection conn = GetConnectionForTable("azusa.euro_exchange_rates");
                getLatestEuroExchangeRatesCommand = conn.CreateCommand();
                getLatestEuroExchangeRatesCommand.CommandText = "SELECT * FROM euro_exchange_rates ORDER BY dateAdded DESC";
            }

            SQLiteDataReader dataReader = getLatestEuroExchangeRatesCommand.ExecuteReader();
            AzusifiedCube cube = new AzusifiedCube();
            if (dataReader.Read())
            {
                cube.CubeDate = dataReader.GetDateTime(0);
                cube.DateAdded = dataReader.GetDateTime(1);
                cube.USD = dataReader.GetDouble(2);
                cube.JPY = dataReader.GetDouble(3);
                cube.GBP = dataReader.GetDouble(4);
                dataReader.Close();
                return cube;
            }
            else
            {
                dataReader.Close();
                cube.CubeDate = new DateTime(2021, 4, 5);
                cube.DateAdded = DateTime.Now;
                cube.USD = 1.1746;
                cube.JPY = 130.03;
                cube.GBP = 0.85195;
                return cube;
            }
        }

        public void InsertEuroExchangeRate(AzusifiedCube cube)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLatestCryptoExchangeRateUpdateDate()
        {
            throw new NotImplementedException();
        }

        public void InsertCryptoExchangeRate(CryptoExchangeRates exchangeRates)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetAllCountries()
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.countries");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.countries";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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
                    country.BlurayRegion = dataReader.GetString(9)[0];
                if (!dataReader.IsDBNull(10))
                    country.DateAdded = dataReader.GetDateTime(10);
                yield return country;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.platforms");
            SQLiteCommand command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM azusa.platforms";
            SQLiteDataReader dataReader = command.ExecuteReader();
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

        public IEnumerable<Shelf> GetAllShelves()
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.shelves");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.shelves";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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

        public IEnumerable<Shop> GetAllShops()
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.shops");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.shops";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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

        public List<string> GetAllPublicTableNames()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllTableNames()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllSchemas()
        {
            throw new NotImplementedException();
        }
        
        private SQLiteCommand getMediaByIdCommand;
        public Media GetMediaById(int o)
        {
            if (getMediaByIdCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("azusa.media");
                getMediaByIdCommand = connection.CreateCommand();
                getMediaByIdCommand.CommandText = "SELECT * FROM azusa.media WHERE id=@id";
                getMediaByIdCommand.Parameters.Add(new SQLiteParameter("@id", DbType.Int32));
            }

            getMediaByIdCommand.Parameters["@id"].Value = o;
            SQLiteDataReader ndr = getMediaByIdCommand.ExecuteReader();
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

        public IEnumerable<MediaInProduct> GetMediaByProduct(Product prod)
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.media");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetMediaByProduct_Postgre;
            cmd.Parameters.Add("@productId", DbType.Int32);
            cmd.Parameters["@productId"].Value = prod.Id;
            SQLiteDataReader ndr = cmd.ExecuteReader();
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

        public IEnumerable<MediaType> GetMediaTypes()
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.mediaTypes");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.mediaTypes";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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
                if (!dataReader.IsDBNull(8))
                    mt.HasFilesystem = dataReader.GetBoolean(8);
                yield return mt;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public Product GetProductById(int id)
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.products");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM azusa.products WHERE id = @id";
            cmd.Parameters.Add("@id", DbType.Int32);
            cmd.Parameters["@id"].Value = id;
            SQLiteDataReader ndr = cmd.ExecuteReader();
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

        public IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf)
        {
            SQLiteConnection connection = GetConnectionForTable("azusa.products");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = Resources.GetProductsInShelf_Postgre;
            cmd.Parameters.Add("@shelf", DbType.Int32);
            cmd.Parameters["@shelf"].Value = shelf.Id;
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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
                    product.ScreenshotSize = dataReader.GetInt64(4);
                if (!dataReader.IsDBNull(5))
                    product.CoverSize = dataReader.GetInt64(5);

                if (!dataReader.IsDBNull(6))
                    product.NSFW = dataReader.GetBoolean(6);
                if (dataReader.IsDBNull(7))
                {
                    Debug.WriteLine(product.Name + " has no media!");
                    continue;
                }

                product.IconId = Convert.ToInt32(dataReader.GetInt64(7));
                product.NumberOfDiscs = dataReader.GetInt32(8);
                product.ContainsUndumped = dataReader.GetInt32(9) > 0;
                product.MissingGraphData = dataReader.GetInt32(10);
                yield return product;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }
        
        public void SetCover(Product product)
        {
            throw new NotSupportedException();
        }

        public void SetScreenshot(Product product)
        {
            throw new NotSupportedException();
        }

        public int Statistics_GetTotalMedia()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingCovers()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingGraphData()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingScreenshots()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalProducts()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalUndumpedMedia()
        {
            throw new NotImplementedException();
        }

        public void Statistics_Insert(DateTime today, int totalProducts, int totalMedia, int missingCover,
            int missingGraph, int undumped, int missingScreenshots)
        {
            throw new NotImplementedException();
        }

        public bool Statistics_TestForDate(DateTime today)
        {
            throw new NotImplementedException();
        }

        public void UpdateMedia(Media media)
        {
            throw new NotSupportedException();
        }

        public void UpdateProduct(Product product)
        {
            throw new NotSupportedException();
        }
        
    }
}
