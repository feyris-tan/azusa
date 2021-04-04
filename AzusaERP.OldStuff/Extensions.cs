using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    static class Extensions
    {
        public static void AddAll(this HashSet<int> coll, IEnumerable<int> addMe)
        {
            foreach (var i in addMe)
                coll.Add(i);
        }
    }
}
