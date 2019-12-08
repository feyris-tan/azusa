using System.IO;

namespace libazusax360
{
    public class FSFile : InputFile
    {
        // Fields
        private string filename;
        private long filesize;
        private FileSystemSource FileSource;

        // Methods
        public FSFile(string filename)
        {
            this.InitFSFile(filename);
        }

        public override byte GetAttributes()
        {
            return 0x80;
        }

        public override long GetFileLength()
        {
            return this.filesize;
        }

        public override string GetFileName()
        {
            return Path.GetFileName(this.filename);
        }
        
        public override Stream GetStream()
        {
            return new FileStream(this.filename, FileMode.Open);
        }

        public void InitFSFile(string Filename)
        {
            this.filename = Filename;
            FileInfo info = new FileInfo(Filename);
            this.filesize = info.Length;
        }

        public override bool IsFolder()
        {
            return false;
        }
    }
}