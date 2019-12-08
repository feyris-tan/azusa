using System.Diagnostics;
using System.IO;
using System.Text;

namespace libazusax360
{
    public class FSOffsetSearcher
    {
        // Fields
        private long InputFileSize;
        private Stream InStream;

        // Methods
        public FSOffsetSearcher(Stream InStream, long InputFileSize)
        {
            this.InStream = InStream;
            this.InputFileSize = InputFileSize;
        }

        public long FindFSOffset()
        {
            Trace.WriteLine("Searching for filesystem offset");
            bool flag = false;
            ISOPartitionDetails details = new ISOPartitionDetails();
            if (this.InputFileSize > 0xfd90000L)
            {
                Trace.WriteLine("Attempting to load XBOX 360 PFI file details");
                this.InStream.Seek(0xfd8e800L, SeekOrigin.Begin);
                byte[] buffer = new byte[0x800];
                byte[] buffer2 = new byte[0x800];
                this.InStream.Read(buffer2, 0, 0x800);
                this.InStream.Seek(0x800L, SeekOrigin.Current);
                this.InStream.Read(buffer, 0, 0x800);
                Trace.WriteLine("Checking PFI details...");
                if (FileSystemSource.SimpleCheckSS(buffer) && FileSystemSource.CheckPFI(buffer2))
                {
                    Trace.WriteLine("SS and PFI are both trivially correct");
                    Stream vs = new ISOFileStreamer(0L, 0xfd8e800L, new BinaryReader(this.InStream));
                    details = FileSystemSource.GetPartitionDetails(buffer, buffer2, Format.Xbox360, FileSystemSource.GetVideoSize(vs));
                    vs.Close();
                    Trace.WriteLine("FileSystemOffset calculated as being at " + details.VideoFileSize.ToString() + " blocks");
                    flag = true;
                }
                else
                {
                    Trace.WriteLine("PFI or SS is invalid or not present, will use default value");
                }
                long num = 0xfd90000L;
                if (flag)
                {
                    num = details.VideoPartitionSize * 0x800L;
                }
                Trace.WriteLine("Now checking FS Magic");
                if (this.InputFileSize > ((num + 0x10000L) + 20L))
                {
                    this.InStream.Seek(num + 0x10000L, SeekOrigin.Begin);
                    byte[] buffer3 = new byte[20];
                    this.InStream.Read(buffer3, 0, 20);
                    if (Encoding.ASCII.GetString(buffer3, 0, 20) == "MICROSOFT*XBOX*MEDIA")
                    {
                        Trace.WriteLine("FS Magic found at " + num.ToString() + "blocks + 0x14");
                        return num;
                    }
                    Trace.WriteLine("XBOX360 Partition loading failed, trying XBOX 1 partition");
                    if (this.InputFileSize > 0x18300000L)
                    {
                        this.InStream.Seek(0x182fe800L, SeekOrigin.Begin);
                        buffer = new byte[0x800];
                        buffer2 = new byte[0x800];
                        this.InStream.Read(buffer2, 0, 0x800);
                        this.InStream.Seek(0x800L, SeekOrigin.Current);
                        this.InStream.Read(buffer, 0, 0x800);
                        Trace.WriteLine("Attempting to read details from Xbox 1 pfi + ss");
                        if (FileSystemSource.CheckXbox1SS(buffer) && FileSystemSource.CheckPFI(buffer2))
                        {
                            Trace.WriteLine("SS and PFI loaded successfully");
                            Stream stream2 = new ISOFileStreamer(0L, 0x182fe800L, new BinaryReader(this.InStream));
                            details = FileSystemSource.GetPartitionDetails(buffer, buffer2, Format.Xbox1, FileSystemSource.GetVideoSize(stream2));
                            stream2.Close();
                            Trace.WriteLine("FileSystemOffset calculated as being at " + details.VideoFileSize.ToString() + " blocks");
                            flag = true;
                        }
                        else
                        {
                            Trace.WriteLine("PFI or SS are invalid or not present, trying default xbox 1 offset");
                            flag = false;
                        }
                        num = 0x18300000L;
                        if (flag)
                        {
                            num = details.VideoPartitionSize * 0x800L;
                        }
                        if (this.InputFileSize > ((num + 0x10000L) + 20L))
                        {
                            this.InStream.Seek(num + 0x10000L, SeekOrigin.Begin);
                            this.InStream.Read(buffer3, 0, 20);
                            if (Encoding.ASCII.GetString(buffer3, 0, 20) == "MICROSOFT*XBOX*MEDIA")
                            {
                                Trace.WriteLine("FS Magic found at " + num.ToString() + "blocks + 0x14");
                                return num;
                            }
                        }
                    }
                }
            }
            else
            {
                Trace.WriteLine("File is too short to contain a video partition (" + this.InputFileSize.ToString() + ")");
            }
            Trace.WriteLine("FS offset not found, using byte 0");
            return 0L;
        }
    }
}