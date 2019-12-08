using System;

namespace moe.yo3explorer.azusa.Control.MailArchive.Entity
{
    public class Mail
    {
        public int Uid { get; set; }
        public int MessageUtime { get; set; }
        public long Folder { get; set; }
        public DateTime DateAdded { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public byte[] Salt { get; set; }
        public short Iterations { get; set; }
        public byte[] Data { get; set; }
    }
}
