using MailKit.Search;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;

namespace moe.yo3explorer.azusa.Control.MailArchive.Boundary
{
    class FolderService
    {
        public static bool CreateIfNotExists(Folder folder)
        {
            AzusaContext context = AzusaContext.GetInstance();

            bool exists = context.DatabaseDriver.MailArchive_TestForFolder(folder.id);

            if (exists)
                return false;

            context.DatabaseDriver.MailArchive_InsertFolder(folder);
            
            return true;
        }

        public static SearchQuery GetSearchQuery(Folder folder)
        {
            bool exists = false;
            AzusaContext context = AzusaContext.GetInstance();
            int count = context.DatabaseDriver.MailArchive_CountItemsInFolder(folder);
            exists = count > 0;

            if (!exists)
                return SearchQuery.All;

            long newestUtime = context.DatabaseDriver.MailArchive_GetHighestMessageUTimeInFolder(folder);
            if (newestUtime > 0)
            { 
                return SearchQuery.DeliveredAfter(UnixTimeConverter.FromUnixTime(newestUtime));
            }
            else
            {
                return SearchQuery.All;
            }
        }
    }
}
