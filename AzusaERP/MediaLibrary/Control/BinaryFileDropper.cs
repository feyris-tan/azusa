using System;
using System.IO;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class BinaryFileDropper : UserControl, ISidecarDisplayControl
    {
        public BinaryFileDropper()
        {
            InitializeComponent();
        }

        private byte[] buffer;

        public bool isComplete()
        {
            return buffer != null && buffer.Length > 0;
        }

        public Guid DisplayControlUuid => new Guid("{3671208D-EB89-4D02-9081-EBA21BDC2E6F}");

        public byte[] Data
        {
            get
            {
                return buffer;
            }
            set
            {
                buffer = value;
                if (buffer != null)
                {
                    label1.Text = String.Format("Buffer Size: {0}", buffer.Length);
                }
                else
                {
                    label1.Text = "No data";
                }

                DataChanged?.Invoke(this, Data);
                OnDataChanged?.Invoke(Data, isComplete(), MediumId);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            Data = File.ReadAllBytes(openFileDialog1.FileName);
        }

        public delegate void DataChangeDelegate(object sender, byte[] newData);
        public event DataChangeDelegate DataChanged;

        private void button2_Click(object sender, EventArgs e)
        {
            Data = null;
        }

        private void label2_DragEnter(object sender, DragEventArgs args)
        {
            if (!_enabled)
                return;

            if (args.Data.GetDataPresent(DataFormats.FileDrop))
            {
                args.Effect = DragDropEffects.Copy;
            }
        }

        private void label2_DragDrop(object sender, DragEventArgs e)
        {
            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            Data = File.ReadAllBytes(jStrings[0]);
        }

        public new bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                button1.Enabled = value;
                button2.Enabled = value;
            }
        }
        private bool _enabled;

        public int MediumId { get; set; }
        public System.Windows.Forms.Control ToControl()
        {
            return this;
        }

        public void ForceEnabled()
        {
            Enabled = true;
        }

        public event SidecarChange OnDataChanged;
    }
}
