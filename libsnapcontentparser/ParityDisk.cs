using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class ParityDisk
    {
        public ParityDisk(uint vLevel, uint vTotalBlocks, uint vFreeBlocks, Guid guid)
        {
            this.ParityLevel = vLevel;
            this.TotalBlocks = vTotalBlocks;
            this.FreeBlocks = vFreeBlocks;
            this.Guid = guid;
        }

        public uint ParityLevel { get; private set; }
        public uint TotalBlocks { get; private set; }
        public uint FreeBlocks { get; private set; }
        public Guid Guid { get; private set; }
    }
}
