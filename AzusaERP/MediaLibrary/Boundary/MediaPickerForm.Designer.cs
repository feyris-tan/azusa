namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    partial class MediaPickerForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shelfSelector1 = new moe.yo3explorer.azusa.MediaLibrary.Control.ShelfSelector();
            this.label1 = new System.Windows.Forms.Label();
            this.productComboBox = new System.Windows.Forms.ComboBox();
            this.productAddButton = new System.Windows.Forms.Button();
            this.addMediaButton = new System.Windows.Forms.Button();
            this.mediaComboBox = new System.Windows.Forms.ComboBox();
            this.confirm = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.shelfSelector1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.productComboBox, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.productAddButton, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.addMediaButton, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.mediaComboBox, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.confirm, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 126);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Medium";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Produkt:";
            // 
            // shelfSelector1
            // 
            this.shelfSelector1.Location = new System.Drawing.Point(103, 3);
            this.shelfSelector1.Name = "shelfSelector1";
            this.shelfSelector1.SelectedIndex = 0;
            this.shelfSelector1.Size = new System.Drawing.Size(175, 22);
            this.shelfSelector1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Regal:";
            // 
            // productComboBox
            // 
            this.productComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productComboBox.Enabled = false;
            this.productComboBox.FormattingEnabled = true;
            this.productComboBox.Location = new System.Drawing.Point(103, 35);
            this.productComboBox.Name = "productComboBox";
            this.productComboBox.Size = new System.Drawing.Size(175, 21);
            this.productComboBox.TabIndex = 3;
            this.productComboBox.SelectedValueChanged += new System.EventHandler(this.productComboBox_SelectedValueChanged);
            // 
            // productAddButton
            // 
            this.productAddButton.Enabled = false;
            this.productAddButton.Image = global::moe.yo3explorer.azusa.Properties.Resources.add;
            this.productAddButton.Location = new System.Drawing.Point(289, 35);
            this.productAddButton.Name = "productAddButton";
            this.productAddButton.Size = new System.Drawing.Size(26, 23);
            this.productAddButton.TabIndex = 5;
            this.productAddButton.UseVisualStyleBackColor = true;
            // 
            // addMediaButton
            // 
            this.addMediaButton.Enabled = false;
            this.addMediaButton.Image = global::moe.yo3explorer.azusa.Properties.Resources.add;
            this.addMediaButton.Location = new System.Drawing.Point(289, 67);
            this.addMediaButton.Name = "addMediaButton";
            this.addMediaButton.Size = new System.Drawing.Size(26, 23);
            this.addMediaButton.TabIndex = 6;
            this.addMediaButton.UseVisualStyleBackColor = true;
            // 
            // mediaComboBox
            // 
            this.mediaComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mediaComboBox.Enabled = false;
            this.mediaComboBox.FormattingEnabled = true;
            this.mediaComboBox.Location = new System.Drawing.Point(103, 67);
            this.mediaComboBox.Name = "mediaComboBox";
            this.mediaComboBox.Size = new System.Drawing.Size(175, 21);
            this.mediaComboBox.TabIndex = 7;
            this.mediaComboBox.SelectedIndexChanged += new System.EventHandler(this.mediaComboBox_SelectedIndexChanged);
            // 
            // confirm
            // 
            this.confirm.Enabled = false;
            this.confirm.Image = global::moe.yo3explorer.azusa.Properties.Resources.accept;
            this.confirm.Location = new System.Drawing.Point(289, 99);
            this.confirm.Name = "confirm";
            this.confirm.Size = new System.Drawing.Size(26, 23);
            this.confirm.TabIndex = 8;
            this.confirm.UseVisualStyleBackColor = true;
            this.confirm.Click += new System.EventHandler(this.confirm_Click);
            // 
            // MediaPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 126);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MediaPickerForm";
            this.Text = "MediaPickerForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private Control.ShelfSelector shelfSelector1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox productComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button productAddButton;
        private System.Windows.Forms.Button addMediaButton;
        private System.Windows.Forms.ComboBox mediaComboBox;
        private System.Windows.Forms.Button confirm;
    }
}