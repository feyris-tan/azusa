using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class BlockInfo
    {
        public BlockInfo(bool bad, bool rehashed, bool justSynced, DateTime dtDateTime)
        {
            this.Bad = bad;
            this.Rehashed = rehashed;
            this.JustSynced = justSynced;
            this.Timestamp = dtDateTime;
        }

        public bool Bad { get; private set; }
        public bool Rehashed { get; private set; }
        public bool JustSynced { get; private set; }
        public DateTime Timestamp { get; private set; }
    }
}
