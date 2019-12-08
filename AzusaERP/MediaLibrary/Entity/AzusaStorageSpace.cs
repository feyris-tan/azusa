using System.IO;
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
}