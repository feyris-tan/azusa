namespace moe.yo3explorer.azusa.DexcomHistory.Boundary
{
    partial class ManualDataEntires
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dateiImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Datenbankzeitstempel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zeitstempel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Messwert = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Einheit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Novorapid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Levemir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Versteckt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zeitkorrektur = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notiz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiImportierenToolStripMenuItem,
            this.beendenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(643, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiImportierenToolStripMenuItem
            // 
            this.dateiImportierenToolStripMenuItem.Name = "dateiImportierenToolStripMenuItem";
            this.dateiImportierenToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.dateiImportierenToolStripMenuItem.Text = "Datei importieren";
            this.dateiImportierenToolStripMenuItem.Click += new System.EventHandler(this.dateiImportierenToolStripMenuItem_Click);
            // 
            // beendenToolStripMenuItem
            // 
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.beendenToolStripMenuItem.Text = "Beenden";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.Datenbankzeitstempel,
            this.Zeitstempel,
            this.Messwert,
            this.Einheit,
            this.BE,
            this.Novorapid,
            this.Levemir,
            this.Versteckt,
            this.Zeitkorrektur,
            this.Notiz});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(643, 288);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // id
            // 
            this.id.HeaderText = "Datenbankschlüssel";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // Datenbankzeitstempel
            // 
            this.Datenbankzeitstempel.HeaderText = "Datenbankzeitstempel";
            this.Datenbankzeitstempel.Name = "Datenbankzeitstempel";
            this.Datenbankzeitstempel.ReadOnly = true;
            this.Datenbankzeitstempel.Visible = false;
            // 
            // Zeitstempel
            // 
            this.Zeitstempel.HeaderText = "Zeitstempel";
            this.Zeitstempel.Name = "Zeitstempel";
            this.Zeitstempel.ReadOnly = true;
            this.Zeitstempel.Width = 150;
            // 
            // Messwert
            // 
            this.Messwert.HeaderText = "Messwert";
            this.Messwert.Name = "Messwert";
            this.Messwert.ReadOnly = true;
            this.Messwert.Width = 70;
            // 
            // Einheit
            // 
            this.Einheit.HeaderText = "Einheit";
            this.Einheit.Name = "Einheit";
            this.Einheit.ReadOnly = true;
            this.Einheit.Width = 50;
            // 
            // BE
            // 
            this.BE.HeaderText = "BE";
            this.BE.Name = "BE";
            this.BE.Width = 40;
            // 
            // Novorapid
            // 
            this.Novorapid.HeaderText = "Novorapid";
            this.Novorapid.Name = "Novorapid";
            this.Novorapid.Width = 70;
            // 
            // Levemir
            // 
            this.Levemir.HeaderText = "Levemir";
            this.Levemir.Name = "Levemir";
            this.Levemir.Width = 70;
            // 
            // Versteckt
            // 
            this.Versteckt.HeaderText = "Versteckt";
            this.Versteckt.Name = "Versteckt";
            this.Versteckt.ReadOnly = true;
            this.Versteckt.Visible = false;
            // 
            // Zeitkorrektur
            // 
            this.Zeitkorrektur.HeaderText = "Zeitkorrektur";
            this.Zeitkorrektur.Name = "Zeitkorrektur";
            this.Zeitkorrektur.ReadOnly = true;
            this.Zeitkorrektur.Visible = false;
            // 
            // Notiz
            // 
            this.Notiz.HeaderText = "Notiz";
            this.Notiz.MaxInputLength = 250;
            this.Notiz.Name = "Notiz";
            this.Notiz.Width = 130;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "diary.csv";
            this.openFileDialog1.Filter = "Accu-Check Aviva (diary.csv)|diary.csv";
            // 
            // ManualDataEntires
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 312);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ManualDataEntires";
            this.Text = "Aviva Connect Utility";
            this.Load += new System.EventHandler(this.ManualDataEntires_Load);
            this.Shown += new System.EventHandler(this.ManualDataEntires_Shown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem dateiImportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Datenbankzeitstempel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zeitstempel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Messwert;
        private System.Windows.Forms.DataGridViewTextBoxColumn Einheit;
        private System.Windows.Forms.DataGridViewTextBoxColumn BE;
        private System.Windows.Forms.DataGridViewTextBoxColumn Novorapid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Levemir;
        private System.Windows.Forms.DataGridViewTextBoxColumn Versteckt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zeitkorrektur;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notiz;
    }
}