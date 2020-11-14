using System;
using System.IO;
using System.Text;

namespace warcsplit
{
    class Program
    {
        private const long MAX_WARC_SIZE = 600000000;
        private const string SUPPORTED_CDX_HEADER = " CDX a b m s k S V g u";

        static void Main(string[] args)
        {
            FileInfo fi = new FileInfo(args[0]);
            new Program().Run(fi);
        }

        public void Run(FileInfo inCdxFileInfo)
        {
            DirectoryInfo inCdxDirectory = inCdxFileInfo.Directory;
            string prefix = Path.GetFileNameWithoutExtension(inCdxFileInfo.Name);
            FileStream fileStream = inCdxFileInfo.OpenRead();
            StreamReader inCdx = new StreamReader(fileStream, Encoding.UTF8);
            string magic = inCdx.ReadLine();

            if (!magic.Equals(SUPPORTED_CDX_HEADER))
            {
                Console.WriteLine("can't support this cdx!");
                return;
            }


            FileInfo currentInWarcInfo = new FileInfo("not_a_warc.bin");
            Stream currentInWarcStream = null;
            int currentOutWarcNumber = 0;
            long currentOutWarcRemain = 0;
            FileInfo currentOutWarcInfo = null;
            Stream currentOutWarcStream = Stream.Null;
            FileInfo outCdxInfo = new FileInfo(prefix + ".cdx");
            Stream outCdxStream = outCdxInfo.OpenWrite();
            StreamWriter outCdxWriter = new StreamWriter(outCdxStream, Encoding.UTF8);
            outCdxWriter.WriteLine(SUPPORTED_CDX_HEADER);

            while (!inCdx.EndOfStream)
            {
                string[] args = inCdx.ReadLine().Split(' ');
                int entrySize = Convert.ToInt32(args[5]);
                long offset = Convert.ToInt64(args[6]);
                string requestedWarcName = args[7];
                FileInfo requestedWarc = new FileInfo(Path.Combine(inCdxDirectory.FullName, requestedWarcName));
                if (!requestedWarc.FullName.Equals(currentInWarcInfo.FullName))
                {
                    currentInWarcInfo = requestedWarc;
                    currentInWarcStream = currentInWarcInfo.OpenRead();
                }

                long warcSizeRequired = entrySize + (offset - currentInWarcStream.Position);
                if (warcSizeRequired > currentOutWarcRemain)
                {
                    currentOutWarcStream.Flush();
                    currentOutWarcStream.Close();

                    currentOutWarcNumber++;
                    currentOutWarcRemain = MAX_WARC_SIZE;
                    currentOutWarcInfo = new FileInfo(string.Format("{0}-{1:00000}.warc.gz", prefix, currentOutWarcNumber));
                    currentOutWarcStream = currentOutWarcInfo.OpenWrite();
                    outCdxWriter.Flush();
                    Console.WriteLine("");
                    Console.Write("Writing {0}", currentOutWarcInfo.Name);
                }

                int bufferLength = (int) (offset - currentInWarcStream.Position);
                byte[] buffer = new byte[bufferLength];
                currentInWarcStream.Read(buffer, 0, bufferLength);
                currentOutWarcStream.Write(buffer, 0, bufferLength);
                currentOutWarcRemain -= bufferLength;

                args[6] = currentOutWarcStream.Position.ToString();
                args[7] = currentOutWarcInfo.Name;
                string writeMe = String.Join(" ", args);
                outCdxWriter.WriteLine(writeMe);

                bufferLength = entrySize;
                buffer = new byte[bufferLength];
                currentInWarcStream.Read(buffer, 0, bufferLength);
                currentOutWarcStream.Write(buffer, 0, bufferLength);
                currentOutWarcRemain -= bufferLength;
                Console.Write(".");
            }
            outCdxWriter.Flush();
            outCdxWriter.Close();
            currentOutWarcStream.Flush();
            currentOutWarcStream.Close();
        }
    }
}
