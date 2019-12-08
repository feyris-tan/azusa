using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace libazusax360
{
    public class FSManipulator
    {
        public static long roundup2048(long bob)
        {
            if ((bob % 0x800L) != 0L)
            {
                bob += 0x800L - (bob % 0x800L);
            }
            return bob;
        }
    }
}