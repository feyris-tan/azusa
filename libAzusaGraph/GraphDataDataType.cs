namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphDataDataType
    {
        private byte mode;
        private byte form;
        private int sectorLength;
        private bool isCd;

        GraphDataDataType()
        {

        }

        public static GraphDataDataType parse(string s)
        {
            GraphDataDataType result = new GraphDataDataType();
            if (s.Equals("AUDIO/2352"))
            {
                result.mode = 42;
                result.sectorLength = 2352;
                result.isCd = true;
                return result;
            }
            
            s = s.Replace("MODE", "");
            s = s.Replace("FORM", "");
            string[] args = s.Split('/');
            if (args.Length == 2)
            {
                //for DVD & BD
                result.mode = byte.Parse(args[0]);
                result.sectorLength = int.Parse(args[1]);
            }
            else
            {
                //for CD
                result.mode = byte.Parse(args[0]);
                result.form = byte.Parse(args[1]);
                result.sectorLength = int.Parse(args[2]);
                result.isCd = true;
            }

            return result;
        }

        public byte Mode
        {
            get
            {
                return mode;
            }
        }

        public byte Form
        {
            get
            {
                return form;
            }
        }

        public int SectorLength
        {
            get
            {
                return sectorLength;
            }
        }
        
        public bool IsCd
        {
            get
            {
                return isCd;
            }
        }

        public override string ToString()
        {
            if (mode == 42)
            {
                return "AUDIO/2352";
            }
            
            if (isCd)
            {
                return string.Format("MODE{0}/FORM{1}/{2}", Mode, Form, SectorLength);
            }
            else
            {
                return string.Format("MODE{0}/{1}", Mode, SectorLength);
            }
        }
    }
}