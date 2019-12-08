using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class AlbumOrganization
    {
        public string link;
        public Dictionary<string, string> names;
        public string role;

        public int GetId()
        {
            if (link.StartsWith("org/"))
                return Convert.ToInt32(link.Substring(4));
            throw new Exception("could not get label id");
        }

        protected bool Equals(AlbumOrganization other)
        {
            return string.Equals(link, other.link) && string.Equals(role, other.role);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AlbumOrganization) obj);
        }
        
    }
}
