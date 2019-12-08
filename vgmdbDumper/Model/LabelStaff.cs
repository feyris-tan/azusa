using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class LabelStaff
    {
        public string link;
        public Dictionary<string, string> names;
        public bool owner;

        public int GetId()
        {
            if (link.StartsWith("artist/"))
                return Convert.ToInt32(link.Substring(7));
            throw new Exception("cant get staff id");
        }
    }
}
