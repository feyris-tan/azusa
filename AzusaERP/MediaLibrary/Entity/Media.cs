using System;
using System.IO;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary;

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
        public DateTime DateAdded { get; set; }
        public string GraphDataContent { get; set; }
        public string CueSheetContent { get; set; }
        public string ChecksumContent { get; set; }
        public string PlaylistContent { get; set; }
        public byte[] CdTextContent { get; set; }
        public string LogfileContent { get; set; }
        public byte[] MdsContent { get; set; }
        public DateTime DateUpdated { get; set; }
        public long FauxHash { get; set; }

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

            //FauxHash setzen
            byte[] buffer = new byte[512];
            string filename = fi.FullName;
            FileStream fs = File.OpenRead(filename);
            int headerLen = fs.Read(buffer, 0, 512);
            fs.Dispose();

            long result = 0;
            for (int i = 0; i < headerLen; i++)
            {
                result += buffer[i];
                result <<= 1;
            }

            FauxHash = result;
        }

        public void SetFilesystemMetadata(Stream inputStream)
        {
            FilesystemMetadataGatherer.Gather(this, inputStream);
        }
    }
}
