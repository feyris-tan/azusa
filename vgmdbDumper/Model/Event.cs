using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Event
    {
        public string enddate;
        public string link;
        public string name;
        public string notes;
        public AlbumListAlbum[] releases;
        public string startdate;
        public string vgmdb_link;

        public int GetId()
        {
            if (link.StartsWith("event/"))
                return Convert.ToInt32(link.Substring(6));
            throw new Exception("cant get event id");
        }
    }
}
