using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Utilities.Ps1BatchImport
{

    class GameDisc
    {
        public string Name { get; set; }
        public FileInfo BinFile { get; set; }
        public FileInfo CueFile { get; set; }
        public FileInfo Md5File { get; set; }
        public string CueContent { get; set; }
        public string Md5Content { get; set; }
        public string IbgContent { get; set; }

        public string GuessSku()
        {
            string bootfile = PlaystationSkuDetector.DetectPs2Sku(BinFile);
            if (string.IsNullOrEmpty(bootfile))
            {
                Console.WriteLine("Can't autodetect SKU of " + BinFile.Name);
                return "";
            }

            bootfile = bootfile.Replace("_", "-");
            bootfile = bootfile.Replace(".", "");
            return bootfile;
        }
    }

}
