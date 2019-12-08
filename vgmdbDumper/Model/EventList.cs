using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class EventList
    {
        public Dictionary<string, EventListEvent[]> events;
        public string link;
        public AlbumListMeta meta;
        public string vgmdb_link;
        public string[] years;
    }
}
