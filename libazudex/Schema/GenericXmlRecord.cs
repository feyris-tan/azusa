using System.IO;
using System.Text;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class GenericXmlRecord : GenericTimestampRecord
    {
        public GenericXmlRecord(BinaryReader br)
        {
            systemTime = br.ReadUInt32();
            displayTime = br.ReadUInt32();

            xml = Encoding.UTF8.GetString(br.ReadBytes(490));
            while (xml.EndsWith("\0"))
                xml = xml.Substring(0, xml.Length - 1);

            crc = br.ReadUInt16();
        }

        protected GenericXmlRecord()
        {

        }

        protected string xml;
    }
}