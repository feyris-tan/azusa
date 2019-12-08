using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osm2azusa.Model
{
    class Relation
    {
        public Relation()
        {
            tags = new Dictionary<string, string>();
            members = new List<Member>();
        }

        public long id;
        public Dictionary<string, string> tags;
        public List<Member> members;
    }
}
