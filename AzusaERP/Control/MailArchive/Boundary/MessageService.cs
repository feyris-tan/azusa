using System.IO;
using System.Security.Cryptography;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;

namespace moe.yo3explorer.azusa.Control.MailArchive.Boundary
{
    static class MessageService
    {
        public static void StoreMessage(Mail mail)
        {
            if (TestForMessage(mail.Uid))
                return;

            AzusaContext.GetInstance().DatabaseDriver.MailArchive_StoreMessage(mail);
        }
        
        public static bool TestForMessage(int uid)
        {
            return AzusaContext.GetInstance().DatabaseDriver.MailArchive_TestForMessage(uid);
        }
        
        public static Mail GetSpecificMessage(int uid)
        {
            return AzusaContext.GetInstance().DatabaseDriver.MailArchive_GetSpecificMessage(uid);
        }

        public static byte[] DecryptMessage(Mail message, string password)
        {
            int length = message.Data.Length;
            length = (length / 16) * 16;

            Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, message.Salt, message.Iterations);
            Aes aes = Aes.Create();
            aes.Key = deriveBytes.GetBytes(32);
            aes.IV = deriveBytes.GetBytes(16);
            aes.Padding = PaddingMode.None;
            MemoryStream ms = new MemoryStream(message.Data);
            CryptoStream cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] buffer = new byte[length];
            int result = cryptoStream.Read(buffer, 0, length);

            if (length != result)
                throw new CryptographicException();

            return buffer;
        }


        public static int GetLatestMessageId()
        {
            return AzusaContext.GetInstance().DatabaseDriver.MailArchive_GetLatestMessageId();
        }
    }
}
