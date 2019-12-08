using System.IO;

namespace libazusax360
{
    public abstract class InputFile
    {
        public abstract byte GetAttributes();

        public abstract long GetFileLength();

        public abstract string GetFileName();
        
        public abstract Stream GetStream();

        public abstract bool IsFolder();
    }
}