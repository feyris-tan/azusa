using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class ProductRelease
    {
        public string date;
        public string link;
        public Dictionary<string, string> names;
        public string platform;
        public string region;

        public int GetId()
        {
            if (link.StartsWith("release/"))
                return Convert.ToInt32(link.Substring(8));
            throw new Exception("cant get release id");
        }
    }
}
