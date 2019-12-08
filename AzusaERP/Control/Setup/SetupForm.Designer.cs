namespace moe.yo3explorer.azusa.Control.Setup
{
    partial class SetupForm
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
            this.dateiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupBeendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.einstellungenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offlineModusErzwingenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hauptfensterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nachStartInDenVordergrundBringenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nachStartMaximierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.postgreSQLVerbindungToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offlinedatenbankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronisationErlaubenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lizensierungToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diesesGerätLizensierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fehlerdiagnoseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dateiToolStripMenuItem,
            this.einstellungenToolStripMenuItem,
            this.fehlerdiagnoseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(387, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dateiToolStripMenuItem
            // 
            this.dateiToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupBeendenToolStripMenuItem});
            this.dateiToolStripMenuItem.Name = "dateiToolStripMenuItem";
            this.dateiToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.dateiToolStripMenuItem.Text = "Datei";
            // 
            // setupBeendenToolStripMenuItem
            // 
            this.setupBeendenToolStripMenuItem.Name = "setupBeendenToolStripMenuItem";
            this.setupBeendenToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.setupBeendenToolStripMenuItem.Text = "Setup beenden";
            this.setupBeendenToolStripMenuItem.Click += new System.EventHandler(this.setupBeendenToolStripMenuItem_Click);
            // 
            // einstellungenToolStripMenuItem
            // 
            this.einstellungenToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.offlineModusErzwingenToolStripMenuItem,
            this.hauptfensterToolStripMenuItem,
            this.postgreSQLVerbindungToolStripMenuItem,
            this.offlinedatenbankToolStripMenuItem,
            this.lizensierungToolStripMenuItem});
            this.einstellungenToolStripMenuItem.Name = "einstellungenToolStripMenuItem";
            this.einstellungenToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.einstellungenToolStripMenuItem.Text = "Einstellungen";
            // 
            // offlineModusErzwingenToolStripMenuItem
            // 
            this.offlineModusErzwingenToolStripMenuItem.CheckOnClick = true;
            this.offlineModusErzwingenToolStripMenuItem.Name = "offlineModusErzwingenToolStripMenuItem";
            this.offlineModusErzwingenToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.offlineModusErzwingenToolStripMenuItem.Text = "Offline-Modus erzwingen";
            this.offlineModusErzwingenToolStripMenuItem.Click += new System.EventHandler(this.offlineModusErzwingenToolStripMenuItem_Click);
            // 
            // hauptfensterToolStripMenuItem
            // 
            this.hauptfensterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nachStartInDenVordergrundBringenToolStripMenuItem,
            this.nachStartMaximierenToolStripMenuItem});
            this.hauptfensterToolStripMenuItem.Name = "hauptfensterToolStripMenuItem";
            this.hauptfensterToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.hauptfensterToolStripMenuItem.Text = "Hauptfenster";
            // 
            // nachStartInDenVordergrundBringenToolStripMenuItem
            // 
            this.nachStartInDenVordergrundBringenToolStripMenuItem.CheckOnClick = true;
            this.nachStartInDenVordergrundBringenToolStripMenuItem.Name = "nachStartInDenVordergrundBringenToolStripMenuItem";
            this.nachStartInDenVordergrundBringenToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.nachStartInDenVordergrundBringenToolStripMenuItem.Text = "Nach Start in den Vordergrund bringen";
            this.nachStartInDenVordergrundBringenToolStripMenuItem.Click += new System.EventHandler(this.nachStartInDenVordergrundBringenToolStripMenuItem_Click);
            // 
            // nachStartMaximierenToolStripMenuItem
            // 
            this.nachStartMaximierenToolStripMenuItem.CheckOnClick = true;
            this.nachStartMaximierenToolStripMenuItem.Name = "nachStartMaximierenToolStripMenuItem";
            this.nachStartMaximierenToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.nachStartMaximierenToolStripMenuItem.Text = "Nach Start maximieren";
            this.nachStartMaximierenToolStripMenuItem.Click += new System.EventHandler(this.nachStartMaximierenToolStripMenuItem_Click);
            // 
            // postgreSQLVerbindungToolStripMenuItem
            // 
            this.postgreSQLVerbindungToolStripMenuItem.Name = "postgreSQLVerbindungToolStripMenuItem";
            this.postgreSQLVerbindungToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.postgreSQLVerbindungToolStripMenuItem.Text = "PostgreSQL-Konfiguration";
            this.postgreSQLVerbindungToolStripMenuItem.Click += new System.EventHandler(this.postgreSQLVerbindungToolStripMenuItem_Click);
            // 
            // offlinedatenbankToolStripMenuItem
            // 
            this.offlinedatenbankToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronisationErlaubenToolStripMenuItem,
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem});
            this.offlinedatenbankToolStripMenuItem.Name = "offlinedatenbankToolStripMenuItem";
            this.offlinedatenbankToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.offlinedatenbankToolStripMenuItem.Text = "Offlinedatenbank";
            // 
            // synchronisationErlaubenToolStripMenuItem
            // 
            this.synchronisationErlaubenToolStripMenuItem.CheckOnClick = true;
            this.synchronisationErlaubenToolStripMenuItem.Name = "synchronisationErlaubenToolStripMenuItem";
            this.synchronisationErlaubenToolStripMenuItem.Size = new System.Drawing.Size(329, 22);
            this.synchronisationErlaubenToolStripMenuItem.Text = "Synchronisation erlauben";
            this.synchronisationErlaubenToolStripMenuItem.Click += new System.EventHandler(this.synchronisationErlaubenToolStripMenuItem_Click);
            // 
            // jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem
            // 
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.CheckOnClick = true;
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Name = "jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem";
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Size = new System.Drawing.Size(329, 22);
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Text = "Jedes mal vollsynchronisieren (nicht empfohlen)";
            this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Click += new System.EventHandler(this.jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem_Click);
            // 
            // lizensierungToolStripMenuItem
            // 
            this.lizensierungToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diesesGerätLizensierenToolStripMenuItem});
            this.lizensierungToolStripMenuItem.Name = "lizensierungToolStripMenuItem";
            this.lizensierungToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.lizensierungToolStripMenuItem.Text = "Lizensierung";
            // 
            // diesesGerätLizensierenToolStripMenuItem
            // 
            this.diesesGerätLizensierenToolStripMenuItem.Name = "diesesGerätLizensierenToolStripMenuItem";
            this.diesesGerätLizensierenToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.diesesGerätLizensierenToolStripMenuItem.Text = "Dieses Gerät lizensieren";
            this.diesesGerätLizensierenToolStripMenuItem.Click += new System.EventHandler(this.diesesGerätLizensierenToolStripMenuItem_Click);
            // 
            // fehlerdiagnoseToolStripMenuItem
            // 
            this.fehlerdiagnoseToolStripMenuItem.Enabled = false;
            this.fehlerdiagnoseToolStripMenuItem.Name = "fehlerdiagnoseToolStripMenuItem";
            this.fehlerdiagnoseToolStripMenuItem.Size = new System.Drawing.Size(99, 20);
            this.fehlerdiagnoseToolStripMenuItem.Text = "Fehlerdiagnose";
            this.fehlerdiagnoseToolStripMenuItem.Click += new System.EventHandler(this.fehlerdiagnoseToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 23);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(387, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 45);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupForm";
            this.Text = "SetupForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dateiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupBeendenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem einstellungenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offlineModusErzwingenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hauptfensterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nachStartInDenVordergrundBringenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nachStartMaximierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem postgreSQLVerbindungToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem offlinedatenbankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronisationErlaubenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fehlerdiagnoseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lizensierungToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diesesGerätLizensierenToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}