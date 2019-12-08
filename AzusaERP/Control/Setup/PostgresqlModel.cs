using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.Control.Setup
{
    class PostgresqlModel
    {
        public string server { get; set; }
        public ushort port { get; set; }
        public string database { get; set; }
        public string password { get; set; }
        public string username { get; set; }
    }
}
