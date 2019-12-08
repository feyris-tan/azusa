using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class UpdateList
    {
        public string link;
        public ProductMeta meta;
        public string section;
        public string[] sections;
        public UpdateListEntry[] updates;
        public string vgmdb_link;
    }
}
