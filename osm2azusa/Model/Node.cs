using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osm2azusa.Model
{
    class Node
    {
        public Node()
        {
            tags = new Dictionary<string, string>();
        }

        public long id;
        public double lat, lon;
        public Dictionary<string, string> tags;
    }
}
