using moe.yo3explorer.azusa.Control.Galleria;
using moe.yo3explorer.azusa.OfflineReaders.VgmDb.Entity;

namespace moe.yo3explorer.azusa.OfflineReaders.VgmDb.Boundary
{
    partial class VgmDbControl
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.propertyTabPage = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.galleriaTabPage = new System.Windows.Forms.TabPage();
            this.notesTabPage = new System.Windows.Forms.TabPage();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.catalogDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classificationNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mediaformatNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.publishformatNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.albumListEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.galleria1 = new Galleria();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.propertyTabPage.SuspendLayout();
            this.galleriaTabPage.SuspendLayout();
            this.notesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumListEntryBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(641, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            this.toolStripTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.toolStripTextBox1_KeyPress);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::moe.yo3explorer.azusa.Properties.Resources.Find_VS;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(66, 22);
            this.toolStripButton1.Text = "Suchen";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Size = new System.Drawing.Size(641, 326);
            this.splitContainer1.SplitterDistance = 495;
            this.splitContainer1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.catalogDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.dateDataGridViewTextBoxColumn,
            this.typeNameDataGridViewTextBoxColumn,
            this.classificationNameDataGridViewTextBoxColumn,
            this.mediaformatNameDataGridViewTextBoxColumn,
            this.publishformatNameDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.albumListEntryBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(495, 326);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_RowEnter);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.9976F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.0024F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(142, 326);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 124);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.propertyTabPage);
            this.tabControl1.Controls.Add(this.galleriaTabPage);
            this.tabControl1.Controls.Add(this.notesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 133);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(136, 190);
            this.tabControl1.TabIndex = 1;
            // 
            // propertyTabPage
            // 
            this.propertyTabPage.Controls.Add(this.propertyGrid1);
            this.propertyTabPage.Location = new System.Drawing.Point(4, 22);
            this.propertyTabPage.Name = "propertyTabPage";
            this.propertyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.propertyTabPage.Size = new System.Drawing.Size(128, 164);
            this.propertyTabPage.TabIndex = 0;
            this.propertyTabPage.Text = "Eigenschaften";
            this.propertyTabPage.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(122, 158);
            this.propertyGrid1.TabIndex = 2;
            // 
            // galleriaTabPage
            // 
            this.galleriaTabPage.Controls.Add(this.galleria1);
            this.galleriaTabPage.Location = new System.Drawing.Point(4, 22);
            this.galleriaTabPage.Name = "galleriaTabPage";
            this.galleriaTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.galleriaTabPage.Size = new System.Drawing.Size(128, 164);
            this.galleriaTabPage.TabIndex = 1;
            this.galleriaTabPage.Text = "Gallerie";
            this.galleriaTabPage.UseVisualStyleBackColor = true;
            // 
            // notesTabPage
            // 
            this.notesTabPage.Controls.Add(this.notesTextBox);
            this.notesTabPage.Location = new System.Drawing.Point(4, 22);
            this.notesTabPage.Name = "notesTabPage";
            this.notesTabPage.Size = new System.Drawing.Size(128, 164);
            this.notesTabPage.TabIndex = 2;
            this.notesTabPage.Text = "Notizen";
            this.notesTabPage.UseVisualStyleBackColor = true;
            // 
            // notesTextBox
            // 
            this.notesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notesTextBox.Location = new System.Drawing.Point(0, 0);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.ReadOnly = true;
            this.notesTextBox.Size = new System.Drawing.Size(128, 164);
            this.notesTextBox.TabIndex = 0;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.ReadOnly = true;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // catalogDataGridViewTextBoxColumn
            // 
            this.catalogDataGridViewTextBoxColumn.DataPropertyName = "catalog";
            this.catalogDataGridViewTextBoxColumn.HeaderText = "SKU";
            this.catalogDataGridViewTextBoxColumn.Name = "catalogDataGridViewTextBoxColumn";
            this.catalogDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Titel";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 300;
            // 
            // dateDataGridViewTextBoxColumn
            // 
            this.dateDataGridViewTextBoxColumn.DataPropertyName = "date";
            this.dateDataGridViewTextBoxColumn.HeaderText = "Erschienen";
            this.dateDataGridViewTextBoxColumn.Name = "dateDataGridViewTextBoxColumn";
            this.dateDataGridViewTextBoxColumn.ReadOnly = true;
            this.dateDataGridViewTextBoxColumn.Width = 75;
            // 
            // typeNameDataGridViewTextBoxColumn
            // 
            this.typeNameDataGridViewTextBoxColumn.DataPropertyName = "typeName";
            this.typeNameDataGridViewTextBoxColumn.HeaderText = "Typ";
            this.typeNameDataGridViewTextBoxColumn.Name = "typeNameDataGridViewTextBoxColumn";
            this.typeNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.typeNameDataGridViewTextBoxColumn.Width = 50;
            // 
            // classificationNameDataGridViewTextBoxColumn
            // 
            this.classificationNameDataGridViewTextBoxColumn.DataPropertyName = "classificationName";
            this.classificationNameDataGridViewTextBoxColumn.HeaderText = "Klassifikation";
            this.classificationNameDataGridViewTextBoxColumn.Name = "classificationNameDataGridViewTextBoxColumn";
            this.classificationNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mediaformatNameDataGridViewTextBoxColumn
            // 
            this.mediaformatNameDataGridViewTextBoxColumn.DataPropertyName = "mediaformatName";
            this.mediaformatNameDataGridViewTextBoxColumn.HeaderText = "Format";
            this.mediaformatNameDataGridViewTextBoxColumn.Name = "mediaformatNameDataGridViewTextBoxColumn";
            this.mediaformatNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.mediaformatNameDataGridViewTextBoxColumn.Width = 50;
            // 
            // publishformatNameDataGridViewTextBoxColumn
            // 
            this.publishformatNameDataGridViewTextBoxColumn.DataPropertyName = "publishformatName";
            this.publishformatNameDataGridViewTextBoxColumn.HeaderText = "Publishformat";
            this.publishformatNameDataGridViewTextBoxColumn.Name = "publishformatNameDataGridViewTextBoxColumn";
            this.publishformatNameDataGridViewTextBoxColumn.ReadOnly = true;
            this.publishformatNameDataGridViewTextBoxColumn.Width = 110;
            // 
            // albumListEntryBindingSource
            // 
            this.albumListEntryBindingSource.DataSource = typeof(AlbumListEntry);
            // 
            // galleria1
            // 
            this.galleria1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.galleria1.Location = new System.Drawing.Point(3, 3);
            this.galleria1.Name = "galleria1";
            this.galleria1.Size = new System.Drawing.Size(122, 158);
            this.galleria1.TabIndex = 0;
            // 
            // VgmDbControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "VgmDbControl";
            this.Size = new System.Drawing.Size(641, 351);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.propertyTabPage.ResumeLayout(false);
            this.galleriaTabPage.ResumeLayout(false);
            this.notesTabPage.ResumeLayout(false);
            this.notesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.albumListEntryBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.BindingSource albumListEntryBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn catalogDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn classificationNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mediaformatNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn publishformatNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage propertyTabPage;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabPage galleriaTabPage;
        private Galleria galleria1;
        private System.Windows.Forms.TabPage notesTabPage;
        private System.Windows.Forms.TextBox notesTextBox;
    }
}
