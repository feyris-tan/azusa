using System.Collections.Generic;
using AzusaERP.OldStuff;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Migrations
{
    static class Migration1to2
    {
        public static void Migrate()
        {
            AzusaContext context = AzusaContext.GetInstance();
            IDatabaseDriver contextDatabaseDriver = context.DatabaseDriver;
            List<string> allTableNames = contextDatabaseDriver.GetAllPublicTableNames();

            foreach (string oldTableName in allTableNames)
            {
                int colonIndex = oldTableName.IndexOf('_');
                string schemaName = oldTableName.Substring(0, colonIndex);
                string newTableName = oldTableName.Substring(colonIndex + 1);
                if (schemaName.Equals("dump"))
                {
                    colonIndex = newTableName.IndexOf('_');
                    schemaName = schemaName + "_" + newTableName.Substring(0, colonIndex);
                    newTableName = newTableName.Substring(colonIndex + 1);
                }
                contextDatabaseDriver.CreateSchema(schemaName);
                contextDatabaseDriver.MoveAndRenameTable("public", oldTableName, schemaName, newTableName);
            }
        }
    }
}
