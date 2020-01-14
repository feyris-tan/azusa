using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    public partial class MediaPickerForm : Form
    {
        private Entity.Shelf selectedShelf;
        private Entity.ProductInShelf selectedProduct;
        private Entity.MediaInProduct selectedMedia;
        private IDatabaseDriver dbDriver;

        public MediaPickerForm()
        {
            InitializeComponent();
            dbDriver = AzusaContext.GetInstance().DatabaseDriver;
            DialogResult = DialogResult.Cancel;
            UpdateControls();
            shelfSelector1.OnShelfSelectionChanged += ShelfSelector1_OnShelfSelectionChanged;
            shelfSelector1.SelectedIndex = 1;
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

            productComboBox.Items.Clear();
            if (selectedShelf != null)
            {
                var productInShelves = dbDriver.GetProductsInShelf(selectedShelf);
                foreach (ProductInShelf productInShelf in productInShelves)
                    productComboBox.Items.Add(productInShelf);
            }
        }

        private void productComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            selectedProduct = (ProductInShelf) productComboBox.SelectedItem;
            UpdateControls();

            mediaComboBox.Items.Clear();
            if (selectedProduct != null)
            {
                Product product = new Product();
                product.Id = selectedProduct.Id;
                IEnumerable<MediaInProduct> mediaInProducts = dbDriver.GetMediaByProduct(product);
                foreach (MediaInProduct mediaInProduct in mediaInProducts)
                    mediaComboBox.Items.Add(mediaInProduct);
                mediaComboBox.SelectedIndex = 0;
            }
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

        private void productAddButton_Click(object sender, EventArgs e)
        {
            string prompt = TextInputForm.Prompt("Name des neuen Produkts?");
            int newProductId = dbDriver.CreateProductAndReturnId(selectedShelf, prompt);
            int mediaId = dbDriver.CreateMediaAndReturnId(newProductId, "Disc 1");
            ShelfSelector1_OnShelfSelectionChanged(selectedShelf);
            foreach (ProductInShelf item in productComboBox.Items)
            {
                if (item.Id == mediaId)
                {
                    productComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}
