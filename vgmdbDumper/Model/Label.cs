using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Label
    {
        public string description;
        public string link;
        public ProductMeta meta;
        public string name;
        public string region;
        public AlbumListAlbum[] releases;
        public LabelStaff[] staff;
        public string type;
        public string vgmdb_link;
        public Dictionary<string, Website[]> websites;

        public int GetId()
        {
            if (link.StartsWith("org/"))
                return Convert.ToInt32(link.Substring(4));
            throw new Exception("could not get label id");
        }
    }
}
