namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataDevice
    {
        internal GraphDataDevice()
        {

        }

        private string address;
        private string makeModel;
        private string firmwareVersion;
        private string driveLetter;
        private string busType;

        public string Address
        {
            get { return address; }
            set { address = value;}
        }

        public string MakeModel
        {
            get { return makeModel; }
            set { makeModel = value;}
        }

        public string FirmwareVersion
        {
            get { return firmwareVersion;}
            set { firmwareVersion = value;}
        }

        public string DriveLetter
        {
            get { return driveLetter;}
            set { driveLetter = value;}
        }

        public string BusType
        {
            get { return busType;}
            set { busType = value;}
        }

        public override string ToString() {
            return string.Format("[{0}] {1} {2} ({3}:) ({4})",address,makeModel,firmwareVersion,driveLetter,busType);
        }
    }
}