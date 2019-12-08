using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class AlbumListAlbum
    {
        public string album_type;

        public string catalog;
        public string link;
        public string release_date;
        public Dictionary<string, string> titles;
        public string type;

        public int GetId()
        {
            if (link.StartsWith("album/"))
            {
                return Convert.ToInt32(link.Substring(6));
            }
            throw new NotSupportedException(String.Format("can't get album id from: " + link));
        }

        public override string ToString()
        {
            if (titles.Count > 0)
            {
                return titles.First().Value;
            }

            return link;
        }
    }
}
