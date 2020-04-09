using System;

namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataMedia
    {
        internal GraphDataMedia()
        {

        }

        private string type;
        private string bookType;
        private string id;
        private string trackPath;
        private float[] speeds;
        private long capacity;
        private long layerBreak;

        internal void ParseSpeeds(string value)
        {
            try
            {
                value = value.Replace("; ","");
                value = value.Replace(",", ".");
                string[] args = value.Split(new string[] {"x"}, StringSplitOptions.RemoveEmptyEntries);
                speeds = new float[args.Length];
                for (int i = 0; i < args.Length; i++)
                {
                    speeds[i] = float.Parse(args[i]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Could not parse speeds: {0}", value);
                speeds = new float[0];
            }
            
        }

        public string Type
        {
            get { return type;}
            set { type = value;}
        }

        public string BookType
        {
            get { return bookType;}
            set { bookType = value;}
        }

        public string Id
        {
            get { return id;}
            set { id = value; }
        }

        public string TrackPath
        {
            get { return trackPath;}
            set { trackPath = value;}
        }

        public float[] Speeds
        {
            get { return speeds;}
            set { speeds = value;}
        }

        public long Capacity
        {
            get { return capacity;}
            set { capacity = value;}
        }

        public long LayerBreak
        {
            get { return layerBreak;}
            set { layerBreak = value;}
        }
    }
}