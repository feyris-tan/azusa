using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            
            if (File.Exists("splash.jpg"))
            {
                ms = new MemoryStream(File.ReadAllBytes("splash.jpg"));
                Image theImage = Image.FromStream(ms);
                Screen screen = Screen.PrimaryScreen;
                int imageHeight = theImage.Height + 50;
                int imageWidth = theImage.Width;

                int targetHeight = screen.WorkingArea.Width;
                int targetWidth = screen.WorkingArea.Height;
                while ((targetWidth < imageWidth) || (targetHeight < imageHeight))
                {
                    imageWidth = (int) ((double)imageWidth * 0.9);
                    imageHeight = (int) ((double)imageHeight * 0.9);
                }

                this.Width = imageWidth;
                this.Height = imageHeight;
                pictureBox1.BackgroundImage = theImage;
            }
        }

        private MemoryStream ms;

        public void InvokeClose()
        {
            if (!Visible)
                return;

            Invoke((MethodInvoker)delegate { Close(); });
        }

        public void SetLabel(string text)
        {
            if (!this.Visible)
                return;

            if (label1 == null)
                return;

            if (Debugger.IsAttached)
                Console.WriteLine(text);

            Invoke((MethodInvoker)delegate { label1.Text = text; });
        }

        public IntPtr WaitForHandle()
        {
            IntPtr test = IntPtr.Zero;
            while (test.Equals(IntPtr.Zero))
            {
                try
                {
                    Invoke((MethodInvoker)delegate { test = Handle; });
                }
                catch (Exception)
                {
                    Thread.Sleep(10);
                }
            }
            return test;
        }
    }
}