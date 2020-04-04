using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.Galleria;

namespace caputil
{
    public partial class Form1 : Form
    {
        private Galleria galleria;

        public Form1()
        {
            InitializeComponent();
            galleria = new Galleria();
            galleria.Dock = DockStyle.Fill;
            splitContainer1.Panel2.Controls.Add(galleria);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            folderBrowserDialog1.ShowNewFolderButton = false;
            if (folderBrowserDialog1.ShowDialog(this) != DialogResult.OK)
                this.Close();

            ScanDirectory(new DirectoryInfo(folderBrowserDialog1.SelectedPath));

            data.Sort((x, y) => x.Item1.CompareTo(y.Item1));
            foreach (Tuple<DateTime, CaputilGalleriaModel> tuple in data)
            {
                tuple.Item2.Sort();
                listBox1.Items.Add(new CaputilListViewItem(tuple));
            }

        }

        private void ScanFile(FileInfo fi)
        {
            string ext = fi.Extension.ToLowerInvariant();
            switch (ext)
            {
                case ".bmp":
                case ".png":
                case ".jpg":
                    break;
                default:
                    return;
            }

            //01234567890123456789
            //2017_0630_0548_36544
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fi.Name);
            if (fileNameWithoutExtension.Length != 20)
                return;

            char[] fileNameChars = fileNameWithoutExtension.ToCharArray();
            if (fileNameChars[4] != '_')
                return;
            if (fileNameChars[9] != '_')
                return;
            if (fileNameChars[14] != '_')
                return;
            string date = fileNameWithoutExtension.Substring(0,9);
            DateTime parsedDate = DateTime.ParseExact(date, "yyyy_MMdd", CultureInfo.InvariantCulture);

            if (data == null)
                data = new List<Tuple<DateTime, CaputilGalleriaModel>>();

            Tuple<DateTime, CaputilGalleriaModel> tuple = data.Find(x => x.Item1.Equals(parsedDate));
            if (tuple == null)
            {
                tuple = new Tuple<DateTime, CaputilGalleriaModel>(parsedDate, new CaputilGalleriaModel());
                data.Add(tuple);
            }

            tuple.Item2.FileInfos.Add(fi);
        }

        private void ScanDirectory(DirectoryInfo di)
        {
            foreach (DirectoryInfo directoryInfo in di.GetDirectories())
                ScanDirectory(directoryInfo);
            foreach (FileInfo fileInfo in di.GetFiles())
                ScanFile(fileInfo);
        }

        private List<Tuple<DateTime, CaputilGalleriaModel>> data;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaputilListViewItem lvi = listBox1.SelectedItem as CaputilListViewItem;
            galleria.GalleriaModel = lvi.GetModel();
            galleria.UpdateControls();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            foreach (Tuple<DateTime, CaputilGalleriaModel> tuple in data) 
                tuple.Item2.Dispose();
        }
    }
}
