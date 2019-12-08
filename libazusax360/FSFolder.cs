using System;
using System.IO;

namespace libazusax360
{
    public class FSFolder : InputFile
    {
        // Fields
        private string foldername;

        // Methods
        public FSFolder(string foldername)
        {
            this.foldername = foldername;
        }

        public override byte GetAttributes()
        {
            return 0x10;
        }

        public override long GetFileLength()
        {
            return 0L;
        }

        public override string GetFileName()
        {
            return this.foldername;
        }
        
        public override Stream GetStream()
        {
            throw new NotSupportedException();
        }

        public override bool IsFolder()
        {
            return true;
        }
    }
}