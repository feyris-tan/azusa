using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = TagLib.File;

namespace moe.yo3explorer.azusa.BandcampImporter
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
