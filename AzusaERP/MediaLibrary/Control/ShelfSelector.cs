using System;
using System.Collections.Generic;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class ShelfSelector : UserControl
    {
        public ShelfSelector()
        {
            InitializeComponent();
            IDatabaseDriver databaseDriver = AzusaContext.GetInstance().DatabaseDriver;
            if (databaseDriver != null)
            {
                IEnumerable<Shelf> shelves = databaseDriver.GetAllShelves();
                foreach (Shelf shelf in shelves)
                {
                    comboBox1.Items.Add(shelf);
                }
            }
            else
            {
                Shelf dummy = new Shelf();
                dummy.Name = "This won't work in the designer!";
                comboBox1.Items.Add(dummy);
            }
            comboBox1.SelectedIndex = 0;
        }

        public void TrySetShelfByName(string name)
        {
            foreach (object comboBox1Item in comboBox1.Items)
            {
                Shelf s = (Shelf) comboBox1Item;
                if (s.Name.Equals(name))
                {
                    comboBox1.SelectedItem = comboBox1Item;
                    return;
                }
            }
        }

        public int SelectedIndex
        {
            get => comboBox1.SelectedIndex;
            set => comboBox1.SelectedIndex = value;
        }

        public Shelf SelectedShelf
        {
            get
            {
                return (Shelf) comboBox1.SelectedItem;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, System.EventArgs e)
        {
            OnShelfSelectionChanged?.Invoke(SelectedShelf);
        }

        public event ShelfSelection OnShelfSelectionChanged;
    }

    public delegate void ShelfSelection(Shelf ssea);

}
