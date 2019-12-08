using System.Collections.Generic;

namespace libazusax360
{
    public class FSOutSectorComparer : IComparer<XDvdFsFileSystemEntry>
    {
        // Methods
        public int Compare(XDvdFsFileSystemEntry x, XDvdFsFileSystemEntry y)
        {
            return (x.StartSector - y.StartSector);
        }
    }
}