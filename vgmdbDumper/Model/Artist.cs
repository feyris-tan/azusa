using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Artist
    {
        public ArtistAlias[] aliases;
        public string birthdate;
        public string birthplace;
        public AlbumListAlbum[] discography;
        public AlbumListAlbum[] featured_on;
        public object info;
        public string link;
        public ProductMeta meta;
        public string name;
        public string notes;
        public string picture_full;
        public string picture_small;
        public string sex;
        public string type;
        public string vgmdb_link;
        public Dictionary<string, Website[]> websites;

        public int GetId()
        {
            if (link.StartsWith("artist/"))
                return Convert.ToInt32(link.Substring(7));
            throw new Exception("cant get artist id");
        }
    }
}
