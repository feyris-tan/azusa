using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using moe.yo3explorer.azusa.OfflineReaders.VocaDB.Control;
using moe.yo3explorer.azusa.OfflineReaders.VocaDB.Entity;
using moe.yo3explorer.azusa.Properties;

namespace moe.yo3explorer.azusa.OfflineReaders.VocaDB.Boundary
{
    public partial class VocaDbOffline : UserControl, IAzusaModule
    {
        public VocaDbOffline()
        {
            InitializeComponent();
        }

        public string IniKey => "vocadb";
        public string Title => "VocaDB Offline";
        private AzusaContext context;

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            context = AzusaContext.GetInstance();
            imageList1.Images.Add(Resources.media_optical);
            imageList1.Images.Add(Resources.internet_web_browser);
        }

        public int Priority => 9;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            List<string> searchStrings = context.DatabaseDriver.VocaDb_FindAlbumNamesBySongNames(toolStripTextBox1.Text);
            searchStrings.Add(toolStripTextBox1.Text);

            foreach (string word in searchStrings.Distinct())
            {
                IEnumerable<VocadbSearchResult> results = context.DatabaseDriver.VocaDb_Search(word);
                foreach (VocadbSearchResult searchResult in results)
                {
                    VocaDbSearchResultListViewItem item = new VocaDbSearchResultListViewItem(searchResult);
                    listView1.Items.Add(item);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pictureBox1.BackgroundImage != null)
            {
                pictureBox1.BackgroundImage.Dispose();
                pictureBox1.BackgroundImage = null;
            }

            metadataTextbox.Text = "";

            if (listView1.SelectedItems.Count == 0)
                return;

            VocaDbSearchResultListViewItem listViewItem = listView1.SelectedItems[0] as VocaDbSearchResultListViewItem;
            Image cover = context.DatabaseDriver.Vocadb_GetAlbumCover(listViewItem.wrapped.Id);
            if (cover != null)
            {
                pictureBox1.BackgroundImage = cover;
                pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            }

            IEnumerable<VocadbTrackEntry> tracks = context.DatabaseDriver.VocaDb_GetTracksByAlbum(listViewItem.wrapped.Id);
            StringWriter sw = new StringWriter();
            foreach (VocadbTrackEntry trackEntry in tracks)
            {
                sw.WriteLine("{0}.{1} {2}",trackEntry.DiscNumber,trackEntry.TrackNumber,trackEntry.Name);
            }

            metadataTextbox.Text = sw.ToString();
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                toolStripButton1_Click(sender, e);
            }
        }
    }
}
