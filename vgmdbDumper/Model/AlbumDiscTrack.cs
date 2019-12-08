using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class AlbumDiscTrack
    {
        public Dictionary<string, string> names;
        public string track_length;

        public int GetTrackLengthSeconds()
        {
            if (string.IsNullOrEmpty(track_length))
                return -1;
            if (track_length.Equals("Unknown"))
                return -2;

            string[] args = track_length.Split(':');
            if (args.Length == 2)
            {
                int minute = Convert.ToInt32(args[0]);
                int second = Convert.ToInt32(args[1]);
                return (minute * 60) + second;
            }
            else
            {
                throw new NotImplementedException(args.Length.ToString());
            }
        }
    }
}
