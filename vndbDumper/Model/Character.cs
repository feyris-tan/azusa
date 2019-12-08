using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vndbDumper.Model
{
    class Character
    {
        public int id;
        public string name;
        public string original;
        public string gender;
        public string bloodt;
        public int?[] birthday;
        public string aliases;
        public string description;
        public string image;
        public int? bust;
        public int? waist;
        public int? hip;
        public int? height;
        public int? weight;
        public int[][] traits;
        public object[][] vns;
        public CharacterVoiced[] voiced;
        public CharacterInstance[] instances;
    }
}
