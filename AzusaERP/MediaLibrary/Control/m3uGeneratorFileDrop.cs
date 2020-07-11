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

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class m3uGeneratorFileDrop : Form
    {
        public m3uGeneratorFileDrop()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            foreach (string jString in jStrings)
            {
                FileInfo fi = new FileInfo(jString);
                if (!fi.Exists)
                    continue;

                listBox1.Items.Add(fi);
                listBox1.Sorted = true;
            }
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            FileStream fileStream = File.OpenWrite(saveFileDialog1.FileName);
            StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8);

            foreach (object listBox1Item in listBox1.Items)
            {
                FileInfo fi = (FileInfo) listBox1Item;
                sw.WriteLine(fi.Name);
            }
            sw.Flush();
            sw.Close();
            DialogResult = DialogResult.OK;
            this.FileName = saveFileDialog1.FileName;
            this.Close();
        }

        public string FileName { get; private set; }
    }
}
