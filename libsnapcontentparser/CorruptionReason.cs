using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public enum CorruptionReason
    {
        InvalidChecksum,
        CantReadSeed,
        InvalidParityLevel,
        ZeroBlockSize,
        FileTooBig,
        NullFilename,
        InvalidBlockNumber,
        InvalidBlockSize,
        InvalidBlockType,
        InconsistentHoleSize,
        InvalidHoleType,
        InconsistentInfoSize,
        InconsistentMissingPreviousChecksum
    }
}
