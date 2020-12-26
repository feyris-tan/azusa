using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscUtils.Iso9660;
using DiscUtils.Streams;
using moe.yo3explorer.azusa.Utilities.FolderMapper.Control;

namespace moe.yo3explorer.azusa.Utilities.Ps1BatchImport
{
    class PlaystationSkuDetector
    {
        private static readonly byte[] SYNC_BYTES = new byte[] { 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00};

        public static bool isBinFile(FileInfo isofile)
        {
            FileStream fileStream = isofile.OpenRead();
            byte[] comparator = new byte[SYNC_BYTES.Length];
            fileStream.Read(comparator, 0, comparator.Length);
            bool minuteValid = fileStream.ReadByte() <= 0x80;
            bool secondValid = fileStream.ReadByte() <= 0x80;
            bool frameValid = fileStream.ReadByte() <= 0x74;
            bool modeValid = fileStream.ReadByte() <= 0x02;
            fileStream.Close();
            if (!minuteValid || !secondValid || !frameValid || !modeValid)
                return false;
            for (int i = 0; i < comparator.Length; i++)
            {
                if (SYNC_BYTES[i] != comparator[i])
                    return false;
            }
            return true;
        }

        public static string DetectPs2Sku(FileInfo isofile)
        {
            Stream isoStream;
            if (isBinFile(isofile))
            {
                isoStream = new RawCdRomStream(isofile);
            }
            else
            {
                isoStream = isofile.OpenRead();
            }

            if (!CDReader.Detect(isoStream))
                return null;
            CDReader cdReader = new CDReader(isoStream, false, true);
            if (!cdReader.FileExists("SYSTEM.CNF"))
            {
                cdReader.Dispose();
                isoStream.Dispose();
                return null;
            }

            SparseStream systemCnfStream = cdReader.OpenFile("SYSTEM.CNF", FileMode.Open, FileAccess.Read);
            StreamReader systemCnfReader = new StreamReader(systemCnfStream, Encoding.ASCII);
            string boot2 = null;
            while (!systemCnfReader.EndOfStream)
            {
                boot2 = systemCnfReader.ReadLine();
                if (boot2.ToUpperInvariant().StartsWith("BOOT"))
                {
                    break;
                }
            }
            systemCnfReader.Dispose();
            systemCnfStream.Dispose();

            while (boot2.Contains("\\"))
                boot2 = boot2.Substring(1);

            if (boot2.EndsWith(";1"))
                boot2 = boot2.Substring(0, boot2.Length - 2);

            cdReader.Dispose();
            isoStream.Dispose();
            return boot2;
        }
    }
}
