namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataData
    {
        internal GraphDataData()
        {

        }

        private string ImageFile;
        private long Sectors;
        private GraphDataDataType Type;
        private string VolumeIdentifier;

        public string ImageFile1
        {
            get { return ImageFile;}
            set { ImageFile = value;}
        }

        public long Sectors1
        {
            get { return Sectors; }
            set { Sectors = value;}
        }

        public GraphDataDataType Type1
        {
            get { return Type;}
            set { Type = value;}
        }

        public string VolumeIdentifier1
        {
            get { return VolumeIdentifier;}
            set { VolumeIdentifier = value;}
        }
    }
}