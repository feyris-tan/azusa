using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vndbDumper.Replay
{
    static class FileFormatConstants
    {
        public const ulong MAGIC_NUMER = 4921966419311426113;
        public const uint OPCODE_END_OF_FILE = 1;
        public const uint OPCODE_VNDB_REQUEST = 2;
        public const uint OPCODE_VNDB_RESPONSE = 3;
        public const uint OPCODE_BEGIN_RECORDING = 4;
        public const uint OPCODE_RESUME_RECORDING = 5;
        public const uint OPCODE_HTTP_OBJECT = 6;
    }
}
