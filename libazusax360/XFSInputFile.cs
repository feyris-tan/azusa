using System;
using System.IO;
using System.Text;

namespace libazusax360
{
    public class XFSInputFile : InputFile
    {
        // Fields
        private byte attributes;
        private long filelength;
        private string filename;
        private XboxISOFileSource SourceISO;

        // Methods
        public XFSInputFile(XboxISOFileSource xbdvd, int SizeOfRoot, int StartBlock)
        {
            this.SourceISO = xbdvd;
            this.attributes = 0x10;
            this.filename = "XDFS Root";
            this.filelength = SizeOfRoot;
            this.StartSector = StartBlock;
        }

        public XFSInputFile(XboxISOFileSource xbdvd, byte[] buffer, int fsOffset)
        {
            this.SourceISO = xbdvd;
            this.StartSector = BitConverter.ToInt32(buffer, fsOffset + 4);
            this.filelength = BitConverter.ToUInt32(buffer, fsOffset + 8);
            byte count = buffer[fsOffset + 13];
            this.filename = Encoding.ASCII.GetString(buffer, fsOffset + 14, count);
            this.attributes = buffer[fsOffset + 12];
        }
        
        public override byte GetAttributes()
        {
            return this.attributes;
        }

        public override long GetFileLength()
        {
            return this.filelength;
        }

        public override string GetFileName()
        {
            return this.filename;
        }
        
        public override Stream GetStream()
        {
            return new ISOFileStreamer(this, this.SourceISO.GetFileSystemOffset(), new BinaryReader(this.SourceISO.GetStream()));
        }

        public override bool IsFolder()
        {
            int is_dir = this.GetAttributes() & 0x10;
            if (is_dir == 0)
            {
                return false;
            }
            return true;
        }

        // Properties
        public int StartSector { get; }
    }
}