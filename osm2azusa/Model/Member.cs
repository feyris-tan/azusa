using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace osm2azusa.Model
{
    class Member
    {
        public string type;
        public long reference;
        public string role;

        public override bool Equals(object obj)
        {
            var member = obj as Member;
            return member != null &&
                   type == member.type &&
                   reference == member.reference &&
                   role == member.role;
        }
    }
}
