namespace moe.yo3explorer.azusa.dex.Schema
{
    public class Partition {
        
        private string nameField;
        
        private string idField;
        
        private string recordRevisionField;
        
        private string recordLengthField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RecordRevision {
            get {
                return this.recordRevisionField;
            }
            set {
                this.recordRevisionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RecordLength {
            get {
                return this.recordLengthField;
            }
            set {
                this.recordLengthField = value;
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}