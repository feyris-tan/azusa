using System;
using System.Windows.Forms;
using libeuroexchange.Model;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class CurrencyConverterTextBox : UserControl
    {
        private AzusaContext context;
        private AzusifiedCube cube;

        public CurrencyConverterTextBox()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
            cube = context.DatabaseDriver.GetLatestEuroExchangeRates();
            jPYEURToolStripMenuItem.Enabled = cube != null;
            uSDEURToolStripMenuItem.Enabled = cube != null;
            gBPEURToolStripMenuItem.Enabled = cube != null;
        }

        private void jPYEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / cube.JPY).ToString();
        }

        private void uSDEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / cube.USD).ToString();
        }

        private void gBPEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / cube.GBP).ToString();
        }
        
        public double? Value
        {
            get
            {
                if (textBox1.Text.Length == 0)
                    return 0;

                string act = textBox1.Text.Replace(',', '.');
                if (IsNumeric)
                    return Convert.ToDouble(act, System.Globalization.CultureInfo.InvariantCulture);
                else
                    return null;
            }
            set
            {
                textBox1.Text = value.Value.ToString();
            }
        }

        public bool IsNumeric
        {
            get
            {
                string act = textBox1.Text.Replace(',', '.');
                foreach (char a in act)
                {
                    if (!char.IsDigit(a) && (a != '.'))
                        return false;
                }
                return true;
            }
        }
    }
}
