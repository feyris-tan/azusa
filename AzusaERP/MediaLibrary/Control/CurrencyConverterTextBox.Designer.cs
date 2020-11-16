namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    partial class CurrencyConverterTextBox
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jPYEURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uSDEURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gBPEURToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.ShortcutsEnabled = false;
            this.textBox1.Size = new System.Drawing.Size(188, 20);
            this.textBox1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jPYEURToolStripMenuItem,
            this.uSDEURToolStripMenuItem,
            this.gBPEURToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 70);
            // 
            // jPYEURToolStripMenuItem
            // 
            this.jPYEURToolStripMenuItem.Enabled = false;
            this.jPYEURToolStripMenuItem.Image = global::moe.yo3explorer.azusa.Properties.Resources.money_yen;
            this.jPYEURToolStripMenuItem.Name = "jPYEURToolStripMenuItem";
            this.jPYEURToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.jPYEURToolStripMenuItem.Text = "JPY -> EUR";
            this.jPYEURToolStripMenuItem.Click += new System.EventHandler(this.jPYEURToolStripMenuItem_Click);
            // 
            // uSDEURToolStripMenuItem
            // 
            this.uSDEURToolStripMenuItem.Enabled = false;
            this.uSDEURToolStripMenuItem.Image = global::moe.yo3explorer.azusa.Properties.Resources.money_dollar;
            this.uSDEURToolStripMenuItem.Name = "uSDEURToolStripMenuItem";
            this.uSDEURToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.uSDEURToolStripMenuItem.Text = "USD -> EUR";
            this.uSDEURToolStripMenuItem.Click += new System.EventHandler(this.uSDEURToolStripMenuItem_Click);
            // 
            // gBPEURToolStripMenuItem
            // 
            this.gBPEURToolStripMenuItem.Enabled = false;
            this.gBPEURToolStripMenuItem.Image = global::moe.yo3explorer.azusa.Properties.Resources.money_pound;
            this.gBPEURToolStripMenuItem.Name = "gBPEURToolStripMenuItem";
            this.gBPEURToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.gBPEURToolStripMenuItem.Text = "GBP -> EUR";
            this.gBPEURToolStripMenuItem.Click += new System.EventHandler(this.gBPEURToolStripMenuItem_Click);
            // 
            // CurrencyConverterTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.textBox1);
            this.Name = "CurrencyConverterTextBox";
            this.Size = new System.Drawing.Size(188, 19);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem jPYEURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uSDEURToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gBPEURToolStripMenuItem;
    }
}
