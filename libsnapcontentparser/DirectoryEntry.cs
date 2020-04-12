using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class DirectoryEntry
    {
        public DirectoryEntry(DataDisk disk, string name)
        {
            this.Disk = disk;
            this.Name = name;
        }

        public DataDisk Disk { get; private set; }
        public string Name { get; private set; }
    }
}
