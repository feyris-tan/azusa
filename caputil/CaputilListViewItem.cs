using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace caputil
{
    class CaputilListViewItem : ListViewItem
    {
        public CaputilListViewItem(Tuple<DateTime, CaputilGalleriaModel> backed)
        {
            this.backed = backed;
            this.Text = backed.Item1.ToShortDateString();
        }

        private Tuple<DateTime, CaputilGalleriaModel> backed;

        public CaputilGalleriaModel GetModel()
        {
            return backed.Item2;
        }
    }
}
