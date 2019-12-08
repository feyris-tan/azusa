using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vndbDumper
{
    class Tag
    {
        public int id;
        public string name, description;
        public bool meta;
        public int vns;
        public string cat;
        public string[] aliases;
        public int[] parents;
    }
}
