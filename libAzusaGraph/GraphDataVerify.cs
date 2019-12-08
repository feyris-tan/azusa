namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataVerify
    {
        internal GraphDataVerify()
        {
        }

        private double speedStart;
        private double speedEnd;
        private double speedAverage;
        private double speedMax;
        private long timeTaken;

        public double SpeedStart
        {
            get { return speedStart;}
            set { speedStart = value;}
        }

        public double SpeedEnd
        {
            get { return speedEnd; } 
            set { speedEnd = value;}
        }

        public double SpeedAverage
        {
            get { return speedAverage; }
            set { speedAverage = value;}
        }

        public double SpeedMax
        {
            get { return speedMax;}
            set { speedMax = value;}
        }

        public long TimeTaken
        {
            get { return timeTaken;}
            set { timeTaken = value; }
        }
    }
}