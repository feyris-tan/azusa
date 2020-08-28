using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.Control.MailArchive.Boundary;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;

namespace moe.yo3explorer.azusa.Control.MailArchive.Control
{
    class MailFetchTask : IPostConnectionTask
    {
        const string iniCategoryName = "mailarchive";
        const string portName = "port";
        const string sslName = "ssl";
        const string passwordName = "password";
        const string lastMailTimestampFilename = "lastmailTimest.amp";

        private static readonly byte[] azusaString = { 0x41, 0x7A, 0x75, 0x73, 0x61, 0x00 };

        public void ExecutePostConnectionTask()
        {
            if (File.Exists(lastMailTimestampFilename))
            {
                DateTime lastTime = new DateTime(Convert.ToInt64(File.ReadAllText(lastMailTimestampFilename)));
                TimeSpan sinceLastTime = DateTime.Now - lastTime;
                if (sinceLastTime.TotalMinutes < 11)
                {
                    return;
                }
            }


            AzusaContext context = AzusaContext.GetInstance();
            if (!context.Ini.ContainsKey(iniCategoryName))
                return;

            if (context.Ini[iniCategoryName].ContainsKey("enabled"))
            {
                bool isEnabled = Convert.ToInt32(context.Ini[iniCategoryName]["enabled"]) > 0;
                if (!isEnabled)
                    return;
            }
            
            bool allcerts = Convert.ToInt32(context.Ini[iniCategoryName]["acceptAllCerts"]) > 0;
            string server = context.Ini[iniCategoryName]["server"];
            int port = context.Ini[iniCategoryName].ContainsKey(portName) ? Convert.ToUInt16(context.Ini[iniCategoryName][portName]) : 143;
            bool useSsl = Convert.ToInt32(context.Ini[iniCategoryName]["ssl"]) > 0;
            string username = context.Ini[iniCategoryName]["username"];
            string password = context.Ini[iniCategoryName].ContainsKey(passwordName) ? context.Ini[iniCategoryName][passwordName] : "";
            
            if (string.IsNullOrEmpty(password))
            {
                password = PasswordManagement.Boundary.PasswordManagement.TryGetPassword(server, username);
            }

            if (string.IsNullOrEmpty(password))
            {
                context.Splash.Invoke((MethodInvoker) delegate { password = TextInputForm.PromptPassword(String.Format("Passwort für {0} auf {1}?", username, server), context.Splash); });
                if (string.IsNullOrEmpty(password))
                    return;
            }


            ImapClient client = new ImapClient(new MailProtocolLogger());
            if (allcerts)
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            client.Connect(server, port, useSsl);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(username, password);
            FolderNamespace rootFolderNamespace = client.PersonalNamespaces[0];
            IList<IMailFolder> folders = client.GetFolders(rootFolderNamespace);
            foreach(IMailFolder folder in folders)
            {
                Folder child = new Folder();
                child.id = MakeId(folder);
                child.name = folder.Name;
                child.parentId = MakeId(folder.ParentFolder);
                bool created = FolderService.CreateIfNotExists(child);
                CopyFolder(folder, child, !created, password);
            }
            client.Disconnect(true);

            File.WriteAllText(lastMailTimestampFilename, DateTime.Now.Ticks.ToString());
        }

        private const MessageSummaryItems fetchSummary = MessageSummaryItems.GMailMessageId | MessageSummaryItems.Id | MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope;
        public void CopyFolder(IMailFolder folder, Folder sqlEntity, bool update, string password)
        {
            try
            {
                folder.Open(FolderAccess.ReadOnly);
            }
            catch (Exception)
            {
                return;
            }

            Random rng = AzusaContext.GetInstance().RandomNumberGenerator;
            IList<UniqueId> uuids = folder.Search(FolderService.GetSearchQuery(sqlEntity));
            foreach(UniqueId uuid in uuids)
            {
                if (MessageService.TestForMessage((int)uuid.Id))
                    continue;

                var message = folder.GetMessage(uuid);

                byte[] saltBuffer = new byte[16];
                rng.NextBytes(saltBuffer);
                Array.Copy(azusaString, 0, saltBuffer, 0, azusaString.Length);
                int iterations = rng.Next(1000, short.MaxValue);
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, saltBuffer, iterations);
                Aes aes = Aes.Create();
                aes.Key = deriveBytes.GetBytes(32);
                aes.IV = deriveBytes.GetBytes(16);
                MemoryStream ms = new MemoryStream();
                CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                message.WriteTo(cryptoStream);
                cryptoStream.Flush();

                Mail child = new Mail();
                child.Uid = (int)uuid.Id;
                child.MessageUtime = message.Date.DateTime.ToUnixTime();
                child.Folder = sqlEntity.id;
                child.From = message.From[0].ToString();
                if (message.To.Count > 0)
                    child.To = message.To[0].ToString();
                else
                    child.To = null;
                child.Subject = message.Subject;
                child.Salt = saltBuffer;
                child.Iterations = (short)iterations;
                child.Data = new byte[ms.Position];
                Array.Copy(ms.GetBuffer(), 0, child.Data, 0, ms.Position);

                MessageService.StoreMessage(child);
            }

            folder.Close(false);
        }

        public long MakeId(IMailFolder folder)
        {
            return (long)HashLib.HashFactory.Hash64.CreateMurmur2().ComputeString(folder.FullName).GetULong();
        }
    }
}
