using System.IO;
using File = TagLib.File;

namespace moe.yo3explorer.azusa.Utilities.BandcampImporter
{
    class ArbitraryStreamFileAbstraction : File.IFileAbstraction
    {
        public ArbitraryStreamFileAbstraction(Stream s,string name)
        {
            backing = s;
            this.name = name;
        }

        private Stream backing;
        private string name;

        public void CloseStream(Stream stream)
        {
            stream.Close();
        }

        public string Name => name;
        public Stream ReadStream => backing;
        public Stream WriteStream => backing;
    }
}
