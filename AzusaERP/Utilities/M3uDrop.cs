using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.Utilities
{
    public partial class M3uDrop : Form
    {
        public M3uDrop()
        {
            InitializeComponent();
        }

        private void label1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            DirectoryInfo di = new DirectoryInfo(files[0]);
            MakeM3uFromDirectory.MakeM3u(di);
        }

        private void M3uDrop_DragEnter(object sender, DragEventArgs e)
        {
            bool result = e.Data.GetDataPresent(DataFormats.FileDrop);
            if (result)
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files.Length == 1)
                {
                    DirectoryInfo di = new DirectoryInfo(files[0]);
                    if (di.Exists)
                    {
                        e.Effect = DragDropEffects.Link;
                    }
                }
            }
        }
    }

    public class M3uDropPlugin : AzusaPlugin
    {
        public override bool IsExecutable => true;
        public override string DisplayName => "m3udrop";

        public override void Execute()
        {
            new M3uDrop().ShowDialog();
        }
    }
}
