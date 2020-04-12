using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class FileEntry
    {
        public FileEntry(DataDisk disk, ulong size, ulong mtime, uint mtimeNsec, ulong inode, string filename, uint blockSize)
        {
            this.Disk = disk;
            this.Length = size;

            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(mtime);
            if (mtimeNsec != UInt32.MaxValue)
            {
                mtimeNsec /= 1000000;
                dtDateTime = dtDateTime.AddMilliseconds(mtimeNsec);
            }

            this.DateModified = dtDateTime;
            this.FileName = filename;
            this.NumBlocks = (size + blockSize - 1) / blockSize;
            this.Blocks = new Block[this.NumBlocks];
        }

        public DataDisk Disk { get; private set; }
        public ulong Length { get; private set; }
        public DateTime DateModified { get; private set; }
        public string FileName { get; private set; }
        public ulong NumBlocks { get; private set; }
        public Block[] Blocks { get; private set; }
    }

}
