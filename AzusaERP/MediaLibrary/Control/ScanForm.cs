using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using NTwain;
using NTwain.Data;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class ScanForm : Form
    {
        TWIdentity appId;
        public TwainSession session;
        DataSource scanner;
        ReturnCode twainRC;

        Bitmap theImage;

        private ScanForm()
        {
            InitializeComponent();
            appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetExecutingAssembly());
            session = new TwainSession(appId);

            session.TransferReady += new EventHandler<TransferReadyEventArgs>(session_TransferReady);
            session.DataTransferred += new EventHandler<DataTransferredEventArgs>(session_DataTransferred);
            
            session.Open();

            scanner = session.ShowSourceSelector();

            if (scanner == null)
            {
                this.DialogResult = DialogResult.Abort;
                session.Close();
                return;
            }

            ReturnCode rc = scanner.Open();
            if (rc != ReturnCode.Success)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                session.Close();
                return;
            }

            
            
            
        }

        void session_DataTransferred(object sender, DataTransferredEventArgs e)
        {
            theImage = null;
            Stream s = e.GetNativeImageStream();
            int len = (int)s.Length;
            byte[] buffer = new byte[len];
            s.Read(buffer, 0, len);
            s.Close();
            MemoryStream ms = new MemoryStream(buffer);
            theImage = (Bitmap)Image.FromStream(ms);
            pictureBox1.Image = theImage;
        }

        void session_TransferReady(object sender, TransferReadyEventArgs e)
        {
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            twainRC = scanner.Enable(SourceEnableMode.ShowUI, true, IntPtr.Zero);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (theImage == null) return;
            theImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox1.Image = theImage;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (theImage == null) return;
            theImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureBox1.Image = theImage;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public static Image ScanSomething(Form f = null)
        {

            ScanForm sf = new ScanForm();
            if (sf.DialogResult == DialogResult.Abort)
                return null;

            if (f == null)
            { sf.ShowDialog(); }
            else
            { sf.ShowDialog(f); }

            sf.session.Close();

            return sf.theImage;
        }

        public static void ScanSomethingAndSaveIt(string fullName, Form f = null)
        {
            Image m = ScanSomething(f);
            m.Save(fullName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
