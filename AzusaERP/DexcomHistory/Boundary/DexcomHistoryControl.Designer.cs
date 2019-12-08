namespace moe.yo3explorer.azusa.DexcomHistory.Boundary
{
    partial class DexcomHistory
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.azusaDexXMLImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gerätespeicherAuslesenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manuelleDatenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.plotSurface2D1 = new NPlot.Windows.PlotSurface2D();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.hBA1CSchätzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.azusaDexXMLImportierenToolStripMenuItem,
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem,
            this.gerätespeicherAuslesenToolStripMenuItem,
            this.cSVExportToolStripMenuItem,
            this.manuelleDatenToolStripMenuItem,
            this.hBA1CSchätzenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(756, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // azusaDexXMLImportierenToolStripMenuItem
            // 
            this.azusaDexXMLImportierenToolStripMenuItem.Name = "azusaDexXMLImportierenToolStripMenuItem";
            this.azusaDexXMLImportierenToolStripMenuItem.Size = new System.Drawing.Size(108, 20);
            this.azusaDexXMLImportierenToolStripMenuItem.Text = "XML importieren";
            this.azusaDexXMLImportierenToolStripMenuItem.Click += new System.EventHandler(this.azusaDexXMLImportierenToolStripMenuItem_Click);
            // 
            // gerätespeicherInAzusaDexXMLToolStripMenuItem
            // 
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem.Name = "gerätespeicherInAzusaDexXMLToolStripMenuItem";
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem.Size = new System.Drawing.Size(186, 20);
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem.Text = "Gerätespeicher in XML kopieren";
            this.gerätespeicherInAzusaDexXMLToolStripMenuItem.Click += new System.EventHandler(this.gerätespeicherInAzusaDexXMLToolStripMenuItem_Click);
            // 
            // gerätespeicherAuslesenToolStripMenuItem
            // 
            this.gerätespeicherAuslesenToolStripMenuItem.Name = "gerätespeicherAuslesenToolStripMenuItem";
            this.gerätespeicherAuslesenToolStripMenuItem.Size = new System.Drawing.Size(145, 20);
            this.gerätespeicherAuslesenToolStripMenuItem.Text = "Gerätespeicher auslesen";
            this.gerätespeicherAuslesenToolStripMenuItem.Click += new System.EventHandler(this.gerätespeicherAuslesenToolStripMenuItem_Click);
            // 
            // cSVExportToolStripMenuItem
            // 
            this.cSVExportToolStripMenuItem.Name = "cSVExportToolStripMenuItem";
            this.cSVExportToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.cSVExportToolStripMenuItem.Text = "CSV-Export";
            this.cSVExportToolStripMenuItem.Click += new System.EventHandler(this.cSVExportToolStripMenuItem_Click);
            // 
            // manuelleDatenToolStripMenuItem
            // 
            this.manuelleDatenToolStripMenuItem.Name = "manuelleDatenToolStripMenuItem";
            this.manuelleDatenToolStripMenuItem.Size = new System.Drawing.Size(102, 20);
            this.manuelleDatenToolStripMenuItem.Text = "manuelle Daten";
            this.manuelleDatenToolStripMenuItem.Click += new System.EventHandler(this.manuelleDatenToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.plotSurface2D1);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer1.Size = new System.Drawing.Size(756, 319);
            this.splitContainer1.SplitterDistance = 99;
            this.splitContainer1.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(99, 319);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // plotSurface2D1
            // 
            this.plotSurface2D1.AutoScaleAutoGeneratedAxes = false;
            this.plotSurface2D1.AutoScaleTitle = false;
            this.plotSurface2D1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.plotSurface2D1.DateTimeToolTip = false;
            this.plotSurface2D1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotSurface2D1.Legend = null;
            this.plotSurface2D1.LegendZOrder = -1;
            this.plotSurface2D1.Location = new System.Drawing.Point(0, 39);
            this.plotSurface2D1.Name = "plotSurface2D1";
            this.plotSurface2D1.RightMenu = null;
            this.plotSurface2D1.ShowCoordinates = true;
            this.plotSurface2D1.Size = new System.Drawing.Size(653, 280);
            this.plotSurface2D1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            this.plotSurface2D1.TabIndex = 3;
            this.plotSurface2D1.Text = "plotSurface2D1";
            this.plotSurface2D1.Title = "";
            this.plotSurface2D1.TitleFont = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plotSurface2D1.XAxis1 = null;
            this.plotSurface2D1.XAxis2 = null;
            this.plotSurface2D1.YAxis1 = null;
            this.plotSurface2D1.YAxis2 = null;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripButton1,
            this.toolStripTextBox1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(653, 39);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(86, 36);
            this.toolStripLabel1.Text = "toolStripLabel1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::moe.yo3explorer.azusa.Properties.Resources.NavBack;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.ReadOnly = true;
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 39);
            this.toolStripTextBox1.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::moe.yo3explorer.azusa.Properties.Resources.NavForward;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 36);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XML Dateien(*.xml)|*.xml";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "UTF-8 CSV (*.csv)|*.csv";
            // 
            // hBA1CSchätzenToolStripMenuItem
            // 
            this.hBA1CSchätzenToolStripMenuItem.Name = "hBA1CSchätzenToolStripMenuItem";
            this.hBA1CSchätzenToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.hBA1CSchätzenToolStripMenuItem.Text = "HBA1C schätzen";
            this.hBA1CSchätzenToolStripMenuItem.Click += new System.EventHandler(this.hBA1CSchätzenToolStripMenuItem_Click);
            // 
            // DexcomHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "DexcomHistory";
            this.Size = new System.Drawing.Size(756, 343);
            this.Resize += new System.EventHandler(this.UserControl1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ToolStripMenuItem azusaDexXMLImportierenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem gerätespeicherInAzusaDexXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gerätespeicherAuslesenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cSVExportToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private NPlot.Windows.PlotSurface2D plotSurface2D1;
        private System.Windows.Forms.ToolStripMenuItem manuelleDatenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hBA1CSchätzenToolStripMenuItem;
    }
}
