using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AzusaERP.OldStuff;

namespace moe.yo3explorer.azusa.Control.PasswordManagement.Boundary
{
    class PasswordManagement
    {
        private static bool failed;
        private static PwDatabase database;
        private static AzusaContext context;
        private static PwGroup group;

        public static string TryGetPassword(string url, string username)
        {
            if (group != null)
                return InternalGetPassword(url, username);

            if (failed)
                return null;

            context = AzusaContext.GetInstance();
            DirectoryInfo di = new DirectoryInfo(".");
            FileInfo dbFileInfo = di.GetFiles("*.kdbx").FirstOrDefault(x => x.Extension.ToLower().Equals(".kdbx"));
            if (dbFileInfo == null)
                dbFileInfo = di.Parent.GetFiles("*.kdbx").FirstOrDefault(x => x.Extension.ToLower().Equals(".kdbx"));
            if (dbFileInfo == null)
            {
                failed = true;
                return null;
            }

            string pw = null;
            context.Splash.Invoke((MethodInvoker)delegate
            {
                pw = TextInputForm.PromptPassword(String.Format("Passwort für {0}?", dbFileInfo.Name), context.Splash);
            });
            if (string.IsNullOrEmpty(pw))
            {
                failed = true;
                return null;
            }

            IOConnectionInfo connectionInfo = IOConnectionInfo.FromPath(dbFileInfo.FullName);
            KcpPassword kcpPassword = new KcpPassword(pw);
            CompositeKey compositeKey = new CompositeKey();
            compositeKey.AddUserKey(kcpPassword);
            database = new PwDatabase();
            try
            {
                database.Open(connectionInfo, compositeKey, null);
                group = FindPwGroup(database.RootGroup);
                if (group == null)
                {
                    failed = true;
                    return null;
                }
                return InternalGetPassword(url, username);
            }
            catch (InvalidCompositeKeyException)
            {
                failed = true;
                return null;
            }
        }

        private static string InternalGetPassword(string url, string username)
        {
            foreach(PwEntry entry in group.Entries)
            {
                string eUsername = entry.Strings.Get("UserName").ReadString();
                string eURL = entry.Strings.Get("URL").ReadString();

                if (url.Equals(eURL) && username.Equals(eUsername))
                {
                    return entry.Strings.Get("Password").ReadString();
                }
            }
            return null;
        }

        private static PwGroup FindPwGroup(PwGroup pwg)
        {
            foreach (PwGroup subGroup in pwg.Groups)
            {
                if (subGroup.Name.Equals("Azusa"))
                    return subGroup;

                PwGroup sub = FindPwGroup(subGroup);
                if (sub != null)
                    return sub;
            }
            return null;
        }
    }
}
