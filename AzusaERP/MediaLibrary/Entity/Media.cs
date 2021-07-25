using System;
using System.IO;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary;
using moe.yo3explorer.azusa.Control.JsonIO;
using Newtonsoft.Json;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Media
    {
        public int Id { get; set; }
        public int RelatedProductId { get; set; }
        public string Name { get; set; }
        public int MediaTypeId { get; set; }
        public string SKU { get; set; }
        public bool isSealed { get; set; }
        public int DumpStorageSpaceId { get; set; }
        public string DumpStorageSpacePath { get; set; }
        public string MetaFileContent { get; set; }
        [JsonConverter(typeof(WeirdQuarkusDatetimeConverter))]
        public DateTime DateAdded { get; set; }
        public string GraphDataContent { get; set; }
        public bool IsSealed { get; set; }
        [JsonConverter(typeof(WeirdQuarkusDatetimeConverter))]
        public DateTime DateUpdated { get; set; }

        public void SetDumpFile(FileInfo fi)
        {
            //Path setzen
            DumpStorageSpacePath = fi.FullName.Substring(fi.Directory.Root.FullName.Length);

            //SpaceId setzen
            string rootName = fi.Directory.Root.FullName;
            string mediaId = Path.Combine(rootName, "azusa_storagespace_id.xml");
            if (File.Exists(mediaId))
            {
                var ami = AzusaStorageSpace.Load(new FileInfo(mediaId));
                if (ami.MediaNo > 0)
                {
                    DumpStorageSpaceId = (int)ami.MediaNo;
                }
            }
        }

        public void SetFilesystemMetadata(Stream inputStream)
        {
            FilesystemMetadataGatherer.Gather(this, inputStream);
        }
    }
}
