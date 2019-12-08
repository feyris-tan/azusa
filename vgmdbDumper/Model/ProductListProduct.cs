using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class ProductListProduct
    {
        public string link;
        public Dictionary<string, string> names;
        public string type;

        public int GetId()
        {
            if (link.StartsWith("product/"))
            {
                return Convert.ToInt32(link.Substring(8));
            }
            throw new NotSupportedException(String.Format("can't get label id from: " + link));
        }

        public bool IsRelease()
        {
            if (link == null)
                return false;

            return link.StartsWith("release/");
        }

        public ProductRelease ToRelease()
        {
            ProductRelease pr = new ProductRelease();
            pr.link = link;
            pr.names = names;
            return pr;
        }
    }
}
