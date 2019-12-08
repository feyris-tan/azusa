using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class Product
    {
        public AlbumListAlbum[] albums;
        public string description;
        public ProductListProduct[] franchises;
        public string link;
        public ProductMeta meta;
        public string name;
        public string name_real;
        public LabelListLabel[] organizations;
        public string picture_full;
        public string picture_small;
        public string release_date;
        public ProductRelease[] releases;
        public string type;
        public string vgmdb_link;
        public Dictionary<string, Website[]> websites;

        public int GetId()
        {
            if (link.StartsWith("product/"))
            {
                return Convert.ToInt32(link.Substring(8));
            }
            throw new NotSupportedException(String.Format("can't get label id from: " + link));
        }
    }
}
