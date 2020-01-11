using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    public partial class MediaPickerForm : Form
    {
        private Entity.Shelf selectedShelf;
        private Entity.ProductInShelf selectedProduct;
        private Entity.MediaInProduct selectedMedia;

        public MediaPickerForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
            shelfSelector1.OnShelfSelectionChanged += ShelfSelector1_OnShelfSelectionChanged;
        }

        public void UpdateControls()
        {
            productComboBox.Enabled = selectedShelf != null;
            productAddButton.Enabled = selectedShelf != null;
            mediaComboBox.Enabled = selectedProduct != null;
            addMediaButton.Enabled = selectedProduct != null;
            confirm.Enabled = selectedMedia != null;
        }

        private void ShelfSelector1_OnShelfSelectionChanged(Entity.Shelf ssea)
        {
            selectedShelf = ssea;
            UpdateControls();
        }

        private void productComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedProduct = (ProductInShelf) productComboBox.SelectedItem;
            UpdateControls();
        }

        private void mediaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMedia = (MediaInProduct) mediaComboBox.SelectedItem;
            UpdateControls();
        }

        public MediaInProduct SelectedMedia
        {
            get => selectedMedia;
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
