using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moe.yo3explorer.azusa.Control.DatabaseIO.Migrations;

namespace AzusaERP.OldStuff
{
    static class Extensions
    {
        public static void AddAll(this HashSet<int> coll, IEnumerable<int> addMe)
        {
            foreach (var i in addMe)
                coll.Add(i);
        }

        public static List<AttachmentMigrationCandidate> ToList(this IEnumerable<AttachmentMigrationCandidate> coll)
        {
            List<AttachmentMigrationCandidate> result = new List<AttachmentMigrationCandidate>();
            foreach (AttachmentMigrationCandidate attachmentMigrationCandidate in coll)
            {
                result.Add(attachmentMigrationCandidate);
            }
            return result;
        }
    }
}
