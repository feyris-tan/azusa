using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class AzusaStorageSpace
    {
        private static XmlSerializer _xmlSerializer;

        public static AzusaStorageSpace Load(FileInfo fi)
        {
            if (_xmlSerializer == null)
            {
                _xmlSerializer = new XmlSerializer(typeof(AzusaStorageSpace));
            }

            var fileStream = fi.OpenRead();
            object result = _xmlSerializer.Deserialize(fileStream);
            fileStream.Close();

            return (AzusaStorageSpace) result;
        }

        public void Save(string fi)
        {
            if (_xmlSerializer == null)
            {
                _xmlSerializer = new XmlSerializer(typeof(AzusaStorageSpace));
            }

            var fileStream = File.OpenWrite(fi);
            _xmlSerializer.Serialize(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
        }

        public long MediaNo { get; set; }
    }

    public class AzusaStorageSpaceDrive
    {
        private AzusaStorageSpaceDrive() { }

        public AzusaStorageSpace StorageSpace { get; private set; }
        public DirectoryInfo RootDirectory { get; private set; }
        public int Ordinal { get; private set; }

        public static IEnumerable<AzusaStorageSpaceDrive> FindConnectedStorageSpaces()
        {
            int ordinal = 0;
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                string xmlFilename = Path.Combine(driveInfo.RootDirectory.FullName, "azusa_storagespace_id.xml");
                FileInfo xmlFile = new FileInfo(xmlFilename);
                if (xmlFile.Exists)
                {
                    AzusaStorageSpaceDrive drive = new AzusaStorageSpaceDrive();
                    drive.StorageSpace = AzusaStorageSpace.Load(xmlFile);
                    drive.RootDirectory = driveInfo.RootDirectory;
                    drive.Ordinal = ordinal++;
                    yield return drive;
                }
            }
        }

        public static FileInfo FindFileOnConnectedSpaces(string file)
        {
            foreach (AzusaStorageSpaceDrive space in FindConnectedStorageSpaces())
            {
                FileInfo fi = new FileInfo(Path.Combine(space.RootDirectory.FullName, file));
                if (fi.Exists)
                    return fi;
            }

            return null;
        }
    }
}