using System;

namespace moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity
{
    public class FilesystemMetadataEntity
    {
        public long Id { get; set; }
        public int MediaId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsDirectory { get; set; }
        public string FullName { get; set; }
        public long? Size { get; set; }
        public DateTime? Modified { get; set; }
        public byte[] Header { get; set; }
        public long? ParentId { get; set; }
    }
}
