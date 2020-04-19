using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using libazustreamblob;
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
using Org.BouncyCastle.Math;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Drivers
{
    class OfflineDriver : IDatabaseDriver, IStreamBlobOwner
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

            azuStreamBlob = new AzusaStreamBlob(rootDirectory);
            connectionBuilder = new SQLiteConnectionStringBuilder();
            connectionBuilder.DataSource = ":memory:";

            connections = new Dictionary<string, SQLiteConnection>();
        }

        private DirectoryInfo rootDirectory;
        private AzusaStreamBlob azuStreamBlob;
        private SQLiteConnectionStringBuilder connectionBuilder;
        private Dictionary<string, SQLiteConnection> connections;

        private SQLiteConnection GetConnectionForTable(string tableName)
        {
            string[] args = tableName.Split('.');
            if (!connections.ContainsKey(args[0]))
            {
                SQLiteConnection connection = new SQLiteConnection(connectionBuilder.ToString());
                connection.Open();
                bool passworded = args[0].Equals("licensing");
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

        public bool TransactionSupported => false;

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

        private SQLiteCommand getAllManualGlucoseValuesCommand;
        public IEnumerable<ManualDataEntity> Dexcom_GetAllManualGlucoseValues()
        {
            if (getAllManualGlucoseValuesCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dexcom.manualdata");
                getAllManualGlucoseValuesCommand = connection.CreateCommand();
                getAllManualGlucoseValuesCommand.CommandText = "SELECT * FROM dexcom.manualdata ORDER BY ts ASC";
            }
            SQLiteDataReader dataReader = getAllManualGlucoseValuesCommand.ExecuteReader();
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

        public IEnumerable<DateTime> Dexcom_GetDates()
        {
            SQLiteConnection connection = GetConnectionForTable("dexcom.history");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT DISTINCT date FROM dexcom.history ORDER BY date ASC";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetDateTime(0);
            dataReader.Dispose();
            cmd.Dispose();
        }

        public IEnumerable<DexTimelineEntry> Dexcom_GetTimelineEntries(DateTime day)
        {
            SQLiteConnection connection = GetConnectionForTable("dexcom.history");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM dexcom.history WHERE date=@date";
            cmd.Parameters.Add("@date", DbType.Date);
            cmd.Parameters["@date"].Value = day.Date;
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                DateTime date = dataReader.GetDateTime(0);
                DateTime time = dataReader.GetDateTime(1);
                DateTime timestamp = new DateTime(date.Ticks + time.Ticks);
                DexTimelineEntry timelineEntry = new DexTimelineEntry();
                timelineEntry.Timestamp = timestamp;
                if (!dataReader.IsDBNull(2))
                    timelineEntry.SensorFiltered = (uint)dataReader.GetInt32(2);
                if (!dataReader.IsDBNull(3))
                    timelineEntry.SensorUnfiltered = (uint)dataReader.GetInt32(3);
                if (!dataReader.IsDBNull(4))
                    timelineEntry.Rssi = (uint)dataReader.GetInt32(4);
                if (!dataReader.IsDBNull(5))
                    timelineEntry.Glucose = (uint)dataReader.GetInt32(5);
                if (!dataReader.IsDBNull(6))
                    timelineEntry.TrendArrow = (TrendArrow)dataReader.GetInt16(6);
                if (!dataReader.IsDBNull(7))
                    timelineEntry.SessionState = (SessionState)dataReader.GetInt16(7);
                if (!dataReader.IsDBNull(8))
                    timelineEntry.MeterGlucose = (uint)dataReader.GetInt16(8);
                if (!dataReader.IsDBNull(9))
                    timelineEntry.EventType = (EventType)dataReader.GetInt16(9);
                if (!dataReader.IsDBNull(10))
                    timelineEntry.Carbs = dataReader.GetDouble(10);
                if (!dataReader.IsDBNull(11))
                    timelineEntry.Insulin = dataReader.GetDouble(11);

                if (!dataReader.IsDBNull(12))
                {
                    byte temp = dataReader.GetByte(12);
                    if (timelineEntry.EventType == EventType.Exercise)
                        timelineEntry.ExerciseEvent = (ExerciseSubType)temp;
                    else if (timelineEntry.EventType == EventType.Health)
                        timelineEntry.HealthEvent = (HealthSubType)temp;
                }

                if (!dataReader.IsDBNull(13))
                    timelineEntry.SpecialGlucoseValue = (SpecialGlucoseValue)dataReader.GetInt32(13);
                yield return timelineEntry;
            }
            dataReader.Dispose();
            cmd.Dispose();
        }

        public bool Dexcom_InsertTimestamp(DexTimelineEntry entry)
        {
            throw new NotSupportedException();
        }

        public void Dexcom_ManualGlucoseValueStore(DateTime timestamp, short value, string unit)
        {
            throw new NotSupportedException();
        }

        public bool Dexcom_ManualGlucoseValueTestForTimestamp(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public void Dexcom_ManualGlucoseValueUpdate(int id, byte be, byte novorapid, byte levemir, string note)
        {
            throw new NotSupportedException();
        }

        private SQLiteCommand testForTimestamp;
        public bool Dexcom_TestForTimestamp(DateTime theDate, DateTime theTime)
        {
            if (testForTimestamp == null)
            {
                testForTimestamp = GetConnectionForTable("dexcom.history").CreateCommand();
                testForTimestamp.CommandText = "SELECT dateAdded FROM dexcom.history WHERE date=@date AND time=@time";
                testForTimestamp.Parameters.Add("@date", DbType.Date);
                testForTimestamp.Parameters.Add("@time", DbType.Time);
            }

            testForTimestamp.Parameters["@date"].Value = theDate.Date;
            testForTimestamp.Parameters["@time"].Value = theTime.TimeOfDay;
            SQLiteDataReader dataReader = testForTimestamp.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Close();
            return result;
        }

        public void Dispose()
        {
            azuStreamBlob.Dispose();
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

        private Random RNG
        {
            get
            {
                if (rngInternal == null)
                {
                    rngInternal = new Random();
                }
                return rngInternal;
            }
        }

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

        public DbCommand Sync_GetWriteCommand(string tableName, List<DatabaseColumn> columns)
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
                        int keyA = azuStreamBlob.DeriveKey(tableName);
                        int keyB = azuStreamBlob.DeriveKey(currentColumn.ColumnName);
                        if (!syncReader.IsDBNull(idColumn.Ordingal))
                        {
                            int keyC = syncReader.GetInt32(idColumn.Ordingal);
                            azuStreamBlob.Put(keyA, keyB, keyC, inBuffer);
                        }
                        else
                        {
                            object pk = syncReader.GetValue(0);
                            Console.WriteLine("Failed to insert row, lacking scalerid: " + pk);
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

        private SQLiteCommand checkLicense;
        public LicenseState CheckLicenseStatus(byte[] uid)
        {
            string val = BitConverter.ToString(uid);
            SQLiteConnection connection = GetConnectionForTable("licensing.fatclient_machines");

            if (checkLicense == null)
            {
                checkLicense = connection.CreateCommand();
                checkLicense.CommandText = "SELECT state FROM licensing.fatclient_machines WHERE uid=@uid";
                checkLicense.Parameters.Add("@uid", DbType.String);
            }

            checkLicense.Parameters["@uid"].Value = val;
            SQLiteDataReader dataReader = checkLicense.ExecuteReader();
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
                return LicenseState.MachineHasNoLicense;
            }
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
                        int rowId = syncReader.GetInt32(idColumn.Ordingal);
                        int currentLen = azuStreamBlob.GetSize(azuStreamBlob.DeriveKey(tableName), azuStreamBlob.DeriveKey(column.ColumnName), rowId);
                        if (buffer != null)
                            if (buffer.Length != currentLen)
                                azuStreamBlob.Put(azuStreamBlob.DeriveKey(tableName), azuStreamBlob.DeriveKey(column.ColumnName), rowId, buffer);
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

        public IEnumerable<Note> Notebook_GetAllNotes()
        {
            SQLiteConnection connection = GetConnectionForTable("notebook.notes");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, iscategory, parent, name FROM notebook.notes";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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

        private SQLiteCommand createNote;
        public Note Notebook_CreateNote(string name, bool isCategory, int? parent)
        {
            SQLiteConnection connection = GetConnectionForTable("notebook.notes");
            if (createNote == null)
            {
                createNote = connection.CreateCommand();
                createNote.CommandText = "INSERT INTO notebook.notes (id,dateadded,iscategory,parent,name) VALUES (@id,@dateadded,@iscategory,@parent,@name)";
                createNote.Parameters.Add("@id", DbType.Int32);
                createNote.Parameters.Add("@dateadded", DbType.DateTime);
                createNote.Parameters.Add("@iscategory", DbType.Boolean);
                createNote.Parameters.Add("@parent", DbType.Int32);
                createNote.Parameters.Add("@name", DbType.String);
            }

            Note result = new Note();
            result.id = AzusaContext.GetInstance().RandomNumberGenerator.Next();
            result.isCategory = isCategory;
            result.name = name;
            result.parent = parent;

            createNote.Parameters["@id"].Value = result.id;
            createNote.Parameters["@dateAdded"].Value = DateTime.Now;
            createNote.Parameters["@iscategory"].Value = isCategory;

            if (parent.HasValue)
                createNote.Parameters["@parent"].Value = parent.Value;
            else
                createNote.Parameters["@parent"].Value = DBNull.Value;

            createNote.Parameters["@name"].Value = name;
            if (createNote.ExecuteNonQuery() != 1)
                throw new Exception("note creation failed");

            return result;
        }

        private SQLiteCommand notebookGetText;
        public string Notebook_GetRichText(int noteId)
        {
            if (notebookGetText == null)
            {
                SQLiteConnection connection = GetConnectionForTable("notebook.notes");
                notebookGetText = connection.CreateCommand();
                notebookGetText.CommandText = "SELECT richtext FROM notebook.notes WHERE id=@id";
                notebookGetText.Parameters.Add("@id", DbType.Int32);
            }

            notebookGetText.Parameters["@id"].Value = noteId;
            SQLiteDataReader dataReader = notebookGetText.ExecuteReader();
            dataReader.Read();
            string result;
            if (dataReader.IsDBNull(0))
                result = "";
            else
                result = dataReader.GetString(0);
            dataReader.Dispose();
            return result;
        }

        private SQLiteCommand updateNotebookNote;
        public void Notebook_UpdateNote(int currentNoteId, string text)
        {
            if (updateNotebookNote == null)
            {
                SQLiteConnection connection = GetConnectionForTable("notebook.notes");
                updateNotebookNote = connection.CreateCommand();
                updateNotebookNote.CommandText =
                    "UPDATE notebook.notes SET richText=@richText, dateUpdated=@dateUpdated WHERE id=@id";
                updateNotebookNote.Parameters.Add("@richText", DbType.String);
                updateNotebookNote.Parameters.Add("@dateUpdated", DbType.DateTime);
                updateNotebookNote.Parameters.Add("@id", DbType.Int32);
            }

            updateNotebookNote.Parameters["@richText"].Value = text;
            updateNotebookNote.Parameters["@dateUpdated"].Value = DateTime.Now;
            updateNotebookNote.Parameters["@id"].Value = currentNoteId;
            if (updateNotebookNote.ExecuteNonQuery() != 1)
                throw new Exception("update failed");
        }

        private SQLiteCommand vgmdbSearchTrackTranslation;
        public IEnumerable<int> Vgmdb_FindAlbumsByTrackMask(string text)
        {
            if (vgmdbSearchTrackTranslation == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_disc_track_translation");
                vgmdbSearchTrackTranslation = connection.CreateCommand();
                vgmdbSearchTrackTranslation.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_disc_track_translation WHERE name LIKE @name";
                vgmdbSearchTrackTranslation.Parameters.Add("@name", DbType.String);
            }

            vgmdbSearchTrackTranslation.Parameters["@name"].Value = text;
            SQLiteDataReader ndr = vgmdbSearchTrackTranslation.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private SQLiteCommand vgmFindAlbumsByArbituraryProducts;
        public IEnumerable<int> Vgmdb_FindAlbumsByArbituraryProducts(string text)
        {
            if (vgmFindAlbumsByArbituraryProducts == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_arbituaryproducts");
                vgmFindAlbumsByArbituraryProducts = connection.CreateCommand();
                vgmFindAlbumsByArbituraryProducts.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_arbituaryproducts WHERE name LIKE @name";
                vgmFindAlbumsByArbituraryProducts.Parameters.Add("@name", DbType.String);
            }

            vgmFindAlbumsByArbituraryProducts.Parameters["@name"].Value = text;
            SQLiteDataReader ndr = vgmFindAlbumsByArbituraryProducts.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private SQLiteCommand vgmFindAlbumsByAlbumTitle;
        public IEnumerable<int> Vgmdb_FindAlbumsByAlbumTitle(string text)
        {
            if (vgmFindAlbumsByAlbumTitle == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_titles");
                vgmFindAlbumsByAlbumTitle = connection.CreateCommand();
                vgmFindAlbumsByAlbumTitle.CommandText =
                    "SELECT DISTINCT albumid FROM dump_vgmdb.album_titles WHERE title LIKE @name";
                vgmFindAlbumsByAlbumTitle.Parameters.Add("@name", DbType.String);
            }

            vgmFindAlbumsByAlbumTitle.Parameters["@name"].Value = text;
            SQLiteDataReader ndr = vgmFindAlbumsByAlbumTitle.ExecuteReader();
            while (ndr.Read())
            {
                yield return ndr.GetInt32(0);
            }
            ndr.Dispose();
        }

        private SQLiteCommand vgmFindAlbumForList;
        public AlbumListEntry Vgmdb_FindAlbumForList(int id)
        {
            if (vgmFindAlbumForList == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.albums");
                vgmFindAlbumForList = connection.CreateCommand();
                vgmFindAlbumForList.CommandText = Resources.VgmDbFindAlbumForList_Postgre;
                vgmFindAlbumForList.Parameters.Add("@id", DbType.Int32);
            }

            vgmFindAlbumForList.Parameters["@id"].Value = id;
            SQLiteDataReader dataReader = vgmFindAlbumForList.ExecuteReader();
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

                result.classificationName = dataReader.GetString(4);
                result.mediaformatName = dataReader.GetString(5);
                result.name = dataReader.GetString(6);
                result.publishformatName = dataReader.GetString(7);
                result.notes = dataReader.GetString(8);
                if (!dataReader.IsDBNull(9))
                    result.publisher = dataReader.GetString(9);
            }
            dataReader.Dispose();
            return result;
        }

        public Bitmap Vgmdb_GetAlbumCover(int entryId)
        {
            byte[] blob = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_vgmdb.albums"), azuStreamBlob.DeriveKey("picture_full"), entryId);

            Bitmap result = null;
            if (blob != null)
                if (blob.Length > 0)
                {
                    MemoryStream ms = new MemoryStream(blob, false);
                    result = (Bitmap)Image.FromStream(ms);
                    ms.Dispose();
                }

            return result;
        }

        private SQLiteCommand vgmdbGetArbitraryProductNamesByAlbumId;
        private SQLiteCommand vgmdbGetProductNamesByAlbumId;
        public IEnumerable<string> Vgmdb_FindProductNamesByAlbumId(int entryId)
        {
            if (vgmdbGetArbitraryProductNamesByAlbumId == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_arbituaryproducts");
                vgmdbGetArbitraryProductNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArbitraryProductNamesByAlbumId.CommandText = "SELECT name FROM dump_vgmdb.album_arbituaryproducts WHERE albumid = @id";
                vgmdbGetArbitraryProductNamesByAlbumId.Parameters.Add("@id", DbType.Int32);

                vgmdbGetProductNamesByAlbumId = connection.CreateCommand();
                vgmdbGetProductNamesByAlbumId.CommandText =
                    "SELECT prod.name FROM dump_vgmdb.product_albums root JOIN dump_vgmdb.products prod ON root.productid = prod.id WHERE root.albumid = @id";
                vgmdbGetProductNamesByAlbumId.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbGetArbitraryProductNamesByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbGetProductNamesByAlbumId.Parameters["@id"].Value = entryId;

            SQLiteDataReader dataReader = vgmdbGetArbitraryProductNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetString(0);
            dataReader.Dispose();

            dataReader = vgmdbGetProductNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetString(0);
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbGetArbitraryArtistNamesByAlbumId, vgmdbGetArtistNamesByAlbumId;
        public IEnumerable<string> Vgmdb_FindArtistNamesByAlbumId(int entryId)
        {
            if (vgmdbGetArbitraryArtistNamesByAlbumId == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_artist_arbitrary");
                vgmdbGetArbitraryArtistNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArbitraryArtistNamesByAlbumId.CommandText = Resources.VgmdbGetArbitraryArtistsByAlbumId_Postgre;
                vgmdbGetArbitraryArtistNamesByAlbumId.Parameters.Add("@id", DbType.Int32);

                vgmdbGetArtistNamesByAlbumId = connection.CreateCommand();
                vgmdbGetArtistNamesByAlbumId.CommandText = Resources.VgmdbGetArtistNamesByAlbumId_Postgre;
                vgmdbGetArtistNamesByAlbumId.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbGetArbitraryArtistNamesByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbGetArtistNamesByAlbumId.Parameters["@id"].Value = entryId;

            SQLiteDataReader dataReader = vgmdbGetArbitraryArtistNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{0} ({1})", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();

            dataReader = vgmdbGetArtistNamesByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{0} ({1})", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbFindArtistsByName;
        public IEnumerable<int> Vgmdb_FindArtistIdsByName(string escaped)
        {
            if (vgmdbFindArtistsByName == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.artist");
                vgmdbFindArtistsByName = connection.CreateCommand();
                vgmdbFindArtistsByName.CommandText = "SELECT id FROM dump_vgmdb.artist WHERE name LIKE @name";
                vgmdbFindArtistsByName.Parameters.Add("@name", DbType.String);
            }

            vgmdbFindArtistsByName.Parameters["@name"].Value = escaped;
            SQLiteDataReader dataReader = vgmdbFindArtistsByName.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();

        }

        private SQLiteCommand vgmdbFindAlbumIdsByArtistId;
        public IEnumerable<int> Vgmdb_FindAlbumIdsByArtistId(int possibleArtist)
        {
            if (vgmdbFindAlbumIdsByArtistId == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_artists");
                vgmdbFindAlbumIdsByArtistId = connection.CreateCommand();
                vgmdbFindAlbumIdsByArtistId.CommandText = "SELECT albumid FROM dump_vgmdb.album_artists WHERE artistid=@id";
                vgmdbFindAlbumIdsByArtistId.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbFindAlbumIdsByArtistId.Parameters["@id"].Value = possibleArtist;
            SQLiteDataReader dataReader = vgmdbFindAlbumIdsByArtistId.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();

        }

        private SQLiteCommand vgmdbFindCoversByAlbumId;
        public IEnumerable<Image> FindCoversByAlbumId(int entryId)
        {
            if (vgmdbFindCoversByAlbumId == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_cover");
                vgmdbFindCoversByAlbumId = connection.CreateCommand();
                vgmdbFindCoversByAlbumId.CommandText = "SELECT scalerid FROM dump_vgmdb.album_cover WHERE albumid=@id";
                vgmdbFindCoversByAlbumId.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbFindCoversByAlbumId.Parameters["@id"].Value = entryId;
            SQLiteDataReader dataReader = vgmdbFindCoversByAlbumId.ExecuteReader();
            while (dataReader.Read())
            {
                int scalerId = dataReader.GetInt32(0);
                byte[] blob = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_vgmdb.album_cover"),azuStreamBlob.DeriveKey("buffer"),scalerId);
                if (blob != null)
                {
                    if (blob.Length > 0)
                    {
                        MemoryStream ms = new MemoryStream(blob, false);
                        Image image = Image.FromStream(ms);
                        ms.Dispose();
                        yield return image;
                    }
                }
            }
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbFindAlbumsBySkuPart;
        public IEnumerable<int> Vgmdb_FindAlbumsBySkuPart(string startswith)
        {
            if (vgmdbFindAlbumsBySkuPart == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.albums");
                vgmdbFindAlbumsBySkuPart = connection.CreateCommand();
                vgmdbFindAlbumsBySkuPart.CommandText = "SELECT id FROM dump_vgmdb.albums WHERE catalog LIKE @key";
                vgmdbFindAlbumsBySkuPart.Parameters.Add("@key", DbType.String);
            }

            vgmdbFindAlbumsBySkuPart.Parameters["@key"].Value = startswith;
            SQLiteDataReader dataReader = vgmdbFindAlbumsBySkuPart.ExecuteReader();
            while (dataReader.Read())
                yield return dataReader.GetInt32(0);
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbFindTrackDataByAlbum;
        public IEnumerable<Tuple<string, int, int, string, int>> Vgmdb_FindTrackDataByAlbum(int entryId)
        {
            if (vgmdbFindTrackDataByAlbum == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_disc_track_translation");
                vgmdbFindTrackDataByAlbum = connection.CreateCommand();
                vgmdbFindTrackDataByAlbum.CommandText = Resources.VgmdbGetTracksByAlbum_Postgre;
                vgmdbFindTrackDataByAlbum.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbFindTrackDataByAlbum.Parameters["@id"].Value = entryId;
            SQLiteDataReader dataReader = vgmdbFindTrackDataByAlbum.ExecuteReader();
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

        private SQLiteCommand vgmdbFindArbituraryLabelsByAlbumId, vgmdbFindLabelsByAlbumId;
        public IEnumerable<string> Vgmdb_FindLabelNamesByAlbumId(int entryId)
        {
            if (vgmdbFindArbituraryLabelsByAlbumId == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_label_arbiturary");
                vgmdbFindArbituraryLabelsByAlbumId = connection.CreateCommand();
                vgmdbFindArbituraryLabelsByAlbumId.CommandText = Resources.VgmdbFindArbituraryLabelNamesByAlbumId_Postgre;
                vgmdbFindArbituraryLabelsByAlbumId.Parameters.Add("@id", DbType.Int32);

                vgmdbFindLabelsByAlbumId = connection.CreateCommand();
                vgmdbFindLabelsByAlbumId.CommandText = Resources.VgmdbFindLabelsByAlbumId_Postgre;
                vgmdbFindLabelsByAlbumId.Parameters.Add("@id", DbType.Int32);
            }

            vgmdbFindArbituraryLabelsByAlbumId.Parameters["@id"].Value = entryId;
            vgmdbFindLabelsByAlbumId.Parameters["@id"].Value = entryId;

            SQLiteDataReader dataReader = vgmdbFindArbituraryLabelsByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{1}: {0}", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();

            dataReader = vgmdbFindLabelsByAlbumId.ExecuteReader();
            while (dataReader.Read())
                yield return String.Format("{1}: {0}", dataReader.GetString(0), dataReader.GetString(1));
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbFindRelatedAlbumsCommand;
        public IEnumerable<string> Vgmdb_FindRelatedAlbums(int albumId)
        {
            if (vgmdbFindRelatedAlbumsCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_relatedalbum");
                vgmdbFindRelatedAlbumsCommand = connection.CreateCommand();
                vgmdbFindRelatedAlbumsCommand.CommandText = Resources.VgmdbGetRelatedAlbums_Postgre;
                vgmdbFindRelatedAlbumsCommand.Parameters.Add("@albumid", DbType.Int32);
            }

            vgmdbFindRelatedAlbumsCommand.Parameters["@albumid"].Value = albumId;
            SQLiteDataReader dataReader = vgmdbFindRelatedAlbumsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string sku = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                yield return String.Format("{0}, {1}", sku, name);
            }
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbGetReleaseEvent;
        public string Vgmdb_GetReleaseEvent(int albumId)
        {
            if (vgmdbGetReleaseEvent == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_releaseevent");
                vgmdbGetReleaseEvent = connection.CreateCommand();
                vgmdbGetReleaseEvent.CommandText = Resources.VgmDbGetReleaseEvent;
                vgmdbGetReleaseEvent.Parameters.Add("@albumid", DbType.Int32);
            }

            vgmdbGetReleaseEvent.Parameters["@albumid"].Value = albumId;
            SQLiteDataReader dataReader = vgmdbGetReleaseEvent.ExecuteReader();
            string result = null;
            if (dataReader.Read())
                if (!dataReader.IsDBNull(0))
                    result = dataReader.GetString(0);
            dataReader.Dispose();
            return result;
        }

        private SQLiteCommand vgmdbFindReprintCommand;
        public IEnumerable<string> Vgmdb_FindReprints(int albumId)
        {
            if (vgmdbFindReprintCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_reprints");
                vgmdbFindReprintCommand = connection.CreateCommand();
                vgmdbFindReprintCommand.CommandText = Resources.VgmdbGetReprints_Postgre;
                vgmdbFindReprintCommand.Parameters.Add("@albumid", DbType.Int32);
            }

            vgmdbFindReprintCommand.Parameters["@albumid"].Value = albumId;
            SQLiteDataReader dataReader = vgmdbFindReprintCommand.ExecuteReader();
            while (dataReader.Read())
            {
                string sku = dataReader.GetString(0);
                string name = dataReader.GetString(1);
                yield return String.Format("{0}, {1}", sku, name);
            }
            dataReader.Dispose();
        }

        private SQLiteCommand vgmdbGetWebsitesCommand;
        public IEnumerable<Uri> Vgmdb_GetWebsites(int albumId)
        {
            if (vgmdbGetWebsitesCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vgmdb.album_websites");
                vgmdbGetWebsitesCommand = connection.CreateCommand();
                vgmdbGetWebsitesCommand.CommandText = "SELECT link FROM dump_vgmdb.album_websites WHERE albumid=@albumid";
                vgmdbGetWebsitesCommand.Parameters.Add("@albumid", DbType.Int32);
            }

            vgmdbGetWebsitesCommand.Parameters["@albumid"].Value = albumId;
            SQLiteDataReader dataReader = vgmdbGetWebsitesCommand.ExecuteReader();
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

        private SQLiteCommand psxdcSearchCommand;
        public IEnumerable<PsxDatacenterPreview> PsxDc_Search(string textBox)
        {
            if (psxdcSearchCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_psxdatacenter.games");
                psxdcSearchCommand = connection.CreateCommand();
                psxdcSearchCommand.CommandText =
                    "SELECT id, platform,sku,title,additionals FROM dump_psxdatacenter.games WHERE title LIKE @p1 OR commontitle LIKE @p1 OR sku LIKE @p2";
                psxdcSearchCommand.Parameters.Add("@p1", DbType.String);
                psxdcSearchCommand.Parameters.Add("@p2", DbType.String);
            }

            psxdcSearchCommand.Parameters["@p1"].Value = String.Format("%{0}%", textBox);
            psxdcSearchCommand.Parameters["@p2"].Value = String.Format("{0}%", textBox);
            SQLiteDataReader dataReader = psxdcSearchCommand.ExecuteReader();
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

        private SQLiteCommand psxdatacenterGetGameCommand;
        public PsxDatacenterGame PsxDc_GetSpecificGame(int previewId)
        {
            if (psxdatacenterGetGameCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_psxdatacenter.games");
                psxdatacenterGetGameCommand = connection.CreateCommand();
                psxdatacenterGetGameCommand.CommandText = Resources.PsxDatacenterGetGame_Postgre;
                psxdatacenterGetGameCommand.Parameters.Add("@id", DbType.Int32);
            }

            psxdatacenterGetGameCommand.Parameters["@id"].Value = previewId;
            SQLiteDataReader dataReader = psxdatacenterGetGameCommand.ExecuteReader();
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
            result.Cover = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_psxdatacenter.games"), azuStreamBlob.DeriveKey("cover"),previewId);
            result.Description = dataReader.GetString(11);
            result.Barcode = dataReader.GetString(12);
            dataReader.Dispose();
            return result;
        }

        private SQLiteCommand psxDatacenterGetScreenshots;
        public IEnumerable<byte[]> PsxDc_GetScreenshots(int previewId)
        {
            if (psxDatacenterGetScreenshots == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_psxdatacenter.game_screenshots");
                psxDatacenterGetScreenshots = connection.CreateCommand();
                psxDatacenterGetScreenshots.CommandText =
                    "SELECT screenshot.scalerid FROM dump_psxdatacenter.game_screenshots root JOIN dump_psxdatacenter.screenshots screenshot ON root.screenshotid = screenshot.id WHERE root.gameid=@id";
                psxDatacenterGetScreenshots.Parameters.Add("@id", DbType.Int32);
            }

            psxDatacenterGetScreenshots.Parameters["@id"].Value = previewId;
            SQLiteDataReader dataReader = psxDatacenterGetScreenshots.ExecuteReader();
            while (dataReader.Read())
            {
                int scalerId = dataReader.GetInt32(0);
                yield return azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_psxdatacenter.screenshots"),
                    azuStreamBlob.DeriveKey("buffer"), scalerId);
            }

            dataReader.Dispose();
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
                    //child.Header = dataReader.GetByteArray(7);
                    int key1 = azuStreamBlob.DeriveKey("azusa.filesysteminfo");
                    int key2 = azuStreamBlob.DeriveKey("head");
                    int key2b = (int)(child.Id << 32);
                    int key3 = (int) (child.Id & 0xffff0000);
                    child.Header = azuStreamBlob.Get(key1, key2 + key2b, key3);
                }

                child.ParentId = dataReader.GetInt64(8);
                yield return child;
            }
            dataReader.Close();
        }

        private SQLiteCommand vndbSearchCommand;
        public IEnumerable<VndbSearchResult> Vndb_Search(string searchquery)
        {
            if (vndbSearchCommand == null)
            {
                vndbSearchCommand = GetConnectionForTable("dump_vndb.release").CreateCommand();
                vndbSearchCommand.CommandText = Resources.VndbSearch_Postgre;
                vndbSearchCommand.Parameters.Add("@query", DbType.String);
            }

            vndbSearchCommand.Parameters["@query"].Value = searchquery;
            SQLiteDataReader reader = vndbSearchCommand.ExecuteReader();
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

        private SQLiteCommand getVnByReleaseCommand;
        public IEnumerable<VndbVnResult> Vndb_GetVnsByRelease(int searchResultRid)
        {
            if (getVnByReleaseCommand == null)
            {
                getVnByReleaseCommand = GetConnectionForTable("dump_vndb.release_vns").CreateCommand();
                getVnByReleaseCommand.CommandText = "SELECT * FROM dump_vndb.release_vns root WHERE rid=@id";
                getVnByReleaseCommand.Parameters.Add("@id", DbType.Int32);
            }

            getVnByReleaseCommand.Parameters["@id"].Value = searchResultRid;
            SQLiteDataReader dataReader = getVnByReleaseCommand.ExecuteReader();
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

        private SQLiteCommand vndbGetReleaseByIdCommand;
        private SQLiteCommand vndbGetReleaseLanguageById;
        private SQLiteCommand vndbGetReleaseMediaById;
        private SQLiteCommand vndbGetReleasePlatformById;
        private SQLiteCommand vndbGetReleaseProducersById;
        public VndbRelease Vndb_GetReleaseById(int releaseResultRid)
        {
            SQLiteConnection connection = GetConnectionForTable("dump_vndb.release");
            if (vndbGetReleaseByIdCommand == null)
            {
                vndbGetReleaseByIdCommand = connection.CreateCommand();
                vndbGetReleaseByIdCommand.CommandText = "SELECT * FROM dump_vndb.release WHERE id=@id";
                vndbGetReleaseByIdCommand.Parameters.Add("@id", DbType.Int32);
            }

            vndbGetReleaseByIdCommand.Parameters["@id"].Value = releaseResultRid;
            SQLiteDataReader dataReader = vndbGetReleaseByIdCommand.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Dispose();
                return null;
            }
            VndbRelease result = new VndbRelease();
            result.Title = dataReader.GetString(1);

            if (!dataReader.IsDBNull(2))
                result.OriginalTitle = dataReader.GetString(2);

            result.Released = dataReader.GetDateTime(3).ToShortDateString();
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
                vndbGetReleaseLanguageById.Parameters.Add("@id", DbType.Int32);
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
                vndbGetReleaseMediaById.Parameters.Add("@id", DbType.Int32);
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
                vndbGetReleasePlatformById.Parameters.Add("@id", DbType.Int32);
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
                vndbGetReleaseProducersById.Parameters.Add("@id", DbType.Int32);
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

        private SQLiteCommand vndbGetVn;
        private SQLiteCommand vndbGetVnAnime;
        private SQLiteCommand vndbGetVnPlatforms;
        private SQLiteCommand vndbGetVnRelations;
        private SQLiteCommand vndbGetVnScreens;
        private SQLiteCommand vndbGetVnLanguages;
        public VndbVn Vndb_GetVnById(int vnResultVnid)
        {
            SQLiteConnection connection = GetConnectionForTable("dump_vndb.vn");
            if (vndbGetVn == null)
            {
                vndbGetVn = connection.CreateCommand();
                vndbGetVn.CommandText = "SELECT * FROM dump_vndb.vn WHERE id=@id";
                vndbGetVn.Parameters.Add("@id", DbType.Int32);
            }

            vndbGetVn.Parameters["@id"].Value = vnResultVnid;
            SQLiteDataReader dataReader = vndbGetVn.ExecuteReader();
            dataReader.Read();
            VndbVn result = new VndbVn();
            result.Title = dataReader.GetString(3);

            if (!dataReader.IsDBNull(4))
                result.OriginalTitle = dataReader.GetString(4);

            if (!dataReader.IsDBNull(5))
                result.ReleaseDate = dataReader.GetDateTime(5).ToShortDateString();

            if (!dataReader.IsDBNull(6))
                result.Alias = dataReader.GetString(6);

            if (!dataReader.IsDBNull(7))
                result.Length = dataReader.GetInt32(7);

            if (!dataReader.IsDBNull(8))
                result.Description = dataReader.GetString(8);

            if (!dataReader.IsDBNull(9))
                result.WikipediaLink = dataReader.GetString(9);

            byte[] coverBuffer = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_vndb.vn"), azuStreamBlob.DeriveKey("image"), dataReader.GetInt32(0));
            if (coverBuffer != null && coverBuffer.Length > 0)
            {
                result.Image = (Bitmap)Image.FromStream(new MemoryStream(coverBuffer));
            }

            result.ImageNSFW = dataReader.GetBoolean(13);
            result.Popularity = dataReader.GetDouble(14);
            result.Rating = dataReader.GetDouble(15);
            dataReader.Close();

            if (vndbGetVnAnime == null)
            {
                vndbGetVnAnime = connection.CreateCommand();
                vndbGetVnAnime.CommandText = "SELECT * FROM dump_vndb.vn_anime WHERE vnid=@id";
                vndbGetVnAnime.Parameters.Add("@id", DbType.Int32);
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
                vndbGetVnPlatforms.Parameters.Add("@id", DbType.Int32);
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
                vndbGetVnRelations.Parameters.Add("@id", DbType.Int32);
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
                vndbGetVnScreens.CommandText = "SELECT scalerid FROM dump_vndb.vn_screens WHERE vnid=@id";
                vndbGetVnScreens.Parameters.Add("@id", DbType.Int32);
            }

            vndbGetVnScreens.Parameters["@id"].Value = vnResultVnid;
            dataReader = vndbGetVnScreens.ExecuteReader();
            result.Screens = new List<Image>();
            while (dataReader.Read())
                if (!dataReader.IsDBNull(0))
                {
                    byte[] buffer = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_vndb.vn_screens"), azuStreamBlob.DeriveKey("image"), dataReader.GetInt32(0));
                    if (buffer != null && buffer.Length > 0)
                        result.Screens.Add(Image.FromStream(new MemoryStream(buffer)));
                }
            dataReader.Close();

            if (vndbGetVnLanguages == null)
            {
                vndbGetVnLanguages = connection.CreateCommand();
                vndbGetVnLanguages.CommandText = "SELECT language FROM dump_vndb.vn_languages WHERE vnid=@id";
                vndbGetVnLanguages.Parameters.Add("@id", DbType.Int32);
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
            throw new NotImplementedException();
        }

        private SQLiteCommand myFigureCollectionSearchCommand;
        public IEnumerable<Figure> MyFigureCollection_Search(string query)
        {
            if (myFigureCollectionSearchCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_myfigurecollection.figures");
                myFigureCollectionSearchCommand = connection.CreateCommand();
                myFigureCollectionSearchCommand.CommandText = Resources.MyFigureCollectionSearch_Postgre;
                myFigureCollectionSearchCommand.Parameters.Add("@query", DbType.String);
            }
            query = "%" + query + "%";
            myFigureCollectionSearchCommand.Parameters["@query"].Value = query;
            SQLiteDataReader dataReader = myFigureCollectionSearchCommand.ExecuteReader();
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

                child.Thumbnail = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_myfigurecollection.figurephotos"), azuStreamBlob.DeriveKey("thumbnail"), child.ID);
                yield return child;
            }
            dataReader.Dispose();
        }

        public Image MyFigureCollection_GetPhoto(int wrappedFigureId)
        {
            byte[] buffer = azuStreamBlob.Get(
                azuStreamBlob.DeriveKey("dump_myfigurecollection.figurephotos"),
                azuStreamBlob.DeriveKey("image"), 
                wrappedFigureId);
            return Image.FromStream(new MemoryStream(buffer));
        }

        private SQLiteCommand vocadbSearchCommand;
        public IEnumerable<VocadbSearchResult> VocaDb_Search(string text)
        {
            text = "%" + text + "%";
            if (vocadbSearchCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vocadb.albums");
                vocadbSearchCommand = connection.CreateCommand();
                vocadbSearchCommand.CommandText = Resources.Vocadb_Search_Postgre;
                vocadbSearchCommand.Parameters.Add("@query", DbType.String);
            }
            vocadbSearchCommand.Parameters["@query"].Value = text;
            SQLiteDataReader dataReader = vocadbSearchCommand.ExecuteReader();
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

        public Image Vocadb_GetAlbumCover(int id)
        {
            byte[] buffer = azuStreamBlob.Get(azuStreamBlob.DeriveKey("dump_vocadb.albums"), azuStreamBlob.DeriveKey("cover"), id);
            if (buffer == null || buffer.Length == 0)
                return null;
            Image image = Image.FromStream(new MemoryStream(buffer));
            return image;
        }

        private SQLiteCommand vocadbFindAlbumNamesBySongNamesCommand;
        public List<string> VocaDb_FindAlbumNamesBySongNames(string text)
        {
            if (vocadbFindAlbumNamesBySongNamesCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vocadb.albumtracks");
                vocadbFindAlbumNamesBySongNamesCommand = connection.CreateCommand();
                vocadbFindAlbumNamesBySongNamesCommand.CommandText = Resources.Vocadb_FindSongRelatedAlbum;
                vocadbFindAlbumNamesBySongNamesCommand.Parameters.Add("@query", DbType.String);
            }
            vocadbFindAlbumNamesBySongNamesCommand.Parameters["@query"].Value = "%" + text + "%";
            var dataReader = vocadbFindAlbumNamesBySongNamesCommand.ExecuteReader();
            List<string> result = new List<string>();
            while (dataReader.Read())
                result.Add(dataReader.GetString(0));
            dataReader.Dispose();
            return result;
        }

        private SQLiteCommand vocadbGetTracksByAlbumCommand;
        public IEnumerable<VocadbTrackEntry> VocaDb_GetTracksByAlbum(int wrappedId)
        {
            if (vocadbGetTracksByAlbumCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_vocadb.albumtracks");
                vocadbGetTracksByAlbumCommand = connection.CreateCommand();
                vocadbGetTracksByAlbumCommand.CommandText =
                    "SELECT * FROM dump_vocadb.albumtracks WHERE albumid = @id ORDER BY albumid ASC, discnumber ASC, tracknumber ASC";
                vocadbGetTracksByAlbumCommand.Parameters.Add("@id", DbType.Int32);
            }

            vocadbGetTracksByAlbumCommand.Parameters["@id"].Value = wrappedId;
            SQLiteDataReader dataReader = vocadbGetTracksByAlbumCommand.ExecuteReader();
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

        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;
        private const string fastbooru = "fastbooru";
        private SQLiteCommand getAllTagsCommand;
        public IEnumerable<GelbooruTag> Gelbooru_GetAllTags()
        {
            long day = DateTime.Now.Ticks / TicksPerDay;
            int day32 = (int) day;
            if (azuStreamBlob.TestFor(azuStreamBlob.DeriveKey(fastbooru), azuStreamBlob.DeriveKey(fastbooru), day32))
            {
                byte[] cacheBuffer = azuStreamBlob.Get(azuStreamBlob.DeriveKey(fastbooru), azuStreamBlob.DeriveKey(fastbooru), day32);
                BinaryReader cacheBr = new BinaryReader(new MemoryStream(cacheBuffer));
                while (cacheBr.ReadByte() == 1)
                {
                    GelbooruTag child = new GelbooruTag();
                    child.Tag = cacheBr.ReadString();
                    child.NumberOfImages = cacheBr.ReadInt32();
                    child.Id = cacheBr.ReadInt32();
                    yield return child;
                }
                cacheBr.Dispose();
                yield break;
            }
            
            if (getAllTagsCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_gb.posttags");
                getAllTagsCommand = connection.CreateCommand();
                getAllTagsCommand.CommandText = Resources.Gelbooru_GetTags_Postgre;
            }

            BinaryWriter cacheGen = new BinaryWriter(azuStreamBlob.CanWrite ? new MemoryStream() : Stream.Null);
            SQLiteDataReader dataReader = getAllTagsCommand.ExecuteReader();
            while (dataReader.Read())
            {
                int numImages = dataReader.GetInt32(1);
                if (numImages > 0)
                {
                    GelbooruTag child = new GelbooruTag();
                    child.Tag = dataReader.GetString(0);
                    if (child.Tag.Length == 0)
                        continue;
                    if (char.IsLetter(child.Tag[0]))
                    {
                        child.NumberOfImages = numImages;
                        child.Id = dataReader.GetInt32(2);
                        cacheGen.Write((byte) 1);
                        cacheGen.Write(child.Tag);
                        cacheGen.Write(child.NumberOfImages);
                        cacheGen.Write(child.Id);
                        yield return child;
                    }
                }
            }
            dataReader.Dispose();
            cacheGen.Write((byte) 2);
            if (azuStreamBlob.CanWrite)
            {
                MemoryStream ms = (MemoryStream) cacheGen.BaseStream;
                azuStreamBlob.Put(azuStreamBlob.DeriveKey(fastbooru), azuStreamBlob.DeriveKey(fastbooru), day32, ms.ToArray());
            }
            cacheGen.Dispose();
        }

        private SQLiteCommand gelbooruGetPostsByTagCommand;
        public IEnumerable<int> Gelbooru_GetPostsByTag(int tagId)
        {
            if (gelbooruGetPostsByTagCommand == null)
            {
                SQLiteConnection connection = GetConnectionForTable("dump_gb.posttags");
                gelbooruGetPostsByTagCommand = connection.CreateCommand();
                gelbooruGetPostsByTagCommand.CommandText =
                    "SELECT DISTINCT postid FROM dump_gb.posttags WHERE tagid = @tagid";
                gelbooruGetPostsByTagCommand.Parameters.Add("@tagid", DbType.Int32);
            }

            gelbooruGetPostsByTagCommand.Parameters["@tagid"].Value = tagId;
            SQLiteDataReader dataReader = gelbooruGetPostsByTagCommand.ExecuteReader();
            while (dataReader.Read())
            {
                yield return dataReader.GetInt32(0);
            }
            dataReader.Dispose();
        }

        public DexTimelineEntry Dexcom_GetLatestGlucoseEntry()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DexTimelineEntry> Dexcom_GetGlucoseEntriesAfter(DateTime scope)
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

        public bool IsAllowedSyncSource()
        {
            return false;
        }

        public bool IsAllowedSyncTarget()
        {
            return true;
        }

        public object GetConnectionObject()
        {
            return connections;
        }

        public string GetConnectionString()
        {
            return null;
        }

        public void InsertDiscArchivatorDisc(long discid, string path, string name)
        {
            throw new NotImplementedException();
        }

        public DiscStatus GetDiscArchivatorDisc(long discid)
        {
            throw new NotImplementedException();
        }

        public void SetDiscArchivatorProperty(long discid, DiscStatusProperty property, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetDiscArchivatorAzusaLink(long discid, int mediumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscStatus> GetDiscArchivatorEntries()
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

        public LicenseState CheckLicenseStatus()
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

        public int GetFirstMediaTypeId()
        {
            throw new NotImplementedException();
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

            if (!ndr.IsDBNull(10))
                m.CueSheetContent = ndr.GetString(10);

            if (!ndr.IsDBNull(11))
                m.ChecksumContent = ndr.GetString(11);

            if (!ndr.IsDBNull(12))
                m.PlaylistContent = ndr.GetString(12);

            m.CdTextContent = azuStreamBlob.Get(azuStreamBlob.DeriveKey("azusa.media"), azuStreamBlob.DeriveKey("cdtext"), m.Id);

            if (!ndr.IsDBNull(14))
                m.LogfileContent = ndr.GetString(14);

            m.MdsContent = azuStreamBlob.Get(azuStreamBlob.DeriveKey("azusa.media"), azuStreamBlob.DeriveKey("mediadescriptorsidecar"), m.Id);
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
                mt.IgnoreForStatistics = dataReader.GetBoolean(6);
                mt.Icon = azuStreamBlob.Get(azuStreamBlob.DeriveKey("azusa.mediatypes"), azuStreamBlob.DeriveKey("icon"), mt.Id);
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
                result.Picture = azuStreamBlob.Get(azuStreamBlob.DeriveKey("azusa.products"), azuStreamBlob.DeriveKey("picture"), result.Id);

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

                result.Screenshot = azuStreamBlob.Get(azuStreamBlob.DeriveKey("azusa.products"), azuStreamBlob.DeriveKey("screenshot"), result.Id);

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

                product.ScreenshotSize = azuStreamBlob.GetSize(azuStreamBlob.DeriveKey("azusa.products"), azuStreamBlob.DeriveKey("screenshot"), product.Id);
                product.CoverSize = azuStreamBlob.GetSize(azuStreamBlob.DeriveKey("azusa.products"), azuStreamBlob.DeriveKey("picture"), product.Id);

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

        public int MailArchive_CountItemsInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public long MailArchive_GetHighestMessageUTimeInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public int MailArchive_GetLatestMessageId()
        {
            throw new NotImplementedException();
        }

        public Mail MailArchive_GetSpecificMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public void MailArchive_InsertFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public void MailArchive_StoreMessage(Mail mail)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForFolder(long folderId)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_ErasePhoto(string personId)
        {
            throw new NotImplementedException();
        }

        public byte[] SedgeTree_GetDataByVersion(int version)
        {
            throw new NotImplementedException();
        }

        public int? SedgeTree_GetLatestVersion()
        {
            SQLiteConnection connection = GetConnectionForTable("sedgetree.versioning");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT MAX(id) FROM sedgetree.versioning";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
            {
                result = dataReader.GetInt32(0);
            }
            dataReader.Dispose();
            dataReader.Close();
            return result;
        }

        public byte[] SedgeTree_GetPhotoByPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_InsertPhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_InsertVersion(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public bool SedgeTree_TestForPhoto(string toString)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_UpdatePhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
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

        public void WarWalking_AddAccessPoint(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tour> WarWalking_GetAllTours()
        {
            SQLiteConnection connection = GetConnectionForTable("warwalking.tours");
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM warwalking.tours";
            SQLiteDataReader dataReader = cmd.ExecuteReader();
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

        public Discovery WarWalking_GetByBssid(string bssid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Discovery> WarWalking_GetDiscoveriesByTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public int WarWalking_InsertTourAndReturnId(long hash, int recordStart, string name)
        {
            throw new NotImplementedException();
        }

        public bool WarWalking_IsAccessPointKnown(string bssid)
        {
            throw new NotImplementedException();
        }

        public bool WarWalking_IsTourKnown(long hash)
        {
            throw new NotImplementedException();
        }

        public void WarWalking_UpdateDiscovery(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public AzusaStreamBlob GetStreamBlob()
        {
            return azuStreamBlob;
        }
    }
}
