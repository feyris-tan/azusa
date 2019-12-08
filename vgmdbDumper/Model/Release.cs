using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Release
    {
        public string catalog;
        public string link;
        public ProductMeta meta;
        public string name;
        public string name_real;
        public string platform;
        public AlbumListAlbum[] product_albums;
        public ProductListProduct[] products;
        public string region;
        public AlbumListAlbum[] release_albums;
        public string release_date;
        public string release_type;
        public string type;
        public string upc;
        public string vgmdb_link;

        public int GetId()
        {
            if (link.StartsWith("release/"))
            {
                return Convert.ToInt32(link.Substring(8));
            }
            throw new NotSupportedException(String.Format("can't get label id from: " + link));
        }
    }
}
