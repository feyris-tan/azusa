using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.Utilities
{
    public class MakeM3uFromDirectory : AzusaPlugin
    {
        public override bool IsExecutable => true;

        public override string DisplayName => "m3u8 Datei für einen Ordner erstellen";

        public override void Execute()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            MainForm mainForm = this.GetContext().MainForm;
            if (fbd.ShowDialog(mainForm) != DialogResult.OK)
                return;

            DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
            FileInfo[] fi = di.GetFiles();
            Array.Sort(fi, new FilenameComparer());

            List<string> filenames = new List<string>();
            foreach (FileInfo subfile in fi)
            {
                string extension = subfile.Extension.ToLowerInvariant();
                switch (extension)
                {
                    case ".mkv":
                        filenames.Add(subfile.Name);
                        break;
                    default:
                        MessageBox.Show(String.Format("{0} ist nicht bekannt.",subfile.Extension));
                        break;
                }
            }

            string m3u8Path = Path.Combine(di.FullName, di.Name + ".m3u8");
            File.WriteAllLines(m3u8Path, filenames.ToArray());
            MessageBox.Show("Erledigt!");
        }
    }

    class FilenameComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
