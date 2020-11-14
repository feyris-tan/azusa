using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.Galleria;
using moe.yo3explorer.azusa.OfflineReaders.PsxDatacenter.Entity;

namespace moe.yo3explorer.azusa.OfflineReaders.PsxDatacenter.Boundary
{
    public partial class PsxDatacenterControl : UserControl, IAzusaModule
    {
        public PsxDatacenterControl()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
        }

        private AzusaContext context;

        public string IniKey => "psxdatacenter";
        public string Title => "PSX DataCenter Offline";
        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
        }

        public int Priority => 6;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            PsxDatacenterPreview[] previews = context.DatabaseDriver.PsxDc_Search(toolStripTextBox1.Text).ToArray();
            dataGridView1.DataSource = previews;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
                return;

            dataGridView1.SelectedCells[0].OwningRow.Selected = true;
            dataGridView1_RowEnter(sender, e);
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
                return;

            IDatabaseDriver database = context.DatabaseDriver;
            DataGridViewRow row = dataGridView1.SelectedRows[0];
            PsxDatacenterPreview preview = (PsxDatacenterPreview)row.DataBoundItem;
            if (row.DataBoundItem == null)
                return;

            if (pictureBox1.BackgroundImage != null)
                pictureBox1.BackgroundImage.Dispose();

            pictureBox1.BackgroundImage = null;
            propertyGrid1.SelectedObject = null;
            galleria1.GalleriaModel = new EmptyGalleriaModel();
            textBox1.Text = String.Empty;

            if (preview != null)
                if (!preview.HasAdditionalData)
                    return;

            PsxDatacenterGame game = database.PsxDc_GetSpecificGame(preview.Id);
            propertyGrid1.SelectedObject = game;
            pictureBox1.BackgroundImage = Image.FromStream(new MemoryStream(game.Cover));
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            textBox1.Text = game.Description;

            List<byte[]> screenshots = database.PsxDc_GetScreenshots(preview.Id).ToList();
            List<Image> images = screenshots.ConvertAll(x => Image.FromStream(new MemoryStream(x, false)));

            DefaultGalleriaModel galleriaModel = new DefaultGalleriaModel();
            galleria1.GalleriaModel = galleriaModel;
            galleriaModel.AddRange(images);
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
