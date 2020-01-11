using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    class Sync
    {
        public Sync(IDatabaseDriver source, IDatabaseDriver target)
        {
            this.source = source;
            this.target = target;
        }
        private IDatabaseDriver source, target;
        public event SyncLogMessageCallback Message;

        public void Execute()
        {
            List<string> tableNames = source.GetAllTableNames();
            tableNames = tableNames.OrderBy(x => x).ToList();

            foreach (string tableName in tableNames)
            {
                List<DatabaseColumn> columns = source.Sync_DefineTable(tableName);
                if (!columns.Any(x => x.ColumnName.ToLowerInvariant().Equals("dateadded")))
                {
                    Console.WriteLine("DateAdded column is missing in {0}!", tableName);
                    continue;
                }

                foreach (DatabaseColumn column in columns)
                {
                    if (IsNumericString(column.ColumnName))
                    {
                        column.ColumnName = "\"" + column.ColumnName + "\"";
                    }
                }
                AutoGenerateParameterNames(columns);
                if (!target.Sync_DoesTableExist(tableName))
                {
                    target.Sync_CreateTable(tableName, columns);
                }

                //Delta
                Message(String.Format("Berechne \u0394 für {0}", tableName));
                DateTime? latestInsert = target.Sync_GetLastSyncDateForTable(tableName);
                bool partialUpdatesPossible = columns.Any(x => x.ColumnName.ToLowerInvariant().Equals("dateupdated"));
                DateTime? latestUpdate = null;

                if (partialUpdatesPossible)
                {
                    //Delta 2
                    Message(String.Format("Berechne \u03942 für {0}", tableName));
                    latestUpdate = target.Sync_GetLatestUpdateForTable(tableName);
                }

                //Delta
                Message(String.Format("Lese \u0394 für {0}", tableName));
                DbDataReader syncReader = source.Sync_GetSyncReader(tableName, latestInsert);
                target.Sync_CopyFrom(tableName, columns, syncReader, Message);
                if (partialUpdatesPossible)
                {
                    //Delta 2
                    Message(String.Format("Lese \u03942 für {0}", tableName));
                    syncReader = source.Sync_GetUpdateSyncReader(tableName, latestUpdate);
                    Queue<object> leftovers = new Queue<object>();
                    target.Sync_CopyUpdatesFrom(tableName, columns, syncReader, Message, leftovers);

                    if (leftovers.Count > 0)
                    {
                        DatabaseColumn idColumn = GetIdColumn(columns);
                        while (leftovers.Count > 0)
                        {
                            Message(String.Format("\u03943 für {0} wird verarbeitet...",tableName));
                            DbDataReader reader = source.Sync_ArbitrarySelect(tableName, idColumn, leftovers.Dequeue());
                            target.Sync_CopyFrom(tableName, columns, reader, Message);
                        }
                    }
                }
            }
            IStreamBlobOwner streamBlobOwner = target as IStreamBlobOwner;
            if (streamBlobOwner != null)
            {
                streamBlobOwner.GetStreamBlob().Flush();
            }

            List<SqlIndex> sourceIndexes = source.GetSqlIndexes().ToList();
            List<SqlIndex> targetIndexes = target.GetSqlIndexes().ToList();
            foreach (SqlIndex index in sourceIndexes)
            {
                if (targetIndexes.Contains(index))
                    continue;
                Message(String.Format("Create Index: {0}", index.IndexName));
                target.CreateIndex(index);
            }
        }

        private bool ContainsColumn(IEnumerable<DatabaseColumn> columns, string colName)
        {
            colName = colName.ToLowerInvariant();
            foreach (DatabaseColumn column in columns)
            {
                string cmp = column.ColumnName.ToLowerInvariant();
                if (cmp.Equals(colName))
                    return true;
            }

            return false;
        }

        private void AutoGenerateParameterNames(List<DatabaseColumn> columns)
        {
            for (int i = 0; i < columns.Count; i++)
            {
                columns[i].ParameterName = "@" + randomString(i + 1);
            }
        }

        private string randomString(int len)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var stringChars = new char[len];
            if (rng == null)
                rng = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rng.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        private Random rng;
        
        private bool IsNumericString(string s)
        {
            foreach (char chara in s)
            {
                if (!Char.IsDigit(chara))
                {
                    return false;
                }
            }

            return true;
        }

        internal static DatabaseColumn GetIdColumn(List<DatabaseColumn> columns)
        {
            DatabaseColumn idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("id"));
            if (idColumn != null)
                return idColumn;

            idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("unfiltered"));   //Workaround für Dexcom. Das Update Feld ist hier aber eh immer NULL
            if (idColumn != null)
                return idColumn;

            idColumn = columns.Find(x => x.ColumnName.ToLowerInvariant().Equals("uid"));
            if (idColumn != null)
                return idColumn;

            throw new NotImplementedException(String.Format("No ID column detected!"));
        }
    }

    public delegate void SyncLogMessageCallback(string message);
}
