using System.IO;
using System.Xml.Serialization;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class PartitionInfo {

        public static PartitionInfo Parse(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            XmlSerializer xs = new XmlSerializer(typeof(PartitionInfo));
            PartitionInfo partitionInfo = (PartitionInfo)xs.Deserialize(ms);
            ms.Dispose();
            return partitionInfo;
        }
            
        private Partition[] partitionField;
        
        private string schemaVersionField;
        
        private string pageHeaderVersionField;
        
        private string pageDataLengthField;
        
        /// <remarks/>
        [XmlElement("Partition", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Partition[] Partition {
            get {
                return this.partitionField;
            }
            set {
                this.partitionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string SchemaVersion {
            get {
                return this.schemaVersionField;
            }
            set {
                this.schemaVersionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string PageHeaderVersion {
            get {
                return this.pageHeaderVersionField;
            }
            set {
                this.pageHeaderVersionField = value;
            }
        }
        
        /// <remarks/>
        [XmlAttribute()]
        public string PageDataLength {
            get {
                return this.pageDataLengthField;
            }
            set {
                this.pageDataLengthField = value;
            }
        }
    }
}