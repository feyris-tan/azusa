using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class ArtistListArtist
    {
        public string link;
        public Dictionary<string, string> names;
        public string name_real;

        public int GetId()
        {
            if (link.StartsWith("artist/"))
            {
                return Convert.ToInt32(link.Substring(7));
            }
            throw new NotSupportedException(String.Format("can't get artist id from: " + link));
        }

        public bool IsNatural()
        {
            if (string.IsNullOrEmpty(link))
                return false;

            return link.StartsWith("artist/");
        }
    }
}
