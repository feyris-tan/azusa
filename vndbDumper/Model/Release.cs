using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vndbDumper.Model
{
    class Release
    {
        public int id;
        public string title;
        public string original;
        public string released;
        public string type;
        public bool patch;
        public bool freeware;
        public bool doujin;
        public string[] languages;
        public string website;
        public string notes;
        public int? minage;
        public string gtin;
        public string catalog;
        public string[] platforms;
        public ReleaseMedia[] media;
        public string resolution;
        public int? voiced;
        public int?[] animation;
        public ReleaseVN[] vn;
        public ReleaseProducer[] producers;
    }
}
