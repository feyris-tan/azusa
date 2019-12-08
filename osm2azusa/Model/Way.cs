using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osm2azusa.Model
{
    class Way
    {
        public Way()
        {
            tags = new Dictionary<string, string>();
            nodes = new List<long>();
        }

        public long id;
        public Dictionary<string, string> tags;
        public List<long> nodes;
    }
}
