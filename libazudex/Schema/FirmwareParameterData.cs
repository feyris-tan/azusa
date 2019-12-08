using System.IO;
using System.Xml.Serialization;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class FirmwareParameterData : GenericXmlRecord
    {
        public FirmwareParameterData(BinaryReader br) : base(br)
        {
            StringReader sr = new StringReader(xml);
            XmlSerializer xs = new XmlSerializer(typeof(FPR));
            fpr = (FPR)xs.Deserialize(sr);
        }

        private FPR fpr;

        public FPR FPR
        {
            get { return fpr; }
        }
    }
    
    public class FPR {
        
        private object[] itemsField;
        
        [XmlElement("AlgorithmSettings", typeof(FPRAlgorithmSettings), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlElement("FirmwareSettings", typeof(FPRFirmwareSettings), Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public object[] Items {
            get {
                return this.itemsField;
            }
            set {
                this.itemsField = value;
            }
        }
    }
    
    public class FPRAlgorithmSettings {
        
        private string maxSField;
        
        private string minSField;
        
        private string minBField;
        
        private string maxBField;
        
        private string lowABField;
        
        private string midABField;
        
        private string highABField;
        
        private string uABPField;
        
        private string lABPField;
        
        private string mUAPField;
        
        private string mDAPField;
        
        private string lNAField;
        
        private string mNAField;
        
        private string hNAField;
        
        private string saField;
        
        private string lNTField;
        
        private string mNTField;
        
        private string hNTField;
        
        private string minCAField;
        
        private string maxCAField;
        
        private string mACAField;
        
        private string cASWField;
        
        private string mAADRAField;
        
        private string aDRASWField;
        
        private string aDRTField;
        
        private string aRTField;
        
        private string mBGBDOField;
        
        private string mDCTField;
        
        private string mBGDField;
        
        private string mELFTField;
        
        [XmlAttribute()]
        public string MaxS {
            get {
                return this.maxSField;
            }
            set {
                this.maxSField = value;
            }
        }
        
        [XmlAttribute()]
        public string MinS {
            get {
                return this.minSField;
            }
            set {
                this.minSField = value;
            }
        }
        
        [XmlAttribute()]
        public string MinB {
            get {
                return this.minBField;
            }
            set {
                this.minBField = value;
            }
        }
        
        [XmlAttribute()]
        public string MaxB {
            get {
                return this.maxBField;
            }
            set {
                this.maxBField = value;
            }
        }
        
        [XmlAttribute()]
        public string LowAB {
            get {
                return this.lowABField;
            }
            set {
                this.lowABField = value;
            }
        }
        
        [XmlAttribute()]
        public string MidAB {
            get {
                return this.midABField;
            }
            set {
                this.midABField = value;
            }
        }
        
        [XmlAttribute()]
        public string HighAB {
            get {
                return this.highABField;
            }
            set {
                this.highABField = value;
            }
        }
        
        [XmlAttribute()]
        public string UABP {
            get {
                return this.uABPField;
            }
            set {
                this.uABPField = value;
            }
        }
        
        [XmlAttribute()]
        public string LABP {
            get {
                return this.lABPField;
            }
            set {
                this.lABPField = value;
            }
        }
        
        [XmlAttribute()]
        public string MUAP {
            get {
                return this.mUAPField;
            }
            set {
                this.mUAPField = value;
            }
        }
        
        [XmlAttribute()]
        public string MDAP {
            get {
                return this.mDAPField;
            }
            set {
                this.mDAPField = value;
            }
        }
        
        [XmlAttribute()]
        public string LNA {
            get {
                return this.lNAField;
            }
            set {
                this.lNAField = value;
            }
        }
        
        [XmlAttribute()]
        public string MNA {
            get {
                return this.mNAField;
            }
            set {
                this.mNAField = value;
            }
        }
        
        [XmlAttribute()]
        public string HNA {
            get {
                return this.hNAField;
            }
            set {
                this.hNAField = value;
            }
        }
        
        [XmlAttribute()]
        public string SA {
            get {
                return this.saField;
            }
            set {
                this.saField = value;
            }
        }
        
        [XmlAttribute()]
        public string LNT {
            get {
                return this.lNTField;
            }
            set {
                this.lNTField = value;
            }
        }
        
        [XmlAttribute()]
        public string MNT {
            get {
                return this.mNTField;
            }
            set {
                this.mNTField = value;
            }
        }
        
        [XmlAttribute()]
        public string HNT {
            get {
                return this.hNTField;
            }
            set {
                this.hNTField = value;
            }
        }
        
        [XmlAttribute()]
        public string MinCA {
            get {
                return this.minCAField;
            }
            set {
                this.minCAField = value;
            }
        }
        
        [XmlAttribute()]
        public string MaxCA {
            get {
                return this.maxCAField;
            }
            set {
                this.maxCAField = value;
            }
        }
        
        [XmlAttribute()]
        public string MACA {
            get {
                return this.mACAField;
            }
            set {
                this.mACAField = value;
            }
        }
        
        [XmlAttribute()]
        public string CASW {
            get {
                return this.cASWField;
            }
            set {
                this.cASWField = value;
            }
        }
        
        [XmlAttribute()]
        public string MAADRA {
            get {
                return this.mAADRAField;
            }
            set {
                this.mAADRAField = value;
            }
        }
        
        [XmlAttribute()]
        public string ADRASW {
            get {
                return this.aDRASWField;
            }
            set {
                this.aDRASWField = value;
            }
        }
        
        [XmlAttribute()]
        public string ADRT {
            get {
                return this.aDRTField;
            }
            set {
                this.aDRTField = value;
            }
        }
        
        [XmlAttribute()]
        public string ART {
            get {
                return this.aRTField;
            }
            set {
                this.aRTField = value;
            }
        }
        
        [XmlAttribute()]
        public string MBGBDO {
            get {
                return this.mBGBDOField;
            }
            set {
                this.mBGBDOField = value;
            }
        }
        
        [XmlAttribute()]
        public string MDCT {
            get {
                return this.mDCTField;
            }
            set {
                this.mDCTField = value;
            }
        }
        
        [XmlAttribute()]
        public string MBGD {
            get {
                return this.mBGDField;
            }
            set {
                this.mBGDField = value;
            }
        }
        
        [XmlAttribute()]
        public string MELFT {
            get {
                return this.mELFTField;
            }
            set {
                this.mELFTField = value;
            }
        }
    }
    
    public class FPRFirmwareSettings {
        
        private string firmwareImageIdField;
        
        [XmlAttribute()]
        public string FirmwareImageId {
            get {
                return this.firmwareImageIdField;
            }
            set {
                this.firmwareImageIdField = value;
            }
        }
    }

}