using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libazustreamblob
{
    public class AzusaStreamBlob : IDisposable
    {
        public AzusaStreamBlob(DirectoryInfo di)
        {
            rootDirectoryInfo = di;
            FileInfo dbFile = GetFileInfo("azustreamblob.db");
            SQLiteConnectionStringBuilder sqlcsb = new SQLiteConnectionStringBuilder();
            sqlcsb.DataSource = dbFile.FullName;
            if (!dbFile.Exists)
            {
                FileInfo testWriteFile = GetFileInfo("test_write1.dvr");
                bool canCreateDb = TestWrite(testWriteFile);
                if (!canCreateDb)
                {
                    CanWrite = false;
                    return;
                }

                CanWrite = true;
                connection = new SQLiteConnection(sqlcsb.ToString());
                connection.Open();
                SetupTables();
            }
            else
            {
                CanWrite = !dbFile.Attributes.HasFlag(FileAttributes.ReadOnly);
                sqlcsb.ReadOnly = !CanWrite;
                connection = new SQLiteConnection(sqlcsb.ToString());
                connection.Open();
            }
            InsertMount();
            currentSegmentId = -11;
            dedupTuples = new Dictionary<string, Tuple<int, long, int>>();
        }

        public AzusaStreamBlob(DirectoryInfo di, bool obfuscate)
        : this(di)
        {
            this.Obfuscation = obfuscate;
        }

        private bool TestWrite(FileInfo fi)
        {
            try
            {
                byte[] buffer = new byte[1024];
                for (int i = 0; i < buffer.Length; i++)
                    buffer[i] = (byte)((i + 0x20) % Byte.MaxValue);

                FileStream outFileStream = fi.OpenWrite();
                for (int i = 0; i < 1000; i++)
                    outFileStream.Write(buffer, 0, buffer.Length);
                outFileStream.Flush(true);
                outFileStream.Close();
                outFileStream.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private SQLiteCommand updateCommand;
        private SQLiteCommand testCommand;
        private SQLiteCommand fetchCommand;
        private const string SEGMENT_NAME_SCHEMA = "azublob{0:0000}.bin";
        private SQLiteCommand createSegment;
        private const int MAX_SEGMENT_SIZE = 680000000;
        private SQLiteCommand selectAllSegments;
        private int currentSegmentId;
        private FileStream currentSegmentStream;
        private SQLiteCommand putCommand;
        private SQLiteConnection connection;
        private DirectoryInfo rootDirectoryInfo;
        private Dictionary<string, Tuple<int, long, int>> dedupTuples;
        public bool CanWrite { get; private set; }
        public bool Obfuscation { get; }
        private SQLiteTransaction transaction;
        private int writtenSinceTransaction;

        private FileInfo GetFileInfo(string fname)
        {
            return new FileInfo(Path.Combine(rootDirectoryInfo.FullName, fname));
        }

        private void SetupTables()
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "create table entries ( key1 int, key2 int, key3 int, segment int, offset int,\r\n\tlength int,\r\n\tdateAdded int default CURRENT_TIMESTAMP,\r\n\tconstraint table_name_pk primary key (key1, key2, key3)\r\n)";
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = connection.CreateCommand();
            cmd.CommandText = "create table segments (id INTEGER constraint segments_pk primary key autoincrement,\r\n    dateAdded int default CURRENT_TIMESTAMP,\r\n    uuid      VARCHAR(37) not null\r\n);\r\n";
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = connection.CreateCommand();
            cmd.CommandText = "create table mounts (id INTEGER not null constraint mounts_pk primary key autoincrement,\r\n\tdateAdded int default CURRENT_TIMESTAMP not null,\r\n\tmachineName VARCHAR(64) not null,\r\n\tusername VARCHAR(64) not null,\r\n\tos VARCHAR(128),\r\n\tuuid VARCHAR(36)\r\n)";
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            cmd = connection.CreateCommand();
            cmd.CommandText = "create table keys (id INTEGER not null constraint key_ok primary key autoincrement, key varchar(128), dateAdded int default CURRENT_TIMESTAMP)";
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        private void InsertMount()
        {
            SQLiteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO mounts (machineName, username, os, uuid) VALUES (@machineName,@username,@os,@uuid)";
            cmd.Parameters.Add(new SQLiteParameter("@machineName", DbType.String));
            cmd.Parameters.Add(new SQLiteParameter("@username", DbType.String));
            cmd.Parameters.Add(new SQLiteParameter("@os", DbType.String));
            cmd.Parameters.Add(new SQLiteParameter("@uuid", DbType.String));
            cmd.Parameters["@machineName"].Value = Environment.MachineName;
            cmd.Parameters["@username"].Value = Environment.UserName;
            cmd.Parameters["@os"].Value = Environment.OSVersion.VersionString;
            cmd.Parameters["@uuid"].Value = Guid.NewGuid().ToString();
            cmd.ExecuteNonQuery();
        }

        private void OpenSegment(int segmentId)
        {
            if (currentSegmentId == segmentId)
                return;

            if (currentSegmentStream != null)
            {
                currentSegmentStream.Close();
                currentSegmentStream.Dispose();
            }

            FileInfo fileInfo = GetFileInfo(String.Format(SEGMENT_NAME_SCHEMA, segmentId));
            FileStream result;
            if (!fileInfo.Exists)
            {
                if (!CanWrite)
                    throw new IOException("can not create new segment in read only mode");

                result = fileInfo.Open(FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                byte[] emptyBuffer = new byte[4096];
                result.Write(emptyBuffer, 0, emptyBuffer.Length);
                result.Flush(true);
            }
            else
            {
                FileAccess access = CanWrite ? FileAccess.ReadWrite : FileAccess.Read;
                result = fileInfo.Open(FileMode.Open, access, FileShare.Read);
            }

            currentSegmentId = segmentId;
            currentSegmentStream = result;
        }

        public void Put(int key1, int key2, int key3, byte[] value)
        {
            if (!CanWrite)
                throw new InvalidOperationException();
            
            if (value == null)
                value = new byte[0];

            value = (byte[])value.Clone();
            if (Obfuscation)
            {
                uint[] seeds = new[] {(uint) key1, (uint) key2, (uint) key3};
                PerformSymmetricObfuscation(value, seeds);
            }

            if (TestFor(key1, key2, key3))
            {
                Update(key1, key2, key3, value);
                return;
            }

            if (putCommand == null)
            {
                putCommand = connection.CreateCommand();
                putCommand.CommandText = "INSERT INTO entries (key1, key2, key3, segment, offset, length) VALUES (@key1,@key2,@key3,@segment,@offset,@length)";
                putCommand.Parameters.Add("@key1", DbType.Int32);
                putCommand.Parameters.Add("@key2", DbType.Int32);
                putCommand.Parameters.Add("@key3", DbType.Int32);
                putCommand.Parameters.Add("@segment", DbType.Int32);
                putCommand.Parameters.Add("@offset", DbType.Int64);
                putCommand.Parameters.Add("@length", DbType.Int32);
            }

            string itemHash = Hashdeep.GetFingerprint(value);

            if (!dedupTuples.ContainsKey(itemHash))
            {
                int usableSegmentId = FindUsableSegment(value.Length + 16);
                OpenSegment(usableSegmentId);
                byte[] header = new byte[16];
                byte[] tmp = BitConverter.GetBytes(key1);
                Array.Copy(tmp, 0, header, 0, 4);
                tmp = BitConverter.GetBytes(key2);
                Array.Copy(tmp, 0, header, 4, 4);
                tmp = BitConverter.GetBytes(key3);
                Array.Copy(tmp, 0, header, 8, 4);
                tmp = BitConverter.GetBytes(value.Length);
                Array.Copy(tmp, 0, header, 12, 4);
                currentSegmentStream.Position = currentSegmentStream.Length;
                currentSegmentStream.Write(header, 0, header.Length);

                int valueSegment = usableSegmentId;
                long valueOffset = currentSegmentStream.Length;
                int valueLength = value.Length;
                putCommand.Parameters["@key1"].Value = key1;
                putCommand.Parameters["@key2"].Value = key2;
                putCommand.Parameters["@key3"].Value = key3;
                putCommand.Parameters["@segment"].Value = valueSegment;
                putCommand.Parameters["@offset"].Value = valueOffset;
                putCommand.Parameters["@length"].Value = valueLength;


                currentSegmentStream.Position = currentSegmentStream.Length;
                currentSegmentStream.Write(value, 0, value.Length);
                currentSegmentStream.Flush(true);
                dedupTuples.Add(itemHash, new Tuple<int, long, int>(valueSegment, valueOffset, valueLength));
            }
            else
            {
                Tuple<int,long,int> dedup = dedupTuples[itemHash];
                putCommand.Parameters["@key1"].Value = key1;
                putCommand.Parameters["@key2"].Value = key2;
                putCommand.Parameters["@key3"].Value = key3;
                putCommand.Parameters["@segment"].Value = dedup.Item1;
                putCommand.Parameters["@offset"].Value = dedup.Item2;
                putCommand.Parameters["@length"].Value = dedup.Item3;
                if (dedup.Item3 > 0)
                {
                    Console.WriteLine("Deduplicated {0},{1},{2} (segment={3},offset={4},size={5})", key1, key2, key3,
                        dedup.Item1, dedup.Item2, dedup.Item3);
                }
            }

            if (transaction == null)
            {
                transaction = connection.BeginTransaction();
                writtenSinceTransaction = 0;
            }

            putCommand.ExecuteNonQuery();
            writtenSinceTransaction += value.Length;
            if (writtenSinceTransaction > 10000000)
            {
                Flush();
            }
        }

        public void Flush()
        {
            currentSegmentStream.Flush();
            if (transaction != null)
            {
                transaction.Commit();
                transaction.Dispose();
                transaction = null;
            }
        }

        private void Update(int key1, int key2, int key3, byte[] value)
        {
            if (updateCommand == null)
            {
                updateCommand = connection.CreateCommand();
                updateCommand.CommandText = "UPDATE entries SET segment=@segment, offset=@offset, length=@length WHERE key1=@key1 AND key2=@key2 AND key3=@key3";
                updateCommand.Parameters.Add("@key1", DbType.Int32);
                updateCommand.Parameters.Add("@key2", DbType.Int32);
                updateCommand.Parameters.Add("@key3", DbType.Int32);
                updateCommand.Parameters.Add("@segment", DbType.Int32);
                updateCommand.Parameters.Add("@offset", DbType.Int64);
                updateCommand.Parameters.Add("@length", DbType.Int32);
            }

            int usableSegmentId = FindUsableSegment(value.Length);
            OpenSegment(usableSegmentId);

            updateCommand.Parameters["@key1"].Value = key1;
            updateCommand.Parameters["@key2"].Value = key2;
            updateCommand.Parameters["@key3"].Value = key3;
            updateCommand.Parameters["@segment"].Value = usableSegmentId;
            updateCommand.Parameters["@offset"].Value = currentSegmentStream.Length;
            updateCommand.Parameters["@length"].Value = value.Length;


            currentSegmentStream.Position = currentSegmentStream.Length;
            currentSegmentStream.Write(value, 0, value.Length);
            currentSegmentStream.Flush(true);

            updateCommand.ExecuteNonQuery();
        }
        
        public bool TestFor(int key1, int key2, int key3)
        {
            if (testCommand == null)
            {
                testCommand = connection.CreateCommand();
                testCommand.CommandText = "SELECT segment FROM entries WHERE key1=@a AND key2=@b AND key3=@c";
                testCommand.Parameters.Add("@a", DbType.Int32);
                testCommand.Parameters.Add("@b", DbType.Int32);
                testCommand.Parameters.Add("@c", DbType.Int32);
            }

            testCommand.Parameters["@a"].Value = key1;
            testCommand.Parameters["@b"].Value = key2;
            testCommand.Parameters["@c"].Value = key3;
            SQLiteDataReader dataReader = testCommand.ExecuteReader();
            bool result = dataReader.Read();
            dataReader.Close();
            dataReader.Dispose();
            return result;
        }

        public byte[] Get(int key1, int key2, int key3)
        {
            if (fetchCommand == null)
            {
                fetchCommand = connection.CreateCommand();
                fetchCommand.CommandText = "SELECT segment, offset, length FROM entries WHERE key1=@a AND key2=@b AND key3=@c";
                fetchCommand.Parameters.Add("@a", DbType.Int32);
                fetchCommand.Parameters.Add("@b", DbType.Int32);
                fetchCommand.Parameters.Add("@c", DbType.Int32);
            }

            fetchCommand.Parameters["@a"].Value = key1;
            fetchCommand.Parameters["@b"].Value = key2;
            fetchCommand.Parameters["@c"].Value = key3;
            SQLiteDataReader dataReader = fetchCommand.ExecuteReader();
            if (!dataReader.Read())
                throw new FileNotFoundException();

            int segment = dataReader.GetInt32(0);
            long offset = dataReader.GetInt64(1);
            int length = dataReader.GetInt32(2);
            dataReader.Dispose();

            OpenSegment(segment);
            currentSegmentStream.Position = offset;
            byte[] result = new byte[length];
            int readResult = currentSegmentStream.Read(result, 0, length);
            if (readResult < length)
                throw new IOException("incomplete read");

            if (Obfuscation)
            {
                uint[] seeds = new[] { (uint)key1, (uint)key2, (uint)key3 };
                PerformSymmetricObfuscation(result, seeds);
            }
            return result;
        }

        public int GetSize(int key1, int key2, int key3)
        {
            if (fetchCommand == null)
            {
                fetchCommand = connection.CreateCommand();
                fetchCommand.CommandText = "SELECT segment, offset, length FROM entries WHERE key1=@a AND key2=@b AND key3=@c";
                fetchCommand.Parameters.Add("@a", DbType.Int32);
                fetchCommand.Parameters.Add("@b", DbType.Int32);
                fetchCommand.Parameters.Add("@c", DbType.Int32);
            }

            fetchCommand.Parameters["@a"].Value = key1;
            fetchCommand.Parameters["@b"].Value = key2;
            fetchCommand.Parameters["@c"].Value = key3;
            SQLiteDataReader dataReader = fetchCommand.ExecuteReader();
            if (!dataReader.Read())
            {
                dataReader.Dispose();
                return 0;
            }

            int length = dataReader.GetInt32(2);
            dataReader.Dispose();
            return length;
        }

        private int FindUsableSegment(int size)
        {
            if (selectAllSegments == null)
            {
                selectAllSegments = connection.CreateCommand();
                selectAllSegments.CommandText = "SELECT id FROM segments";
            }

            SQLiteDataReader dataReader = selectAllSegments.ExecuteReader();
            while (dataReader.Read())
            {
                int id = dataReader.GetInt32(0);
                FileInfo segmentFileInfo = GetFileInfo(String.Format(SEGMENT_NAME_SCHEMA, id));
                long currentSegmentSize = segmentFileInfo.Length;
                long segmentSizeWhenDone = currentSegmentSize + size;
                if (segmentSizeWhenDone < MAX_SEGMENT_SIZE)
                {
                    dataReader.Dispose();
                    return id;
                }
            }
            dataReader.Dispose();

            if (createSegment == null)
            {
                createSegment = connection.CreateCommand();
                createSegment.CommandText = "INSERT INTO segments (uuid) VALUES (@uuid)";
                createSegment.Parameters.Add("@uuid", DbType.String);
            }

            createSegment.Parameters["@uuid"].Value = Guid.NewGuid();
            createSegment.ExecuteNonQuery();
            return (int) connection.LastInsertRowId;
        }

        public void Dispose()
        {
            Flush();
            fetchCommand?.Dispose();
            createSegment?.Dispose();
            selectAllSegments?.Dispose();
            currentSegmentStream?.Dispose();
            putCommand?.Dispose();
            connection?.Dispose();
        }

        private void PerformSymmetricObfuscation(byte[] data, uint[] seedUints)
        {
            Random random = MersenneTwister.MTRandom.Create(seedUints);
            byte[] mask = new byte[1024];
            random.NextBytes(mask);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ mask[i % mask.Length]);
            }
        }

        private SQLiteCommand addKeyCmd;
        private SQLiteCommand deriveKeyCmd;
        public int DeriveKey(string key)
        {
            if (deriveKeyCmd == null)
            {
                deriveKeyCmd = connection.CreateCommand();
                deriveKeyCmd.CommandText = "SELECT id FROM keys WHERE key=@key";
                deriveKeyCmd.Parameters.Add("@key", DbType.String);
            }

            deriveKeyCmd.Parameters["@key"].Value = key;
            SQLiteDataReader dataReader = deriveKeyCmd.ExecuteReader();
            int? result = null;
            if (dataReader.Read())
                result = dataReader.GetInt32(0);
            dataReader.Close();
            if (result.HasValue)
                return result.Value;
            else
            {
                if (addKeyCmd == null)
                {
                    addKeyCmd = connection.CreateCommand();
                    addKeyCmd.CommandText = "INSERT INTO keys (key) VALUES (@key)";
                    addKeyCmd.Parameters.Add("@key", DbType.String);
                }

                addKeyCmd.Parameters["@key"].Value = key;
                addKeyCmd.ExecuteNonQuery();
                return DeriveKey(key);
            }
        }
    }
}
