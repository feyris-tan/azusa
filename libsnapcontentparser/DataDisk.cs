using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class DataDisk
    {
        public DataDisk(string name, uint vPos, uint totalBlocks, uint freeBlocks, Guid guid)
        {
            this.Name = name;
            this.VPosition = vPos;
            this.TotalBlocks = totalBlocks;
            this.FreeBlocks = freeBlocks;
            this.Guid = guid;
        }

        public string Name { get; private set; }
        public uint VPosition { get; private set; }
        public uint TotalBlocks { get; private set; }
        public uint FreeBlocks { get; private set; }
        public Guid Guid { get; private set; }
    }
}
