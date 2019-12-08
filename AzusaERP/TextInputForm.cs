using System;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa
{
    public partial class TextInputForm : Form
    {
        public TextInputForm()
        {
            InitializeComponent();
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        public static string Prompt(string question,Form form = null)
        {
            TextInputForm tif = new TextInputForm();
            tif.label1.Text = question;
            if (tif.ShowDialog(form) == DialogResult.OK)
            {
                return tif.textBox1.Text;
            }
            else
            {
                return null;
            }
        }

        public static string PromptPassword(string question, Form form = null)
        {
            TextInputForm tif = new TextInputForm();
            tif.label1.Text = question;
            tif.textBox1.UseSystemPasswordChar = true;
            if (tif.ShowDialog(form) == DialogResult.OK)
            {
                return tif.textBox1.Text;
            }
            else
            {
                return null;
            }
        }

        private void TextInputForm_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
            }
        }
    }
}
