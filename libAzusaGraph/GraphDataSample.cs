namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataSample
    {
        internal GraphDataSample()
        {

        }

        private float readSpeed;
        private long sectorNo;
        private int sampleDistance;
        private float cpuLoad;
        private double averageCpuLoad;
        private double averageReadSpeed;
        
        public float ReadSpeed
        {
            get { return readSpeed;}
            internal set { readSpeed = value;}
        }

        public long SectorNo
        {
            get { return sectorNo;}
            internal set { sectorNo = value;}
        }

        public int SampleDistance
        {
            get { return sampleDistance;}
            internal set { sampleDistance = value;}
        }

        public float CpuLoad
        {
            get { return cpuLoad; }
            internal set { cpuLoad = value; }
        }

        public double AverageCpuLoad
        {
            get { return averageCpuLoad; }
            set { averageCpuLoad = value; }
        }

        public double AverageReadSpeed
        {
            get {return averageReadSpeed;}
            set { averageReadSpeed = value; }
        }
    }
}