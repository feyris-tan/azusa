using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{
    public class SnapraidContent
    {
        private SnapraidContent() { }
        private const int SNAP = 1346457171;
        private const int CNT1 = 827608643;
        private const int CNT2 = 844385859;
        private const int CNT3 = 861163075;
        private const int N300 = 778;
        private const int HASH_MAX = 16;
        private const int LEV_MAX = 6;
        private const int BLOCK_HASH_SIZE = HASH_MAX;

        private List<DataDisk> dataDisks;
        private List<ParityDisk> parityDisks;
        private List<FileEntry> files;
        private List<DirectoryEntry> directories;

        public uint BlockSize { get; private set; }
        public uint MaximumBlockSize { get; private set; }
        public HashAlgorithm HashAlgorithm { get; private set; }
        public CorruptionReason? CorruptionReason { get; private set; }
        public bool IsCorrupted { get; private set; }
        public byte[] HashSeed { get; private set; }
        public ReadOnlyCollection<DataDisk> DataDisks => dataDisks.AsReadOnly();
        public ReadOnlyCollection<ParityDisk> ParityDisks => parityDisks.AsReadOnly();
        public ReadOnlyCollection<FileEntry> FileEntries => files.AsReadOnly();
        public ulong FileCount { get; private set; }
        public ReadOnlyCollection<DirectoryEntry> DirectoryEntries => directories.AsReadOnly();
        public ulong DirectoryCount { get; private set; }
        public BlockInfo[] Infos { get; private set; }
        public int StoredCRC { get; private set; }

        public static SnapraidContent Parse(Stream stream)
        {
            SnapraidContent result = new SnapraidContent();
            result.dataDisks = new List<DataDisk>();
            if (stream.ReadInt32LE() != SNAP)
            {
                throw new SnapraidContentException(SnapraidContentFailureReason.INVALID_MAGIC);
            }

            int version = stream.ReadInt32LE();
            switch (version)
            {
                case CNT1:
                case CNT2:
                case CNT3:
                    break;
                default:
                    throw new SnapraidContentException(SnapraidContentFailureReason.UNKNOWN_VERSION);
            }

            int actualN300 = stream.ReadInt32LE();

            while (true)
            {
                if (stream.Position == stream.Length)
                    break;

                char c = stream.ReadAsciiChar();
                switch (c)
                {
                    case 'z':
                        result.BlockSize = stream.sgetb32();
                        break;
                    case 'x':
                        result.MaximumBlockSize = stream.sgetb32();
                        break;
                    case 'c':
                        if (ReadHashInfo(stream, result))
                            return result;
                        break;
                    case 'M':
                    case 'm':
                        ReadDataDisk(stream, c, result);
                        break;
                    case 'P':
                        if (ReadParityDisk(stream, result))
                            return result;
                        break;
                    case 'f':
                        if (ReadFile(stream, result))
                            return result;
                        break;
                    case 'r':
                        ReadDirectory(stream, result);
                        break;
                    case 'h':
                        if (ReadHole(stream, result))
                            return result;
                        break;
                    case 'i':
                        if (ReadInfoBlock(stream, result))
                            return result;
                        break;
                    case 'N':
                        result.StoredCRC = stream.ReadInt32LE();
                        break;
                    default:
                        throw new NotImplementedException(new string(c, 1));
                }
            }
            return result;
        }

        private static bool ReadInfoBlock(Stream stream, SnapraidContent result)
        {
            uint vOldest = stream.sgetb32();
            uint vPos = 0;
            while (vPos < result.MaximumBlockSize)
            {
                uint t, flag, vCount;

                vCount = stream.sgetb32();
                if (vPos + vCount > result.MaximumBlockSize)
                {
                    result.CorruptionReason = snapcontentparser.CorruptionReason.InconsistentInfoSize;
                    result.IsCorrupted = true;
                    return true;
                }

                flag = stream.sgetb32();

                BlockInfo snapraidInfo;
                if ((flag & 1) != 0)
                {
                    t = stream.sgetb32();
                    bool bad = (flag & 2) != 0;
                    bool rehashed = (flag & 4) != 0;
                    bool justSynced = (flag & 8) != 0;

                    if (rehashed && result.HashAlgorithm == HashAlgorithm.Undefined)
                    {
                        result.CorruptionReason = snapcontentparser.CorruptionReason.InconsistentMissingPreviousChecksum;
                        result.IsCorrupted = true;
                        return true;
                    }

                    DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                    dtDateTime = dtDateTime.AddSeconds(t);
                    dtDateTime = dtDateTime.AddSeconds(vOldest);
                    snapraidInfo = new BlockInfo(bad, rehashed, justSynced, dtDateTime);
                }
                else
                {
                    snapraidInfo = null;
                }

                while (vCount > 0)
                {
                    if (result.Infos == null)
                        result.Infos = new BlockInfo[result.MaximumBlockSize];
                    result.Infos[vPos] = snapraidInfo;
                    ++vPos;
                    --vCount;
                }
            }

            return false;
        }

        private static bool ReadHole(Stream stream, SnapraidContent result)
        {
            char c;
            uint diskMapping = stream.sgetb32();
            DataDisk disk = result.dataDisks.Find(x => x.VPosition == diskMapping);

            uint vPos = 0;
            while (vPos < result.MaximumBlockSize)
            {
                uint vCount = stream.sgetb32();
                if (vPos + vCount > result.MaximumBlockSize)
                {
                    result.IsCorrupted = true;
                    result.CorruptionReason = snapcontentparser.CorruptionReason.InconsistentHoleSize;
                    return true;
                }

                c = stream.ReadAsciiChar();
                switch (c)
                {
                    case 'O':
                        vPos += vCount;
                        break;
                    default:
                        result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidHoleType;
                        result.IsCorrupted = true;
                        return true;
                }
            }

            return false;
        }

        private static void ReadDirectory(Stream stream, SnapraidContent result)
        {
            uint diskMapping = stream.sgetb32();
            DataDisk disk = result.dataDisks.Find(x => x.VPosition == diskMapping);
            string name = stream.ReadBinaryString(Int16.MaxValue);
            DirectoryEntry directoryEntry = new DirectoryEntry(disk, name);
            if (result.directories == null)
                result.directories = new List<DirectoryEntry>();
            result.directories.Add(directoryEntry);
        }

        private static bool ReadFile(Stream stream, SnapraidContent result)
        {
            uint mapping = stream.sgetb32();
            DataDisk disk = result.dataDisks.Find(x => x.VPosition == mapping);
            ulong size = stream.sgetb64();
            if (result.BlockSize == 0)
            {
                result.CorruptionReason = snapcontentparser.CorruptionReason.ZeroBlockSize;
                result.IsCorrupted = true;
                return true;
            }

            if (size / result.BlockSize > result.MaximumBlockSize)
            {
                result.CorruptionReason = snapcontentparser.CorruptionReason.FileTooBig;
                result.IsCorrupted = true;
                return true;
            }

            ulong mtime = stream.sgetb64();
            uint mtimeNsec = stream.sgetb32();
            if (mtimeNsec == 0)
                mtimeNsec = UInt32.MaxValue;
            else
                --mtimeNsec;

            ulong inode = stream.sgetb64();
            string filename = stream.ReadBinaryString(Int16.MaxValue);
            if (string.IsNullOrEmpty(filename))
            {
                result.CorruptionReason = snapcontentparser.CorruptionReason.NullFilename;
                result.IsCorrupted = true;
                return true;
            }

            if (result.files == null)
                result.files = new List<FileEntry>();

            FileEntry fileEntry = new FileEntry(disk, size, mtime, mtimeNsec, inode, filename, result.BlockSize);
            result.files.Add(fileEntry);
            ulong vIdx = 0;
            while (vIdx < fileEntry.NumBlocks)
            {
                char c = stream.ReadAsciiChar();
                uint vPos = stream.sgetb32();
                uint vCount = stream.sgetb32();
                if (vIdx + vCount > fileEntry.NumBlocks)
                {
                    result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidBlockNumber;
                    result.IsCorrupted = true;
                    return true;
                }

                if (vPos + vCount > result.MaximumBlockSize)
                {
                    result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidBlockSize;
                    result.IsCorrupted = true;
                    return true;
                }

                while (vCount > 0)
                {
                    BlockState? blockState;
                    byte[] hash;
                    switch (c)
                    {
                        case 'b':
                            blockState = BlockState.HashAndParityComputed;
                            break;
                        default:
                            result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidBlockType;
                            result.IsCorrupted = true;
                            return true;
                    }

                    if (c != 'n')
                        hash = stream.ReadFixedLength(BLOCK_HASH_SIZE);
                    else
                        hash = new byte[BLOCK_HASH_SIZE];

                    ++vIdx;
                    ++vPos;
                    --vCount;
                    fileEntry.Blocks[vIdx - 1] = new Block(blockState, hash);
                }
            }

            result.FileCount++;
            return false;
        }

        private static bool ReadParityDisk(Stream stream, SnapraidContent result)
        {
            if (result.parityDisks == null)
                result.parityDisks = new List<ParityDisk>();

            uint v_level = stream.sgetb32();
            uint v_totalBlocks = stream.sgetb32();
            uint v_freeBlocks = stream.sgetb32();
            string guidString = stream.ReadBinaryString();
            Guid guid = new Guid(guidString);
            if (v_level > LEV_MAX)
            {
                result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidParityLevel;
                result.IsCorrupted = true;
                return true;
            }

            ParityDisk parityDisk = new ParityDisk(v_level, v_totalBlocks, v_freeBlocks, guid);
            result.parityDisks.Add(parityDisk);
            return false;
        }

        private static void ReadDataDisk(Stream stream, char c, SnapraidContent result)
        {
            string name = stream.ReadBinaryString();
            uint vPos = stream.sgetb32();
            uint totalBlocks;
            uint freeBlocks;
            if (c == 'M')
            {
                totalBlocks = stream.sgetb32();
                freeBlocks = stream.sgetb32();
            }
            else
            {
                totalBlocks = 0;
                freeBlocks = 0;
            }

            string guidString = stream.ReadBinaryString();
            Guid guid = new Guid(guidString);
            DataDisk disk = new DataDisk(name, vPos, totalBlocks, freeBlocks, guid);
            result.dataDisks.Add(disk);
        }

        private static bool ReadHashInfo(Stream stream, SnapraidContent result)
        {
            char c;
            c = stream.ReadAsciiChar();
            switch (c)
            {
                case 'u':
                    result.HashAlgorithm = HashAlgorithm.Murmur3;
                    break;
                case 'k':
                    result.HashAlgorithm = HashAlgorithm.Spooky2;
                    break;
                default:
                    result.IsCorrupted = true;
                    result.CorruptionReason = snapcontentparser.CorruptionReason.InvalidChecksum;
                    return true;
            }

            result.HashSeed = new byte[HASH_MAX];
            if (stream.Read(result.HashSeed, 0, HASH_MAX) != HASH_MAX)
            {
                result.IsCorrupted = true;
                result.CorruptionReason = snapcontentparser.CorruptionReason.CantReadSeed;
                return true;
            }

            return false;
        }
    }

    
}
