using System;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

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
        [JsonConverter(typeof(WeirdQuarkusDatetimeConverter))]
        public DateTime? Modified { get; set; }
        public byte[] Header { get; set; }
        public long? ParentId { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(ParentId)}: {ParentId}, {nameof(FullName)}: {FullName}";
        }
    }
}
