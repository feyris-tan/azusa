using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.Galleria;
using moe.yo3explorer.azusa.OfflineReaders.VgmDb.Entity;

namespace moe.yo3explorer.azusa.OfflineReaders.VgmDb.Boundary
{
    public partial class VgmDbControl : UserControl, IAzusaModule
    {
        public VgmDbControl()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
        }

        private AzusaContext context;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            IDatabaseDriver database = context.DatabaseDriver;
            string startswith = toolStripTextBox1.Text + "%";
            string escaped = toolStripTextBox1.Text;
            escaped = "%" + escaped + "%";

            HashSet<int> albumIdHashSet = new HashSet<int>();

            int[] possibleArtists = database.Vgmdb_FindArtistIdsByName(escaped).ToArray();
            if (possibleArtists.Length == 1)
            {
                albumIdHashSet.AddAll(database.Vgmdb_FindAlbumIdsByArtistId(possibleArtists[0]));
            }
            albumIdHashSet.AddAll(database.Vgmdb_FindAlbumsBySkuPart(startswith));
            albumIdHashSet.AddAll(database.Vgmdb_FindAlbumsByAlbumTitle(escaped));
            albumIdHashSet.AddAll(database.Vgmdb_FindAlbumsByArbituraryProducts(escaped));
            albumIdHashSet.AddAll(database.Vgmdb_FindAlbumsByTrackMask(escaped));

            int[] albumIds = albumIdHashSet.ToArray();
            AlbumListEntry[] entries = Array.ConvertAll(albumIds, x => database.Vgmdb_FindAlbumForList(x));
            
            dataGridView1.DataSource = entries;
            dataGridView1.ReadOnly = true;
        }

        public string IniKey => "vgmdb";
        public string Title => "VGMDB-Offline";

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
        }

        public int Priority => 6;

        private int lastRowSelected;

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            IDatabaseDriver database = context.DatabaseDriver;
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            AlbumListEntry entry = (AlbumListEntry)row.DataBoundItem;
            if (entry == null)
                return;

            if (lastRowSelected == entry.id)
                return;
            lastRowSelected = entry.id;

            pictureBox1.BackgroundImage = database.Vgmdb_GetAlbumCover(entry.id);

            /*
             *  [x] postgres.public.dump_vgmdb_album_arbituaryproducts
                [x] postgres.public.dump_vgmdb_album_artist_arbitrary
                [x] postgres.public.dump_vgmdb_album_artist_type
                [x] postgres.public.dump_vgmdb_album_artists
                [x] postgres.public.dump_vgmdb_album_classification
                [x] postgres.public.dump_vgmdb_album_cover
                [x] postgres.public.dump_vgmdb_album_disc_track_translation
                [x] postgres.public.dump_vgmdb_album_disc_tracks
                [x] postgres.public.dump_vgmdb_album_discs
                [x] postgres.public.dump_vgmdb_album_label_arbiturary
                [x] postgres.public.dump_vgmdb_album_label_roles
                [x] postgres.public.dump_vgmdb_album_labels
                [x] postgres.public.dump_vgmdb_album_mediaformat
                [x] postgres.public.dump_vgmdb_album_relatedalbum
                [x] postgres.public.dump_vgmdb_album_releaseevent
                [x] postgres.public.dump_vgmdb_album_reprints
                [x] postgres.public.dump_vgmdb_album_titles
                [x] postgres.public.dump_vgmdb_album_types
                [x] postgres.public.dump_vgmdb_album_websites
                [ ] postgres.public.dump_vgmdb_albums
             */
            Album model = new Album();
            model.Products = database.Vgmdb_FindProductNamesByAlbumId(entry.id).ToArray();
            model.Artists = database.Vgmdb_FindArtistNamesByAlbumId(entry.id).ToArray();
            model.Classification = entry.classificationName;
            model.RelatedAlbums = database.Vgmdb_FindRelatedAlbums(entry.id).ToArray();
            model.ReleaseEvent = database.Vgmdb_GetReleaseEvent(entry.id);
            model.Reprints = database.Vgmdb_FindReprints(entry.id).ToArray();
            model.TypeName = entry.typeName;
            model.Websites = database.Vgmdb_GetWebsites(entry.id).ToArray();
            if (entry.date.HasValue)
                model.Date = entry.date.Value.ToShortDateString();
            else
                model.Date = "???";
            model.Publisher = entry.publisher;
            
            DefaultGalleriaModel galleriaModel = new DefaultGalleriaModel();
            galleria1.GalleriaModel = galleriaModel;
            galleriaModel.AddRange(database.FindCoversByAlbumId(entry.id));
            galleria1.GalleriaModel = galleriaModel;
            galleria1.Visible = galleriaModel.Count > 0;

            Tuple<string, int, int, string, int>[] trackTuples = database.Vgmdb_FindTrackDataByAlbum(entry.id).ToArray();
            model.Tracks = Array.ConvertAll(trackTuples, x => new Track(x));

            model.Labels = database.Vgmdb_FindLabelNamesByAlbumId(entry.id).ToArray();
            model.MediaFormat = entry.mediaformatName;

            notesTextBox.Text = entry.notes;

            propertyGrid1.SelectedObject = model;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
                return;

            dataGridView1.SelectedCells[0].OwningRow.Selected = true;
            dataGridView1_RowEnter(sender, e);
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
