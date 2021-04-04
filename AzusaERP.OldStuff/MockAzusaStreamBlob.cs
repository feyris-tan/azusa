using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.OldStuff
{
    class AzusaStreamBlob
    {
        private DirectoryInfo di;
        private bool v;

        public AzusaStreamBlob(DirectoryInfo di, bool v)
        {
            this.di = di;
            this.v = v;
        }

        public bool TestFor(int i, int i1, int postId)
        {
            throw new NotImplementedException();
        }

        public byte[] Get(int i, int i1, int i2)
        {
            throw new NotImplementedException();
        }
    }
}
