using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AzusaERP;
using libazustreamblob;
using moe.yo3explorer.azusa.Gelbooru.Control;
using moe.yo3explorer.azusa.Gelbooru.Entity;

namespace moe.yo3explorer.azusa.Gelbooru.Boundary
{
    public partial class GelboooruControl : UserControl, IAzusaModule
    {
        public GelboooruControl()
        {
            InitializeComponent();
        }

        private string[] BAD_WORDS = { "vaginal", "sex", "pussy"};
        private AzusaContext context;
        private AzusaStreamBlob streamBlob;
        private List<GelbooruTag> gelbooruTags;

        public string IniKey => "safebooru";
        public string Title => "Safebooru (25GB Anime-Bilder)";
        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            context = AzusaContext.GetInstance();
            if (!context.Ini.ContainsKey(IniKey))
                return;

            IniSection iniSection = context.Ini[IniKey];
            if (!iniSection.ContainsKey("path"))
                throw new Exception("path missing");

            if (iniSection.ContainsKey("disable"))
                if (Convert.ToInt32(iniSection["disable"]) > 0)
                    return;

            DirectoryInfo di = new DirectoryInfo(iniSection["path"]);
            if (!di.Exists)
                throw new DirectoryNotFoundException(di.FullName);

            context.Splash.SetLabel("Starte Anime-Bilder-Datenbank...");
            streamBlob = new AzusaStreamBlob(di, true);

            context.Splash.SetLabel("Lese Tag-Index der Bilder-Datenbank...");
            IEnumerable<GelbooruTag> tags = context.DatabaseDriver.Gelbooru_GetAllTags();
            gelbooruTags = new List<GelbooruTag>();
            foreach (GelbooruTag tag in tags)
            {
                if (!BAD_WORDS.Contains(tag.Tag))
                    gelbooruTags.Add(tag);
            }
            textBox1_TextChanged(null, null);
        }

        public int Priority => 10;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GelbooruTag tag = (GelbooruTag)listBox1.SelectedItem;
            IEnumerable<int> postIds = context.DatabaseDriver.Gelbooru_GetPostsByTag(tag.Id);
            GelbooruGalleriaModel model = new GelbooruGalleriaModel();
            galleria1.GalleriaModel = model;
            model.StreamBlob = streamBlob;
            List<int> posts = new List<int>();
            foreach (int postId in postIds)
            {
                if (streamBlob.TestFor(1, 1, postId))
                {
                    posts.Add(postId);
                }
            }

            model.Ids = posts.ToArray();
            galleria1.UpdateControls();
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListBox.ObjectCollection addMe = new ListBox.ObjectCollection(listBox1);
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string filter = textBox1.Text;
                IEnumerable<GelbooruTag> tags = gelbooruTags.Where(x => x.Tag.Contains(filter));
                foreach (GelbooruTag tag in tags)
                {
                    addMe.Add(tag);
                }
            }
            else
            {
                foreach (GelbooruTag tag in gelbooruTags)
                {
                    addMe.Add(tag);
                }
            }

            context.Splash.SetLabel("Baue ListBox1 im GelbooruControl auf...");
            listBox1.Items.AddRange(addMe);
        }
    }
}
