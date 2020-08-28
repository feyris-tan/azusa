namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    partial class SedgeTreeControl
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.speichernToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personenVerwaltenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neuePersonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personBearbeitenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personLöschenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baumansichtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datenbankÜberprüfenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 240);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(465, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.speichernToolStripMenuItem,
            this.personenVerwaltenToolStripMenuItem,
            this.baumansichtToolStripMenuItem,
            this.datenbankÜberprüfenToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(465, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // speichernToolStripMenuItem
            // 
            this.speichernToolStripMenuItem.Enabled = false;
            this.speichernToolStripMenuItem.Name = "speichernToolStripMenuItem";
            this.speichernToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.speichernToolStripMenuItem.Text = "Speichern";
            this.speichernToolStripMenuItem.Click += new System.EventHandler(this.speichernToolStripMenuItem_Click);
            // 
            // personenVerwaltenToolStripMenuItem
            // 
            this.personenVerwaltenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuePersonToolStripMenuItem,
            this.personBearbeitenToolStripMenuItem,
            this.personLöschenToolStripMenuItem});
            this.personenVerwaltenToolStripMenuItem.Name = "personenVerwaltenToolStripMenuItem";
            this.personenVerwaltenToolStripMenuItem.Size = new System.Drawing.Size(115, 20);
            this.personenVerwaltenToolStripMenuItem.Text = "Personen verwalten";
            // 
            // neuePersonToolStripMenuItem
            // 
            this.neuePersonToolStripMenuItem.Name = "neuePersonToolStripMenuItem";
            this.neuePersonToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.neuePersonToolStripMenuItem.Text = "Neue Person";
            this.neuePersonToolStripMenuItem.Click += new System.EventHandler(this.neuePersonToolStripMenuItem_Click);
            // 
            // personBearbeitenToolStripMenuItem
            // 
            this.personBearbeitenToolStripMenuItem.Name = "personBearbeitenToolStripMenuItem";
            this.personBearbeitenToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.personBearbeitenToolStripMenuItem.Text = "Person bearbeiten";
            this.personBearbeitenToolStripMenuItem.Click += new System.EventHandler(this.personBearbeitenToolStripMenuItem_Click);
            // 
            // personLöschenToolStripMenuItem
            // 
            this.personLöschenToolStripMenuItem.Name = "personLöschenToolStripMenuItem";
            this.personLöschenToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.personLöschenToolStripMenuItem.Text = "Person löschen";
            this.personLöschenToolStripMenuItem.Click += new System.EventHandler(this.personLöschenToolStripMenuItem_Click);
            // 
            // baumansichtToolStripMenuItem
            // 
            this.baumansichtToolStripMenuItem.Name = "baumansichtToolStripMenuItem";
            this.baumansichtToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.baumansichtToolStripMenuItem.Text = "Baumansicht";
            this.baumansichtToolStripMenuItem.Click += new System.EventHandler(this.baumansichtToolStripMenuItem_Click);
            // 
            // datenbankÜberprüfenToolStripMenuItem
            // 
            this.datenbankÜberprüfenToolStripMenuItem.Name = "datenbankÜberprüfenToolStripMenuItem";
            this.datenbankÜberprüfenToolStripMenuItem.Size = new System.Drawing.Size(128, 20);
            this.datenbankÜberprüfenToolStripMenuItem.Text = "Datenbank überprüfen";
            this.datenbankÜberprüfenToolStripMenuItem.Click += new System.EventHandler(this.datenbankÜberprüfenToolStripMenuItem_Click);
            // 
            // SedgeTreeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "SedgeTreeControl";
            this.Size = new System.Drawing.Size(465, 262);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem speichernToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personenVerwaltenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neuePersonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personBearbeitenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personLöschenToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem baumansichtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datenbankÜberprüfenToolStripMenuItem;

        #endregion
    }
}
