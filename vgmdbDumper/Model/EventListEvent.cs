using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class EventListEvent
    {
        public string enddate;
        public string link;
        public Dictionary<string, string> names;
        public string shortname;
        public string startdate;

        public int GetId()
        {
            if (link.StartsWith("event/"))
            {
                return Convert.ToInt32(link.Substring(6));
            }
            throw new NotSupportedException(String.Format("can't get event id from: " + link));
        }
    }
}
