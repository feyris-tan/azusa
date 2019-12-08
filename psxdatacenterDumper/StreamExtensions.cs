using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psxdatacenterDumper
{
    static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream inStream)
        {
            MemoryStream ms = new MemoryStream();
            inStream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
