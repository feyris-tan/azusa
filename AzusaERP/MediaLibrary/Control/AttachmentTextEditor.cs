using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class AttachmentTextEditor : UserControl, ISidecarDisplayControl
    {
        public AttachmentTextEditor()
        {
            InitializeComponent();
            textBox1.Enabled = true;
        }

        public Guid DisplayControlUuid => Guid.Parse("c56e7f3e-f129-4a8f-b0d2-9035474b69b8");

        public byte[] Data
        {
            get { return Encoding.UTF8.GetBytes(textBox1.Text); }
            set
            {
                if (value == null)
                    return;
                textBox1.Text = Encoding.UTF8.GetString(value);
                OnDataChanged?.Invoke(value, isComplete(), MediumId);
            }
        }

        public bool isComplete()
        {
            return !string.IsNullOrEmpty(textBox1.Text);
        }

        public int MediumId { get; set; }
        public System.Windows.Forms.Control ToControl()
        {
            return this;
        }

        private void Global_DragEnter(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
                args.Effect = DragDropEffects.Copy;
        }

        private void TextBox_DragAndDrop(object sender, DragEventArgs e)
        {
            TextBox target = (TextBox)sender;

            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            target.MaxLength = Int32.MaxValue;
            target.Text = File.ReadAllText(jStrings[0]);
            if (target.Text.StartsWith("<?xml "))
            {
                target.Text = FormatXml(target.Text);
            }
            OnDataChanged?.Invoke(Data, isComplete(), MediumId);
        }

        public void ForceEnabled()
        {
            textBox1.Enabled = true;
        }

        public event SidecarChange OnDataChanged;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            OnDataChanged?.Invoke(Data, isComplete(), MediumId);
        }

        string FormatXml(string xml)
        {
            try
            {
                XDocument doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                // Handle and throw if fatal exception here; don't just ignore them
                return xml;
            }
        }
    }
}
