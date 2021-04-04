using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AzusaERP;
using AzusaERP.OldStuff;
using BrightIdeasSoftware;
using moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity;

namespace moe.yo3explorer.azusa.OfflineReaders.VnDb.Boundary
{
    public partial class VnDbControl : UserControl, IAzusaModule
    {
        /*
         *
         * postgres.public.dump_vndb_character
           postgres.public.dump_vndb_character_instances
           postgres.public.dump_vndb_character_traits
           postgres.public.dump_vndb_character_vns
           postgres.public.dump_vndb_character_voiced
           [x] postgres.public.dump_vndb_release
           [x] postgres.public.dump_vndb_release_languages
           [x] postgres.public.dump_vndb_release_media
           [x] postgres.public.dump_vndb_release_platforms
           [x] postgres.public.dump_vndb_release_producers
           [x] postgres.public.dump_vndb_release_vns
           postgres.public.dump_vndb_tags
           postgres.public.dump_vndb_tags_aliases
           postgres.public.dump_vndb_tags_parents
           postgres.public.dump_vndb_traits
           postgres.public.dump_vndb_traits_aliases
           postgres.public.dump_vndb_traits_parents
           postgres.public.dump_vndb_vn
           postgres.public.dump_vndb_vn_anime
           postgres.public.dump_vndb_vn_languages
           postgres.public.dump_vndb_vn_platforms
           postgres.public.dump_vndb_vn_relation
           postgres.public.dump_vndb_vn_screens
           postgres.public.dump_vndb_vn_staff
           postgres.public.dump_vndb_vn_tag
           postgres.public.dump_vndb_votes 
         */
        private AzusaContext context;
        private IDatabaseDriver database;

        public VnDbControl()
        {
            InitializeComponent();
        }

        public string IniKey => "vndb";
        public string Title => "VNDB-Offline";
        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            context = AzusaContext.GetInstance();
            database = context.DatabaseDriver;

            foreach (MediaType mediaType in database.GetMediaTypes())
            {
                if (!string.IsNullOrEmpty(mediaType.VnDbKey))
                {
                    imageList1.Images.Add(mediaType.VnDbKey, Image.FromStream(new MemoryStream(mediaType.Icon)));
                }
            }
        }

        public int Priority => 7;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            treeListView1.SetObjects(null);

            string searchquery = String.Format("%{0}%",toolStripTextBox1.Text);
            IEnumerable<VndbSearchResult> search = database.Vndb_Search(searchquery);
            List<VndbSearchResult> searchResults = search.ToList();
            foreach (VndbSearchResult searchResult in searchResults)
            {
                IEnumerable<VndbVnResult> vnSearch = database.Vndb_GetVnsByRelease(searchResult.RID);
                searchResult.Vns = vnSearch.ToArray();
            }
            treeListView1.SetObjects(searchResults);

            olvColumn1.ImageGetter = new ImageGetterDelegate(MainListImageGetter);
            treeListView1.CanExpandGetter = new TreeListView.CanExpandGetterDelegate(CanExpandGetter);
            treeListView1.ChildrenGetter = new TreeListView.ChildrenGetterDelegate(ChildrenGetter);
            treeListView1.SelectionChanged += TreeListView1_SelectionChanged;
        }

        private void TreeListView1_SelectionChanged(object sender, EventArgs e)
        {
            galleria1.GalleriaModel = new EmptyGalleriaModel();
            propertyGrid1.SelectedObject = null;
            notesTextBox.Text = "";
            pictureBox1.BackgroundImage = null;

            var selectedObject = treeListView1.SelectedObject;
            if (selectedObject == null)
                return;

            VndbSearchResult releaseResult = selectedObject as VndbSearchResult;
            if (releaseResult != null)
            {
                VndbRelease release = database.Vndb_GetReleaseById(releaseResult.RID);
                propertyGrid1.SelectedObject = release;
                notesTextBox.Text = release.Notes;
                if (releaseResult.Vns.Length == 1)
                    selectedObject = releaseResult.Vns[0];
                else
                    return;
            }

            VndbVnResult vnResult = selectedObject as VndbVnResult;
            if (vnResult != null)
            {
                VndbVn vn = database.Vndb_GetVnById(vnResult.VNID);
                propertyGrid1.SelectedObject = vn;
                if (vn.Screens.Count > 0)
                {
                    Galleria.DefaultGalleriaModel galleriaModel = new Galleria.DefaultGalleriaModel();
                    galleria1.GalleriaModel = galleriaModel;
                    galleriaModel.AddRange(vn.Screens);
                }

                notesTextBox.Text = vn.Description;
                bool showImage = true;
                if (vn.ImageNSFW)
                {
                    if (!context.Ini.ContainsKey("vndb"))
                        showImage = false;

                    IniSection iniSection = context.Ini["vndb"];
                    if (!iniSection.ContainsKey("hideNsfw"))
                        showImage = false;

                    int hideNsfw = Convert.ToInt32(iniSection["hideNsfw"]);
                    showImage = hideNsfw == 0;
                }

                if (showImage)
                {
                    pictureBox1.BackgroundImage = vn.Image;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
                }

                return;
            }

            MessageBox.Show("???");
        }

        private IEnumerable ChildrenGetter(object model)
        {
            VndbSearchResult result = model as VndbSearchResult;
            if (result == null)
                return null;
            if (result.Vns == null)
                return null;
            return result.Vns;
        }

        private bool CanExpandGetter(object model)
        {
            VndbSearchResult result = model as VndbSearchResult;
            if (result == null)
                return false;
            if (result.Vns == null)
                return false;
            if (result.Vns.Length == 0)
                return false;
            return true;
        }
        private object MainListImageGetter(object rowObject)
        {
            string type = rowObject.GetType().Name;
            switch (type)
            {
                case "VndbSearchResult":
                    return "package-x-generic";
                case "VndbVnResult":
                    return "input-gaming";
                default:
                    Console.WriteLine("wtf is a {0}", type);
                    return null;
            }
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
