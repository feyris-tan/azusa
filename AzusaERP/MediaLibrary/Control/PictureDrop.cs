using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ionic.Zip;
using TagLib;
using File = System.IO.File;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class PictureDrop : UserControl, ISidecarDisplayControl
    {
        const int MaxPictureSize = 900 * 1000;

        public PictureDrop()
        {
            InitializeComponent();
            pictureBox1.AllowDrop = true;
            pictureBox1.DragEnter += PictureBox_DragEnter;
            pictureBox1.DragDrop += PictureBox_DragDrop;
            pictureBox1.Paint += PictureBox1_Paint;

            
        }

        private bool pluginsLoaded;
        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (!pluginsLoaded)
            {
                LoadPlugins();
                pluginsLoaded = true;
            }
        }

        private void LoadPlugins()
        {
            AzusaContext azusaContext = AzusaContext.GetInstance();
            foreach (IImageAcquisitionPlugin plugin in azusaContext.ImageAcquisitionPlugins)
            {
                if (plugin.CanStart())
                {
                    ToolStripButton tsb = new ToolStripButton(plugin.Name);
                    tsb.Click += delegate (object sender, EventArgs args)
                    {
                        Image result = plugin.Acquire();
                        if (result != null)
                        {
                            Data = JpegCompressor.CompressJpeg(result, MaxPictureSize);
                        }
                        else
                        {
                            MessageBox.Show("Das Plug-In hat kein Bild geliefert!");
                        }
                    };
                    this.contextMenuStrip1.Items.Add(tsb);
                }
            }
        }

        private void PictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                object j = e.Data.GetData(DataFormats.FileDrop);
                string[] jStrings = (string[])j;
                if (jStrings.Length == 1)
                {
                    FileInfo fi = new FileInfo(jStrings[0]);
                    if (IsImageFile(fi))
                    {
                        e.Effect = DragDropEffects.Copy;
                    }
                }
            }
        }

        private bool IsImageFile(FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            byte[] buffer = new byte[6];
            int readSize = fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            if (readSize != 6)
                return false;

            if ((buffer[0] == 0x42) & (buffer[1] == 0x4D))
                return true;

            if ((buffer[0] == 0xFF) & (buffer[1] == 0xD8) & (buffer[2] == 0xFF) & (buffer[3] == 0xE0))
                return true;

            if ((buffer[0] == 0xFF) & (buffer[1] == 0xD8) & (buffer[2] == 0xFF) & (buffer[3] == 0xE1))
                return true;

            return false;
        }

        private void PictureBox_DragDrop(object sender, DragEventArgs e)
        {
            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            Data = File.ReadAllBytes(jStrings[0]);
        }

        private byte[] buffer;
        private MemoryStream ms;

        public bool isComplete()
        {
            return buffer != null && buffer.Length > 0;
        }

        public Guid DisplayControlUuid => new Guid("EBF2D5F6-CF90-4E16-979C-CB8F73066AE0");

        public byte[] Data
        {
            set
            {
                if (ms != null)
                {
                    ms.Dispose();
                }

                buffer = value;
                bildSpeichernToolStripMenuItem.Enabled = buffer != null;

                bool hasImage = false;
                if (buffer != null)
                {
                    if (buffer.Length > 0)
                    {
                        ms = new MemoryStream(buffer);
                        pictureBox1.BackgroundImage = Image.FromStream(ms);
                        hasImage = true;
                    }
                }
                
                if (!hasImage)
                {
                    buffer = null;
                    pictureBox1.BackgroundImage = null;
                }
                DataChanged = true;
            }
            get
            {
                return buffer;
            }
        }
        
        public bool DataChanged { get; set; }

        private void dateiLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openJpg.ShowDialog() == DialogResult.OK)
            {
                if (IsImageFile(new FileInfo(openJpg.FileName)))
                {
                    Data = File.ReadAllBytes(openJpg.FileName);
                }
                else
                {
                    MessageBox.Show(String.Format("{0} ist kein Bild!", openJpg.FileName));
                }
            }
        }

        private void neuesBildScannenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image image = ScanForm.ScanSomething(this.FindForm());
            if (image != null)
            {
                Data = JpegCompressor.CompressJpeg(image, MaxPictureSize);
            }
        }

        private void bildSpeichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(saveFileDialog1.FileName, Data);
            }
        }

        private void bildLöschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Data = null;
        }

        private void ePUBCoverExtrahierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openEpub.ShowDialog() != DialogResult.OK)
                return;

            ZipFile zf = ZipFile.Read(openEpub.FileName);
            foreach(ZipEntry entry in zf)
            {
                if (entry.FileName.EndsWith("cover.jpg") || entry.FileName.EndsWith("hyousi.jpg"))
                {
                    Stream zipStream = entry.OpenReader();
                    MemoryStream imageStream = new MemoryStream();
                    zipStream.CopyTo(imageStream);
                    int imageLength = (int)imageStream.Position;
                    zipStream.Close();
                    if (imageLength > MaxPictureSize)
                    {
                        Image image = Image.FromStream(imageStream);
                        Data = JpegCompressor.CompressJpeg(image, MaxPictureSize);
                        return;
                    }
                    Data = imageStream.ToArray();
                    return;
                }
            }
        }

        private void coverbildAusAPETagsExtrahierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFlac.ShowDialog(this) != DialogResult.OK)
                return;

            TagLib.File file = TagLib.File.Create(openFlac.FileName);
            IPicture[] pictures = file.Tag.Pictures;
            byte[] data = pictures[0].Data.Data;
            Data = data;
            file.Dispose();
        }

        private void openJpg_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        public int MediumId { get; set; }
    }
}
