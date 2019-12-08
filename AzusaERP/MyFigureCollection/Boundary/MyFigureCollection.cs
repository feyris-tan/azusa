using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MyFigureCollection.Entity;

namespace moe.yo3explorer.azusa.MyFigureCollection.Boundary
{
    public partial class MyFigureCollection : UserControl, IAzusaModule
    {
        public MyFigureCollection()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
        }

        private AzusaContext context;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (Image trashedImage in imageList1.Images) 
                trashedImage.Dispose();

            imageList1.Images.Clear();
            listView1.Items.Clear();
            if (pictureBox1.BackgroundImage != null)
            {
                pictureBox1.BackgroundImage.Dispose();
                pictureBox1.BackgroundImage = null;
            }

            bool cheapout = context.ReadIniKey(IniKey, "cheapout", 0) != 0;

            IEnumerable<Figure> search = context.DatabaseDriver.MyFigureCollection_Search(toolStripTextBox1.Text);
            int ordinal = 0;
            foreach (Figure figure in search)
            {
                imageList1.Images.Add(Image.FromStream(new MemoryStream(figure.Thumbnail)));
                FigureListViewItem item = new FigureListViewItem(ordinal++, figure, cheapout);
                listView1.Items.Add(item);
            }

            if (imageList1.Images.Count > 0)
            {
                Size imageSize = imageList1.Images[0].Size;
                imageList1.ImageSize = imageSize;
            }
        }

        public string IniKey => "myfigurecollection";
        public string Title => "My Figure Collection Offline";
        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
        }

        public int Priority => 8;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection items = listView1.SelectedItems;
            if (items.Count == 0)
                return;
            FigureListViewItem figure = (FigureListViewItem)items[0];
            pictureBox1.Image = context.DatabaseDriver.MyFigureCollection_GetPhoto(figure.WrappedFigure.ID);
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            
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
