using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class LabelListLabel
    {
        public string link;
        public Dictionary<string, string> names;
        public LabelListLabel[] imprints;
        public LabelListLabel[] formerly;
        public LabelListLabel[] subsidiaries;

        public int GetId()
        {
            if (link.StartsWith("org/"))
            {
                return Convert.ToInt32(link.Substring(4));
            }
            throw new NotSupportedException(String.Format("can't get label id from: " + link));
        }
    }
}
