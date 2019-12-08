using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class LabelList
    {
        public string[] letters;
        public string link;
        public AlbumListMeta meta;
        public Dictionary<string, LabelListLabel[]> orgs;
        public string vgmdb_link;
    }
}
