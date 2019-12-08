using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace libazuwarc
{
    static class BinaryReaderExtensions
    {
        public static string ReadAsciiLine(this BinaryReader br)
        {
            string result = "";
            while (!result.EndsWith("\n"))
                result += (char) br.ReadByte();

            result = result.Substring(0, result.Length - 1);
            
            if (result.EndsWith("\r"))
                result = result.Substring(0, result.Length - 1);

            return result;
        }
    }
}
