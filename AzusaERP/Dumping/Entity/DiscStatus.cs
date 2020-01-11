using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Dumping.Entity
{
    public class DiscStatus
    {
        public long DiscId { get; set; }
        public DateTime DateAdded { get; set; }
        public int PgSerial { get; set; }
        public DirectoryInfo Path { get; set; }
        public bool Dumped { get; set; }
        public bool Ripped { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public bool AzusaLinked { get; set; }
    }
}
