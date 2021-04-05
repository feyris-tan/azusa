using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Control;
using moe.yo3explorer.sharpBluRay;
using moe.yo3explorer.sharpBluRay.FilesystemAbstraction;
using moe.yo3explorer.sharpBluRay.Model;

namespace moe.yo3explorer.azusa.Utilities.BDInfo
{
    public partial class BdInfoControl : UserControl, ISidecarDisplayControl
    {
        public BdInfoControl()
        {
            InitializeComponent();
        }

        public Guid DisplayControlUuid => new Guid("{56070D17-607D-469D-80AA-D131C6221143}");

        private byte[] _data;
        private BluRayDiscMovie bdmv;

        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnDataChanged?.Invoke(_data, isComplete(), MediumId);
                if (isComplete())
                {
                    IDirectoryAbstraction bdMeta = MetadataArchiveManager.Deserialize(_data);
                    bdmv = new BluRayDiscMovie(bdMeta);
                    propertyGrid1.SelectedObject = bdmv;
                }
            }
        }

        public bool isComplete()
        {
            if (_data != null)
            {
                if (_data.Length > 8)
                {
                    bool result =  BitConverter.ToUInt64(_data, 0) == 5279417649501403713;
                    return result;
                }
            }
            return false;
        }

        public int MediumId { get; set; }
        public System.Windows.Forms.Control ToControl()
        {
            return this;
        }

        public void ForceEnabled()
        {
            propertyGrid1.Focus();
        }

        public event SidecarChange OnDataChanged;

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(Cursor.Position);
        }

        private void propertyGrid1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void propertyGrid1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                object j = e.Data.GetData(DataFormats.FileDrop);
                string[] jStrings = (string[])j;
                Data = File.ReadAllBytes(jStrings[0]);
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bdmv == null)
            {
                MessageBox.Show("Keine BDMV Metadaten geladen!");
                return;
            }

            if (bdmv.Playlists == null)
            {
                MessageBox.Show("BDMV Applikation enthält keine Videoclips!");
                return;
            }

            Playlist mainMoviePlaylist = bdmv.Playlists.First(x => x.Marks.Length > 2);
            if (mainMoviePlaylist == null)
            {
                MessageBox.Show("Konnte Hauptfilm nicht erkennen!");
                return;
            }

            TimeSpan[] timestamps = Array.ConvertAll(mainMoviePlaylist.Marks, x => x.DotNetTimeStamp);
            string[] output = new string[timestamps.Length * 2];
            Array.Sort(timestamps);
            int numChapters = Math.Min(timestamps.Length, 98);
            for (int i = 0; i < numChapters; i++)
            {
                output[(i * 2) + 0] = String.Format("CHAPTER{0:00}={1:00}:{2:00}:{3:00}.{4:000}", i + 1, timestamps[i].Hours, timestamps[i].Minutes, timestamps[i].Seconds, timestamps[i].Milliseconds);
                output[(i * 2) + 1] = String.Format("CHAPTER{0:00}NAME=Chapter {0}", i + 1);
            }

            DialogResult dialogResult = saveTxt.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
                return;

            File.WriteAllLines(saveTxt.FileName, output);
            MessageBox.Show("OK!");
        }
    }
}
