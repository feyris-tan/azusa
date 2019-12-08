using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.Control.Setup
{
    public partial class PropertyGridForm : Form
    {
        public PropertyGridForm(object obj)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = obj;
            StuffChanged = false;
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            StuffChanged = true;
        }

        public bool StuffChanged { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
