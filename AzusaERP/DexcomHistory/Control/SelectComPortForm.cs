using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.DexcomHistory.Control
{
    public partial class SelectComPortForm : Form
    {
        public SelectComPortForm()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Abort;
            foreach(string pName in SerialPort.GetPortNames())
            {
                listBox1.Items.Add(pName);
                listBox1.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectedSerialPort = new SerialPort(listBox1.SelectedItem.ToString());
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        public SerialPort SelectedSerialPort { get; private set; }

        public static SerialPort ShowSelectionFor(Form parent)
        {
            SelectComPortForm scpf = new SelectComPortForm();
            scpf.ShowDialog(parent);
            if (scpf.DialogResult == DialogResult.OK)
            {
                return scpf.SelectedSerialPort;
            }
            else
            {
                return null;
            }
        }
    }
}
