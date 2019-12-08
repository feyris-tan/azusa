using System;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class CurrencyConverterTextBox : UserControl
    {
        public CurrencyConverterTextBox()
        {
            InitializeComponent();
        }

        private void jPYEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / 128.99).ToString();
        }

        private void uSDEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / 1.14).ToString();
        }

        private void gBPEURToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) / 0.89).ToString();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (IsNumeric)
                textBox1.Text = (Convert.ToDouble(textBox1.Text) * 0.5).ToString();
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
