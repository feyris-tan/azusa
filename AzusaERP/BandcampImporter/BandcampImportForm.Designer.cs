namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    partial class BandcampImportForm
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
            this.inputFolderTextBox = new System.Windows.Forms.TextBox();
            this.discographyCheckbox = new System.Windows.Forms.CheckBox();
            this.discographyNameTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.inputFolderOpenButton = new System.Windows.Forms.Button();
            this.inputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.outputFolderTextBox = new System.Windows.Forms.TextBox();
            this.outputFolderOpenButton = new System.Windows.Forms.Button();
            this.outputFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.shelfSelector1 = new moe.yo3explorer.azusa.MediaLibrary.Control.ShelfSelector();
            this.SuspendLayout();
            // 
            // inputFolderTextBox
            // 
            this.inputFolderTextBox.Location = new System.Drawing.Point(75, 40);
            this.inputFolderTextBox.Name = "inputFolderTextBox";
            this.inputFolderTextBox.ReadOnly = true;
            this.inputFolderTextBox.Size = new System.Drawing.Size(150, 20);
            this.inputFolderTextBox.TabIndex = 1;
            // 
            // discographyCheckbox
            // 
            this.discographyCheckbox.AutoSize = true;
            this.discographyCheckbox.Location = new System.Drawing.Point(75, 92);
            this.discographyCheckbox.Name = "discographyCheckbox";
            this.discographyCheckbox.Size = new System.Drawing.Size(148, 17);
            this.discographyCheckbox.TabIndex = 2;
            this.discographyCheckbox.Text = "Dies ist eine Diskographie";
            this.discographyCheckbox.UseVisualStyleBackColor = true;
            this.discographyCheckbox.CheckedChanged += new System.EventHandler(this.discographyCheckbox_CheckedChanged);
            // 
            // discographyNameTextBox
            // 
            this.discographyNameTextBox.Enabled = false;
            this.discographyNameTextBox.Location = new System.Drawing.Point(75, 115);
            this.discographyNameTextBox.Name = "discographyNameTextBox";
            this.discographyNameTextBox.Size = new System.Drawing.Size(148, 20);
            this.discographyNameTextBox.TabIndex = 3;
            this.discographyNameTextBox.TextChanged += new System.EventHandler(this.discographyNameTextBox_TextChanged);
            // 
            // goButton
            // 
            this.goButton.Enabled = false;
            this.goButton.Location = new System.Drawing.Point(231, 115);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(75, 23);
            this.goButton.TabIndex = 4;
            this.goButton.Text = "Start";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // inputFolderOpenButton
            // 
            this.inputFolderOpenButton.Image = global::moe.yo3explorer.azusa.Properties.Resources.openfolderHS;
            this.inputFolderOpenButton.Location = new System.Drawing.Point(231, 38);
            this.inputFolderOpenButton.Name = "inputFolderOpenButton";
            this.inputFolderOpenButton.Size = new System.Drawing.Size(75, 23);
            this.inputFolderOpenButton.TabIndex = 5;
            this.inputFolderOpenButton.UseVisualStyleBackColor = true;
            this.inputFolderOpenButton.Click += new System.EventHandler(this.openFolderButton_Click);
            // 
            // outputFolderTextBox
            // 
            this.outputFolderTextBox.Location = new System.Drawing.Point(75, 66);
            this.outputFolderTextBox.Name = "outputFolderTextBox";
            this.outputFolderTextBox.ReadOnly = true;
            this.outputFolderTextBox.Size = new System.Drawing.Size(150, 20);
            this.outputFolderTextBox.TabIndex = 6;
            // 
            // outputFolderOpenButton
            // 
            this.outputFolderOpenButton.Image = global::moe.yo3explorer.azusa.Properties.Resources.openfolderHS;
            this.outputFolderOpenButton.Location = new System.Drawing.Point(231, 66);
            this.outputFolderOpenButton.Name = "outputFolderOpenButton";
            this.outputFolderOpenButton.Size = new System.Drawing.Size(75, 23);
            this.outputFolderOpenButton.TabIndex = 7;
            this.outputFolderOpenButton.UseVisualStyleBackColor = true;
            this.outputFolderOpenButton.Click += new System.EventHandler(this.outputFolderOpenButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Regal:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Quellordner:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Zielordner:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-5, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Künstlername:";
            // 
            // shelfSelector1
            // 
            this.shelfSelector1.Location = new System.Drawing.Point(75, 12);
            this.shelfSelector1.Name = "shelfSelector1";
            this.shelfSelector1.SelectedIndex = 0;
            this.shelfSelector1.Size = new System.Drawing.Size(150, 22);
            this.shelfSelector1.TabIndex = 0;
            // 
            // BandcampImportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 145);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputFolderOpenButton);
            this.Controls.Add(this.outputFolderTextBox);
            this.Controls.Add(this.inputFolderOpenButton);
            this.Controls.Add(this.goButton);
            this.Controls.Add(this.discographyNameTextBox);
            this.Controls.Add(this.discographyCheckbox);
            this.Controls.Add(this.inputFolderTextBox);
            this.Controls.Add(this.shelfSelector1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "BandcampImportForm";
            this.Text = "BandcampImportForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Control.ShelfSelector shelfSelector1;
        private System.Windows.Forms.TextBox inputFolderTextBox;
        private System.Windows.Forms.CheckBox discographyCheckbox;
        private System.Windows.Forms.TextBox discographyNameTextBox;
        private System.Windows.Forms.Button goButton;
        private System.Windows.Forms.Button inputFolderOpenButton;
        private System.Windows.Forms.FolderBrowserDialog inputFolderBrowserDialog;
        private System.Windows.Forms.TextBox outputFolderTextBox;
        private System.Windows.Forms.Button outputFolderOpenButton;
        private System.Windows.Forms.FolderBrowserDialog outputFolderBrowserDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}