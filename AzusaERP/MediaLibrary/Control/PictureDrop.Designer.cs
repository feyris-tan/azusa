namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    partial class PictureDrop
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.dateiLadenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuesBildScannenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bildSpeichernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bildLöschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ePUBCoverExtrahierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openJpg = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openEpub = new System.Windows.Forms.OpenFileDialog();
            this.openFlac = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(150, 150);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiLadenToolStripMenuItem,
            this.neuesBildScannenToolStripMenuItem,
            this.bildSpeichernToolStripMenuItem,
            this.bildLöschenToolStripMenuItem,
            this.ePUBCoverExtrahierenToolStripMenuItem,
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(261, 136);
            // 
            // dateiLadenToolStripMenuItem
            // 
            this.dateiLadenToolStripMenuItem.Name = "dateiLadenToolStripMenuItem";
            this.dateiLadenToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.dateiLadenToolStripMenuItem.Text = "Datei laden";
            this.dateiLadenToolStripMenuItem.Click += new System.EventHandler(this.dateiLadenToolStripMenuItem_Click);
            // 
            // neuesBildScannenToolStripMenuItem
            // 
            this.neuesBildScannenToolStripMenuItem.Name = "neuesBildScannenToolStripMenuItem";
            this.neuesBildScannenToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.neuesBildScannenToolStripMenuItem.Text = "Neues Bild scannen";
            this.neuesBildScannenToolStripMenuItem.Click += new System.EventHandler(this.neuesBildScannenToolStripMenuItem_Click);
            // 
            // bildSpeichernToolStripMenuItem
            // 
            this.bildSpeichernToolStripMenuItem.Name = "bildSpeichernToolStripMenuItem";
            this.bildSpeichernToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.bildSpeichernToolStripMenuItem.Text = "Bild speichern";
            this.bildSpeichernToolStripMenuItem.Click += new System.EventHandler(this.bildSpeichernToolStripMenuItem_Click);
            // 
            // bildLöschenToolStripMenuItem
            // 
            this.bildLöschenToolStripMenuItem.Name = "bildLöschenToolStripMenuItem";
            this.bildLöschenToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.bildLöschenToolStripMenuItem.Text = "Bild löschen";
            this.bildLöschenToolStripMenuItem.Click += new System.EventHandler(this.bildLöschenToolStripMenuItem_Click);
            // 
            // ePUBCoverExtrahierenToolStripMenuItem
            // 
            this.ePUBCoverExtrahierenToolStripMenuItem.Name = "ePUBCoverExtrahierenToolStripMenuItem";
            this.ePUBCoverExtrahierenToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.ePUBCoverExtrahierenToolStripMenuItem.Text = "EPUB-Cover extrahieren";
            this.ePUBCoverExtrahierenToolStripMenuItem.Click += new System.EventHandler(this.ePUBCoverExtrahierenToolStripMenuItem_Click);
            // 
            // coverbildAusAPETagsExtrahierenToolStripMenuItem
            // 
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem.Name = "coverbildAusAPETagsExtrahierenToolStripMenuItem";
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem.Text = "Coverbild aus APE-Tags extrahieren";
            this.coverbildAusAPETagsExtrahierenToolStripMenuItem.Click += new System.EventHandler(this.coverbildAusAPETagsExtrahierenToolStripMenuItem_Click);
            // 
            // openJpg
            // 
            this.openJpg.FileName = "Bild öffnen";
            this.openJpg.Filter = "Bilddatei (*.jpg;*.bmp)|*.jpg;*.bmp";
            this.openJpg.FileOk += new System.ComponentModel.CancelEventHandler(this.openJpg_FileOk);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "JPEG Bild (*.jpg)|*.jpg";
            // 
            // openEpub
            // 
            this.openEpub.Filter = "Electronic PUBlication (*.epub)|*.epub";
            // 
            // openFlac
            // 
            this.openFlac.FileName = "openFileDialog1";
            this.openFlac.Filter = "FLAC (*.flac)|*.flac";
            // 
            // PictureDrop
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Name = "PictureDrop";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PictureBox_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiLadenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openJpg;
        private System.Windows.Forms.ToolStripMenuItem neuesBildScannenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bildSpeichernToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem bildLöschenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ePUBCoverExtrahierenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openEpub;
        private System.Windows.Forms.ToolStripMenuItem coverbildAusAPETagsExtrahierenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFlac;
    }
}
