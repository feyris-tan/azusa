using System;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.Control.Galleria
{
    public partial class Galleria : UserControl
    {
        public Galleria()
        {
            InitializeComponent();
            galleriaModel = new DefaultGalleriaModel();
            galleriaModel.Galleria = this;
            UpdateControls();
        }

        private IGalleriaModel galleriaModel;
        private int currentImageNo;

        public int CurrentImageNo
        {
            get { return currentImageNo; }
            set { currentImageNo = value; UpdateControls(); }
        }

        public IGalleriaModel GalleriaModel
        {
            get
            {
                galleriaModel.Galleria = this;
                return galleriaModel;
            }
            set
            {
                currentImageNo = 0;
                galleriaModel = value;
                if (galleriaModel != null)
                    galleriaModel.Galleria = this;
            }
        }

        public void UpdateControls()
        {
            if (galleriaModel.ImagesCount == 0)
            {
                button1.Enabled = false;
                button2.Enabled = false;
                if (pictureBox1.Image != null)
                    pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }
            else
            {
                if (galleriaModel.ImagesCount == 1)
                    currentImageNo = 0;

                button1.Enabled = currentImageNo > 0;
                button2.Enabled = galleriaModel.ImagesCount - 1  > currentImageNo;
                pictureBox1.BackgroundImage = galleriaModel.GetImage(currentImageNo);
                pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            currentImageNo--;
            UpdateControls();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentImageNo++;
            UpdateControls();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            Form childform = new Form();
            childform.BackgroundImage = pictureBox1.BackgroundImage;
            childform.BackgroundImageLayout = ImageLayout.Zoom;
            childform.Width = pictureBox1.BackgroundImage.Width;
            childform.Height = pictureBox1.BackgroundImage.Height;
            childform.WindowState = FormWindowState.Maximized;
            childform.DoubleClick += delegate { childform.Close(); };
            childform.ShowDialog(this.FindForm());
        }
    }
}
