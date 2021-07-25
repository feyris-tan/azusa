using moe.yo3explorer.azusa.MediaLibrary.Control;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    partial class MediaLibraryControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaLibraryControl));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.neuesProduktToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bandcampKollektionImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defekteM3UDateienReparierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metafilesAutomatischErgänzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageOrdnerImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pSXISOBatchImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dVDBoxImportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vervollständigkeitsassistentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allesVervollständigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regalVervollständigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.produktVervollständigenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.productsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.productTabMainPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.productName = new System.Windows.Forms.TextBox();
            this.productComplete = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.productPurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.productISBN = new System.Windows.Forms.TextBox();
            this.productSave = new System.Windows.Forms.Button();
            this.productCost = new moe.yo3explorer.azusa.MediaLibrary.Control.CurrencyConverterTextBox();
            this.productNSFW = new System.Windows.Forms.CheckBox();
            this.productTabCover = new System.Windows.Forms.TabPage();
            this.productCover = new moe.yo3explorer.azusa.MediaLibrary.Control.PictureDrop();
            this.productTabMedia = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.productMediaListView = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.productAddMediaButton = new System.Windows.Forms.Button();
            this.productRemoveMediaButton = new System.Windows.Forms.Button();
            this.productTabScreenshots = new System.Windows.Forms.TabPage();
            this.productScreenshot = new moe.yo3explorer.azusa.MediaLibrary.Control.PictureDrop();
            this.productTabLinks = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.productPlatform = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.productSupplier = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.productCountryOfOrigin = new System.Windows.Forms.ComboBox();
            this.mediaTabPages = new System.Windows.Forms.TabControl();
            this.mediaTabInfo = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.label11 = new System.Windows.Forms.Label();
            this.mediaName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.mediaType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mediaSku = new System.Windows.Forms.TextBox();
            this.mediaStillSealed = new System.Windows.Forms.CheckBox();
            this.mediaStorageSpaceId = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.mediaDumpPath = new System.Windows.Forms.TextBox();
            this.mediaMoreOptionsButton = new System.Windows.Forms.Button();
            this.mediaSave = new System.Windows.Forms.Button();
            this.mediaTabMetadata = new System.Windows.Forms.TabPage();
            this.mediaMetadata = new System.Windows.Forms.TextBox();
            this.mediaTabGraphdata = new System.Windows.Forms.TabPage();
            this.graphDataControl1 = new moe.yo3explorer.azusa.MediaLibrary.Control.GraphDataControl();
            this.graphDataContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.automatischLadenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaTabFilesystem = new System.Windows.Forms.TabPage();
            this.mediaFilesystemTreeView = new System.Windows.Forms.TreeView();
            this.filesystemContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.imageParsenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.discScannenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesystemImageList = new System.Windows.Forms.ImageList(this.components);
            this.mediaMoreOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setMetadataAndDump = new System.Windows.Forms.ToolStripMenuItem();
            this.öffneDumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadatenExportierenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m3UEditorÖffnenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.weitereAnhängeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metadataAndDumpOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.dumpOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.filesystemIsoParseOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.productTabMainPage.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.productTabCover.SuspendLayout();
            this.productTabMedia.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.productTabScreenshots.SuspendLayout();
            this.productTabLinks.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.mediaTabPages.SuspendLayout();
            this.mediaTabInfo.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mediaStorageSpaceId)).BeginInit();
            this.mediaTabMetadata.SuspendLayout();
            this.mediaTabGraphdata.SuspendLayout();
            this.graphDataContextMenuStrip.SuspendLayout();
            this.mediaTabFilesystem.SuspendLayout();
            this.filesystemContextMenuStrip.SuspendLayout();
            this.mediaMoreOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.neuesProduktToolStripMenuItem,
            this.batchImportToolStripMenuItem,
            this.vervollständigkeitsassistentToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(866, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // neuesProduktToolStripMenuItem
            // 
            this.neuesProduktToolStripMenuItem.Name = "neuesProduktToolStripMenuItem";
            this.neuesProduktToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.neuesProduktToolStripMenuItem.Text = "Neues Produkt";
            this.neuesProduktToolStripMenuItem.Click += new System.EventHandler(this.neuesProduktToolStripMenuItem_Click);
            // 
            // batchImportToolStripMenuItem
            // 
            this.batchImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bandcampKollektionImportierenToolStripMenuItem,
            this.defekteM3UDateienReparierenToolStripMenuItem,
            this.metafilesAutomatischErgänzenToolStripMenuItem,
            this.imageOrdnerImportierenToolStripMenuItem,
            this.pSXISOBatchImportToolStripMenuItem,
            this.dVDBoxImportierenToolStripMenuItem});
            this.batchImportToolStripMenuItem.Name = "batchImportToolStripMenuItem";
            this.batchImportToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.batchImportToolStripMenuItem.Text = "Import";
            // 
            // bandcampKollektionImportierenToolStripMenuItem
            // 
            this.bandcampKollektionImportierenToolStripMenuItem.Name = "bandcampKollektionImportierenToolStripMenuItem";
            this.bandcampKollektionImportierenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.bandcampKollektionImportierenToolStripMenuItem.Text = "Bandcamp Kollektion importieren";
            this.bandcampKollektionImportierenToolStripMenuItem.Click += new System.EventHandler(this.bandcampKollektionImportierenToolStripMenuItem_Click);
            // 
            // defekteM3UDateienReparierenToolStripMenuItem
            // 
            this.defekteM3UDateienReparierenToolStripMenuItem.Name = "defekteM3UDateienReparierenToolStripMenuItem";
            this.defekteM3UDateienReparierenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.defekteM3UDateienReparierenToolStripMenuItem.Text = "Defekte M3U Dateien reparieren";
            this.defekteM3UDateienReparierenToolStripMenuItem.Click += new System.EventHandler(this.defekteM3UDateienReparierenToolStripMenuItem_Click);
            // 
            // metafilesAutomatischErgänzenToolStripMenuItem
            // 
            this.metafilesAutomatischErgänzenToolStripMenuItem.Name = "metafilesAutomatischErgänzenToolStripMenuItem";
            this.metafilesAutomatischErgänzenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.metafilesAutomatischErgänzenToolStripMenuItem.Text = "Metafiles automatisch ergänzen";
            this.metafilesAutomatischErgänzenToolStripMenuItem.Click += new System.EventHandler(this.metafilesAutomatischErgänzenToolStripMenuItem_Click);
            // 
            // imageOrdnerImportierenToolStripMenuItem
            // 
            this.imageOrdnerImportierenToolStripMenuItem.Name = "imageOrdnerImportierenToolStripMenuItem";
            this.imageOrdnerImportierenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.imageOrdnerImportierenToolStripMenuItem.Text = "Image Ordner importieren";
            this.imageOrdnerImportierenToolStripMenuItem.Click += new System.EventHandler(this.imageOrdnerImportierenToolStripMenuItem_Click);
            // 
            // pSXISOBatchImportToolStripMenuItem
            // 
            this.pSXISOBatchImportToolStripMenuItem.Name = "pSXISOBatchImportToolStripMenuItem";
            this.pSXISOBatchImportToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.pSXISOBatchImportToolStripMenuItem.Text = "PSXISO Batch Import";
            this.pSXISOBatchImportToolStripMenuItem.Click += new System.EventHandler(this.pSXISOBatchImportToolStripMenuItem_Click);
            // 
            // dVDBoxImportierenToolStripMenuItem
            // 
            this.dVDBoxImportierenToolStripMenuItem.Name = "dVDBoxImportierenToolStripMenuItem";
            this.dVDBoxImportierenToolStripMenuItem.Size = new System.Drawing.Size(252, 22);
            this.dVDBoxImportierenToolStripMenuItem.Text = "DVD-Box importieren";
            this.dVDBoxImportierenToolStripMenuItem.Click += new System.EventHandler(this.dVDBoxImportierenToolStripMenuItem_Click);
            // 
            // vervollständigkeitsassistentToolStripMenuItem
            // 
            this.vervollständigkeitsassistentToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allesVervollständigenToolStripMenuItem,
            this.regalVervollständigenToolStripMenuItem,
            this.produktVervollständigenToolStripMenuItem});
            this.vervollständigkeitsassistentToolStripMenuItem.Name = "vervollständigkeitsassistentToolStripMenuItem";
            this.vervollständigkeitsassistentToolStripMenuItem.Size = new System.Drawing.Size(164, 20);
            this.vervollständigkeitsassistentToolStripMenuItem.Text = "Vervollständigungsassistent";
            // 
            // allesVervollständigenToolStripMenuItem
            // 
            this.allesVervollständigenToolStripMenuItem.Name = "allesVervollständigenToolStripMenuItem";
            this.allesVervollständigenToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.allesVervollständigenToolStripMenuItem.Text = "alles vervollständigen";
            this.allesVervollständigenToolStripMenuItem.Click += new System.EventHandler(this.allesVervollständigen_Click);
            // 
            // regalVervollständigenToolStripMenuItem
            // 
            this.regalVervollständigenToolStripMenuItem.Name = "regalVervollständigenToolStripMenuItem";
            this.regalVervollständigenToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.regalVervollständigenToolStripMenuItem.Text = "Regal vervollständigen";
            this.regalVervollständigenToolStripMenuItem.Click += new System.EventHandler(this.regalVervollständigen_Click);
            // 
            // produktVervollständigenToolStripMenuItem
            // 
            this.produktVervollständigenToolStripMenuItem.Name = "produktVervollständigenToolStripMenuItem";
            this.produktVervollständigenToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.produktVervollständigenToolStripMenuItem.Text = "Produkt vervollständigen";
            this.produktVervollständigenToolStripMenuItem.Click += new System.EventHandler(this.produktVervollständigen_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(866, 395);
            this.splitContainer1.SplitterDistance = 667;
            this.splitContainer1.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.productsListView, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(667, 395);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // productsListView
            // 
            this.productsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader4,
            this.columnHeader3,
            this.columnHeader5});
            this.productsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productsListView.HideSelection = false;
            this.productsListView.Location = new System.Drawing.Point(3, 31);
            this.productsListView.MultiSelect = false;
            this.productsListView.Name = "productsListView";
            this.productsListView.Size = new System.Drawing.Size(661, 361);
            this.productsListView.SmallImageList = this.imageList1;
            this.productsListView.StateImageList = this.imageList1;
            this.productsListView.TabIndex = 4;
            this.productsListView.UseCompatibleStateImageBehavior = false;
            this.productsListView.View = System.Windows.Forms.View.Details;
            this.productsListView.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Titel";
            this.columnHeader1.Width = 400;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Preis";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "# Discs";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Kaufdatum";
            this.columnHeader3.Width = 70;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Mängel";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(661, 22);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tabControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.mediaTabPages);
            this.splitContainer2.Size = new System.Drawing.Size(195, 395);
            this.splitContainer2.SplitterDistance = 192;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.AllowDrop = true;
            this.tabControl2.Controls.Add(this.productTabMainPage);
            this.tabControl2.Controls.Add(this.productTabCover);
            this.tabControl2.Controls.Add(this.productTabMedia);
            this.tabControl2.Controls.Add(this.productTabScreenshots);
            this.tabControl2.Controls.Add(this.productTabLinks);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(195, 192);
            this.tabControl2.TabIndex = 0;
            // 
            // productTabMainPage
            // 
            this.productTabMainPage.Controls.Add(this.tableLayoutPanel2);
            this.productTabMainPage.Location = new System.Drawing.Point(4, 22);
            this.productTabMainPage.Name = "productTabMainPage";
            this.productTabMainPage.Padding = new System.Windows.Forms.Padding(3);
            this.productTabMainPage.Size = new System.Drawing.Size(187, 166);
            this.productTabMainPage.TabIndex = 0;
            this.productTabMainPage.Text = "Info";
            this.productTabMainPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.productName, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.productComplete, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.productPurchaseDate, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.productISBN, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.productSave, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.productCost, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.productNSFW, 1, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.44001F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.44001F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.7931F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.44001F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.29562F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.29562F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.29562F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(181, 160);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // productName
            // 
            this.productName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productName.Enabled = false;
            this.productName.Location = new System.Drawing.Point(73, 3);
            this.productName.Name = "productName";
            this.productName.Size = new System.Drawing.Size(105, 20);
            this.productName.TabIndex = 1;
            // 
            // productComplete
            // 
            this.productComplete.AutoSize = true;
            this.productComplete.Enabled = false;
            this.productComplete.Location = new System.Drawing.Point(73, 26);
            this.productComplete.Name = "productComplete";
            this.productComplete.Size = new System.Drawing.Size(77, 17);
            this.productComplete.TabIndex = 2;
            this.productComplete.Text = "Vollständig";
            this.productComplete.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Preis:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Kaufdatum:";
            // 
            // productPurchaseDate
            // 
            this.productPurchaseDate.Enabled = false;
            this.productPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.productPurchaseDate.Location = new System.Drawing.Point(73, 94);
            this.productPurchaseDate.Name = "productPurchaseDate";
            this.productPurchaseDate.Size = new System.Drawing.Size(97, 20);
            this.productPurchaseDate.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "EAN/JAN:";
            // 
            // productISBN
            // 
            this.productISBN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productISBN.Enabled = false;
            this.productISBN.Location = new System.Drawing.Point(73, 116);
            this.productISBN.Name = "productISBN";
            this.productISBN.Size = new System.Drawing.Size(105, 20);
            this.productISBN.TabIndex = 9;
            // 
            // productSave
            // 
            this.productSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productSave.Enabled = false;
            this.productSave.Location = new System.Drawing.Point(73, 138);
            this.productSave.Name = "productSave";
            this.productSave.Size = new System.Drawing.Size(105, 19);
            this.productSave.TabIndex = 10;
            this.productSave.Text = "Speichern";
            this.productSave.UseVisualStyleBackColor = true;
            this.productSave.Click += new System.EventHandler(this.productSave_Click);
            // 
            // productCost
            // 
            this.productCost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productCost.Location = new System.Drawing.Point(73, 71);
            this.productCost.Name = "productCost";
            this.productCost.Size = new System.Drawing.Size(105, 17);
            this.productCost.TabIndex = 11;
            this.productCost.Value = 0D;
            // 
            // productNSFW
            // 
            this.productNSFW.AutoSize = true;
            this.productNSFW.Location = new System.Drawing.Point(73, 49);
            this.productNSFW.Name = "productNSFW";
            this.productNSFW.Size = new System.Drawing.Size(58, 16);
            this.productNSFW.TabIndex = 12;
            this.productNSFW.Text = "NSFW";
            this.productNSFW.UseVisualStyleBackColor = true;
            // 
            // productTabCover
            // 
            this.productTabCover.AllowDrop = true;
            this.productTabCover.Controls.Add(this.productCover);
            this.productTabCover.Location = new System.Drawing.Point(4, 22);
            this.productTabCover.Name = "productTabCover";
            this.productTabCover.Size = new System.Drawing.Size(187, 166);
            this.productTabCover.TabIndex = 2;
            this.productTabCover.Text = "Cover";
            this.productTabCover.UseVisualStyleBackColor = true;
            // 
            // productCover
            // 
            this.productCover.AllowDrop = true;
            this.productCover.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.productCover.Data = null;
            this.productCover.DataChanged = true;
            this.productCover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productCover.Location = new System.Drawing.Point(0, 0);
            this.productCover.MediumId = 0;
            this.productCover.Name = "productCover";
            this.productCover.Size = new System.Drawing.Size(187, 166);
            this.productCover.TabIndex = 0;
            // 
            // productTabMedia
            // 
            this.productTabMedia.Controls.Add(this.tableLayoutPanel5);
            this.productTabMedia.Location = new System.Drawing.Point(4, 22);
            this.productTabMedia.Name = "productTabMedia";
            this.productTabMedia.Size = new System.Drawing.Size(187, 166);
            this.productTabMedia.TabIndex = 5;
            this.productTabMedia.Text = "Medien";
            this.productTabMedia.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.productMediaListView, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(187, 166);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // productMediaListView
            // 
            this.productMediaListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6});
            this.productMediaListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productMediaListView.HideSelection = false;
            this.productMediaListView.Location = new System.Drawing.Point(3, 3);
            this.productMediaListView.MultiSelect = false;
            this.productMediaListView.Name = "productMediaListView";
            this.productMediaListView.Size = new System.Drawing.Size(181, 125);
            this.productMediaListView.SmallImageList = this.imageList1;
            this.productMediaListView.StateImageList = this.imageList1;
            this.productMediaListView.TabIndex = 1;
            this.productMediaListView.UseCompatibleStateImageBehavior = false;
            this.productMediaListView.View = System.Windows.Forms.View.List;
            this.productMediaListView.SelectedIndexChanged += new System.EventHandler(this.productMediaListView_SelectedIndexChanged);
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Name";
            this.columnHeader6.Width = 250;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel6.Controls.Add(this.productAddMediaButton, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.productRemoveMediaButton, 0, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 134);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(181, 29);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // productAddMediaButton
            // 
            this.productAddMediaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productAddMediaButton.Enabled = false;
            this.productAddMediaButton.Location = new System.Drawing.Point(48, 3);
            this.productAddMediaButton.Name = "productAddMediaButton";
            this.productAddMediaButton.Size = new System.Drawing.Size(39, 23);
            this.productAddMediaButton.TabIndex = 0;
            this.productAddMediaButton.Text = "Hinzufügen";
            this.productAddMediaButton.UseVisualStyleBackColor = true;
            this.productAddMediaButton.Click += new System.EventHandler(this.addMediaButton_Click);
            // 
            // productRemoveMediaButton
            // 
            this.productRemoveMediaButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productRemoveMediaButton.Enabled = false;
            this.productRemoveMediaButton.Location = new System.Drawing.Point(3, 3);
            this.productRemoveMediaButton.Name = "productRemoveMediaButton";
            this.productRemoveMediaButton.Size = new System.Drawing.Size(39, 23);
            this.productRemoveMediaButton.TabIndex = 1;
            this.productRemoveMediaButton.Text = "Entfernen";
            this.productRemoveMediaButton.UseVisualStyleBackColor = true;
            this.productRemoveMediaButton.Click += new System.EventHandler(this.productRemoveMediaButton_Click);
            // 
            // productTabScreenshots
            // 
            this.productTabScreenshots.AllowDrop = true;
            this.productTabScreenshots.Controls.Add(this.productScreenshot);
            this.productTabScreenshots.Location = new System.Drawing.Point(4, 22);
            this.productTabScreenshots.Name = "productTabScreenshots";
            this.productTabScreenshots.Size = new System.Drawing.Size(187, 166);
            this.productTabScreenshots.TabIndex = 4;
            this.productTabScreenshots.Text = "Screenshot";
            this.productTabScreenshots.UseVisualStyleBackColor = true;
            // 
            // productScreenshot
            // 
            this.productScreenshot.AllowDrop = true;
            this.productScreenshot.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.productScreenshot.Data = null;
            this.productScreenshot.DataChanged = true;
            this.productScreenshot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productScreenshot.Location = new System.Drawing.Point(0, 0);
            this.productScreenshot.MediumId = 0;
            this.productScreenshot.Name = "productScreenshot";
            this.productScreenshot.Size = new System.Drawing.Size(187, 166);
            this.productScreenshot.TabIndex = 0;
            // 
            // productTabLinks
            // 
            this.productTabLinks.Controls.Add(this.tableLayoutPanel4);
            this.productTabLinks.Location = new System.Drawing.Point(4, 22);
            this.productTabLinks.Name = "productTabLinks";
            this.productTabLinks.Size = new System.Drawing.Size(187, 166);
            this.productTabLinks.TabIndex = 3;
            this.productTabLinks.Text = "Verknüpfungen";
            this.productTabLinks.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.36871F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53.63129F));
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.productPlatform, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.productSupplier, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.productCountryOfOrigin, 1, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(187, 166);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Plattform:";
            // 
            // productPlatform
            // 
            this.productPlatform.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productPlatform.Enabled = false;
            this.productPlatform.FormattingEnabled = true;
            this.productPlatform.Location = new System.Drawing.Point(89, 3);
            this.productPlatform.Name = "productPlatform";
            this.productPlatform.Size = new System.Drawing.Size(95, 21);
            this.productPlatform.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "gekauft bei:";
            // 
            // productSupplier
            // 
            this.productSupplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productSupplier.FormattingEnabled = true;
            this.productSupplier.Location = new System.Drawing.Point(89, 58);
            this.productSupplier.Name = "productSupplier";
            this.productSupplier.Size = new System.Drawing.Size(95, 21);
            this.productSupplier.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(76, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Herkunftsland:";
            // 
            // productCountryOfOrigin
            // 
            this.productCountryOfOrigin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productCountryOfOrigin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.productCountryOfOrigin.FormattingEnabled = true;
            this.productCountryOfOrigin.Location = new System.Drawing.Point(89, 113);
            this.productCountryOfOrigin.Name = "productCountryOfOrigin";
            this.productCountryOfOrigin.Size = new System.Drawing.Size(95, 21);
            this.productCountryOfOrigin.TabIndex = 5;
            // 
            // mediaTabPages
            // 
            this.mediaTabPages.Controls.Add(this.mediaTabInfo);
            this.mediaTabPages.Controls.Add(this.mediaTabMetadata);
            this.mediaTabPages.Controls.Add(this.mediaTabGraphdata);
            this.mediaTabPages.Controls.Add(this.mediaTabFilesystem);
            this.mediaTabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaTabPages.Location = new System.Drawing.Point(0, 0);
            this.mediaTabPages.Name = "mediaTabPages";
            this.mediaTabPages.SelectedIndex = 0;
            this.mediaTabPages.Size = new System.Drawing.Size(195, 199);
            this.mediaTabPages.TabIndex = 0;
            // 
            // mediaTabInfo
            // 
            this.mediaTabInfo.Controls.Add(this.tableLayoutPanel7);
            this.mediaTabInfo.Location = new System.Drawing.Point(4, 22);
            this.mediaTabInfo.Name = "mediaTabInfo";
            this.mediaTabInfo.Size = new System.Drawing.Size(187, 173);
            this.mediaTabInfo.TabIndex = 0;
            this.mediaTabInfo.Text = "Info";
            this.mediaTabInfo.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.31285F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.68715F));
            this.tableLayoutPanel7.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.mediaName, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.label12, 0, 1);
            this.tableLayoutPanel7.Controls.Add(this.mediaType, 1, 1);
            this.tableLayoutPanel7.Controls.Add(this.label13, 0, 2);
            this.tableLayoutPanel7.Controls.Add(this.mediaSku, 1, 2);
            this.tableLayoutPanel7.Controls.Add(this.mediaStillSealed, 1, 3);
            this.tableLayoutPanel7.Controls.Add(this.mediaStorageSpaceId, 1, 4);
            this.tableLayoutPanel7.Controls.Add(this.label14, 0, 4);
            this.tableLayoutPanel7.Controls.Add(this.label15, 0, 5);
            this.tableLayoutPanel7.Controls.Add(this.mediaDumpPath, 1, 5);
            this.tableLayoutPanel7.Controls.Add(this.mediaMoreOptionsButton, 0, 6);
            this.tableLayoutPanel7.Controls.Add(this.mediaSave, 1, 6);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 7;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.32152F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.32152F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.32152F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.1783F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(187, 173);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Name:";
            // 
            // mediaName
            // 
            this.mediaName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaName.Enabled = false;
            this.mediaName.Location = new System.Drawing.Point(70, 3);
            this.mediaName.Name = "mediaName";
            this.mediaName.Size = new System.Drawing.Size(114, 20);
            this.mediaName.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 24);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "Medientyp:";
            // 
            // mediaType
            // 
            this.mediaType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mediaType.Enabled = false;
            this.mediaType.FormattingEnabled = true;
            this.mediaType.Location = new System.Drawing.Point(70, 27);
            this.mediaType.Name = "mediaType";
            this.mediaType.Size = new System.Drawing.Size(114, 21);
            this.mediaType.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(32, 13);
            this.label13.TabIndex = 4;
            this.label13.Text = "SKU:";
            // 
            // mediaSku
            // 
            this.mediaSku.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaSku.Enabled = false;
            this.mediaSku.Location = new System.Drawing.Point(70, 51);
            this.mediaSku.Name = "mediaSku";
            this.mediaSku.Size = new System.Drawing.Size(114, 20);
            this.mediaSku.TabIndex = 5;
            // 
            // mediaStillSealed
            // 
            this.mediaStillSealed.AutoSize = true;
            this.mediaStillSealed.Enabled = false;
            this.mediaStillSealed.Location = new System.Drawing.Point(70, 75);
            this.mediaStillSealed.Name = "mediaStillSealed";
            this.mediaStillSealed.Size = new System.Drawing.Size(96, 17);
            this.mediaStillSealed.TabIndex = 6;
            this.mediaStillSealed.Text = "ist verschweißt";
            this.mediaStillSealed.UseVisualStyleBackColor = true;
            // 
            // mediaStorageSpaceId
            // 
            this.mediaStorageSpaceId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaStorageSpaceId.Enabled = false;
            this.mediaStorageSpaceId.Location = new System.Drawing.Point(70, 99);
            this.mediaStorageSpaceId.Name = "mediaStorageSpaceId";
            this.mediaStorageSpaceId.Size = new System.Drawing.Size(114, 20);
            this.mediaStorageSpaceId.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 96);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 24);
            this.label14.TabIndex = 8;
            this.label14.Text = "Archivmedium:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 120);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(61, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "Archivpfad:";
            // 
            // mediaDumpPath
            // 
            this.mediaDumpPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaDumpPath.Enabled = false;
            this.mediaDumpPath.Location = new System.Drawing.Point(70, 123);
            this.mediaDumpPath.Name = "mediaDumpPath";
            this.mediaDumpPath.Size = new System.Drawing.Size(114, 20);
            this.mediaDumpPath.TabIndex = 10;
            // 
            // mediaMoreOptionsButton
            // 
            this.mediaMoreOptionsButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaMoreOptionsButton.Enabled = false;
            this.mediaMoreOptionsButton.Location = new System.Drawing.Point(3, 147);
            this.mediaMoreOptionsButton.Name = "mediaMoreOptionsButton";
            this.mediaMoreOptionsButton.Size = new System.Drawing.Size(61, 23);
            this.mediaMoreOptionsButton.TabIndex = 11;
            this.mediaMoreOptionsButton.Text = "Weitere Optionen";
            this.mediaMoreOptionsButton.UseVisualStyleBackColor = true;
            this.mediaMoreOptionsButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // mediaSave
            // 
            this.mediaSave.Enabled = false;
            this.mediaSave.Location = new System.Drawing.Point(70, 147);
            this.mediaSave.Name = "mediaSave";
            this.mediaSave.Size = new System.Drawing.Size(75, 23);
            this.mediaSave.TabIndex = 12;
            this.mediaSave.Text = "Speichern";
            this.mediaSave.UseVisualStyleBackColor = true;
            this.mediaSave.Click += new System.EventHandler(this.mediaSave_Click);
            // 
            // mediaTabMetadata
            // 
            this.mediaTabMetadata.Controls.Add(this.mediaMetadata);
            this.mediaTabMetadata.Location = new System.Drawing.Point(4, 22);
            this.mediaTabMetadata.Name = "mediaTabMetadata";
            this.mediaTabMetadata.Size = new System.Drawing.Size(187, 173);
            this.mediaTabMetadata.TabIndex = 2;
            this.mediaTabMetadata.Text = "Metadaten";
            this.mediaTabMetadata.UseVisualStyleBackColor = true;
            // 
            // mediaMetadata
            // 
            this.mediaMetadata.AllowDrop = true;
            this.mediaMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaMetadata.Enabled = false;
            this.mediaMetadata.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mediaMetadata.Location = new System.Drawing.Point(0, 0);
            this.mediaMetadata.MaxLength = 999999999;
            this.mediaMetadata.Multiline = true;
            this.mediaMetadata.Name = "mediaMetadata";
            this.mediaMetadata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.mediaMetadata.Size = new System.Drawing.Size(187, 173);
            this.mediaMetadata.TabIndex = 0;
            this.mediaMetadata.WordWrap = false;
            this.mediaMetadata.DragDrop += new System.Windows.Forms.DragEventHandler(this.TextBox_DragAndDrop);
            this.mediaMetadata.DragEnter += new System.Windows.Forms.DragEventHandler(this.Global_DragEnter);
            // 
            // mediaTabGraphdata
            // 
            this.mediaTabGraphdata.Controls.Add(this.graphDataControl1);
            this.mediaTabGraphdata.Location = new System.Drawing.Point(4, 22);
            this.mediaTabGraphdata.Name = "mediaTabGraphdata";
            this.mediaTabGraphdata.Size = new System.Drawing.Size(187, 173);
            this.mediaTabGraphdata.TabIndex = 3;
            this.mediaTabGraphdata.Text = "Graphdaten";
            this.mediaTabGraphdata.UseVisualStyleBackColor = true;
            // 
            // graphDataControl1
            // 
            this.graphDataControl1.AllowDrop = true;
            this.graphDataControl1.ContextMenuStrip = this.graphDataContextMenuStrip;
            this.graphDataControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphDataControl1.Location = new System.Drawing.Point(0, 0);
            this.graphDataControl1.MediumId = 0;
            this.graphDataControl1.Name = "graphDataControl1";
            this.graphDataControl1.Size = new System.Drawing.Size(187, 173);
            this.graphDataControl1.TabIndex = 0;
            // 
            // graphDataContextMenuStrip
            // 
            this.graphDataContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.automatischLadenToolStripMenuItem});
            this.graphDataContextMenuStrip.Name = "graphDataContextMenuStrip";
            this.graphDataContextMenuStrip.Size = new System.Drawing.Size(173, 26);
            // 
            // automatischLadenToolStripMenuItem
            // 
            this.automatischLadenToolStripMenuItem.Name = "automatischLadenToolStripMenuItem";
            this.automatischLadenToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.automatischLadenToolStripMenuItem.Text = "automatisch laden";
            this.automatischLadenToolStripMenuItem.Click += new System.EventHandler(this.automatischLadenToolStripMenuItem_Click);
            // 
            // mediaTabFilesystem
            // 
            this.mediaTabFilesystem.Controls.Add(this.mediaFilesystemTreeView);
            this.mediaTabFilesystem.Location = new System.Drawing.Point(4, 22);
            this.mediaTabFilesystem.Name = "mediaTabFilesystem";
            this.mediaTabFilesystem.Size = new System.Drawing.Size(187, 173);
            this.mediaTabFilesystem.TabIndex = 10;
            this.mediaTabFilesystem.Text = "Dateisystem";
            this.mediaTabFilesystem.UseVisualStyleBackColor = true;
            // 
            // mediaFilesystemTreeView
            // 
            this.mediaFilesystemTreeView.ContextMenuStrip = this.filesystemContextMenuStrip;
            this.mediaFilesystemTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaFilesystemTreeView.ImageIndex = 0;
            this.mediaFilesystemTreeView.ImageList = this.filesystemImageList;
            this.mediaFilesystemTreeView.Location = new System.Drawing.Point(0, 0);
            this.mediaFilesystemTreeView.Name = "mediaFilesystemTreeView";
            this.mediaFilesystemTreeView.SelectedImageIndex = 0;
            this.mediaFilesystemTreeView.Size = new System.Drawing.Size(187, 173);
            this.mediaFilesystemTreeView.TabIndex = 0;
            // 
            // filesystemContextMenuStrip
            // 
            this.filesystemContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageParsenToolStripMenuItem,
            this.discScannenToolStripMenuItem});
            this.filesystemContextMenuStrip.Name = "filesystemContextMenuStrip";
            this.filesystemContextMenuStrip.Size = new System.Drawing.Size(217, 48);
            this.filesystemContextMenuStrip.Text = "Filesystem";
            // 
            // imageParsenToolStripMenuItem
            // 
            this.imageParsenToolStripMenuItem.Enabled = false;
            this.imageParsenToolStripMenuItem.Name = "imageParsenToolStripMenuItem";
            this.imageParsenToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.imageParsenToolStripMenuItem.Text = "Image parsen";
            this.imageParsenToolStripMenuItem.Click += new System.EventHandler(this.imageParsenToolStripMenuItem_Click);
            // 
            // discScannenToolStripMenuItem
            // 
            this.discScannenToolStripMenuItem.Enabled = false;
            this.discScannenToolStripMenuItem.Name = "discScannenToolStripMenuItem";
            this.discScannenToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.discScannenToolStripMenuItem.Text = "physikalische Disc scannen";
            this.discScannenToolStripMenuItem.Click += new System.EventHandler(this.discScannenToolStripMenuItem_Click);
            // 
            // filesystemImageList
            // 
            this.filesystemImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("filesystemImageList.ImageStream")));
            this.filesystemImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.filesystemImageList.Images.SetKeyName(0, "cdrom.png");
            this.filesystemImageList.Images.SetKeyName(1, "closedDir.png");
            this.filesystemImageList.Images.SetKeyName(2, "file.png");
            this.filesystemImageList.Images.SetKeyName(3, "openDir.png");
            // 
            // mediaMoreOptions
            // 
            this.mediaMoreOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setMetadataAndDump,
            this.öffneDumpToolStripMenuItem,
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem,
            this.metadatenExportierenToolStripMenuItem,
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem,
            this.m3UEditorÖffnenToolStripMenuItem,
            this.weitereAnhängeToolStripMenuItem});
            this.mediaMoreOptions.Name = "mediaMoreOptions";
            this.mediaMoreOptions.Size = new System.Drawing.Size(390, 158);
            // 
            // setMetadataAndDump
            // 
            this.setMetadataAndDump.Name = "setMetadataAndDump";
            this.setMetadataAndDump.Size = new System.Drawing.Size(389, 22);
            this.setMetadataAndDump.Text = "Setze Metadaten und Dump gleich";
            this.setMetadataAndDump.Click += new System.EventHandler(this.setMetadataAndDump_Click);
            // 
            // öffneDumpToolStripMenuItem
            // 
            this.öffneDumpToolStripMenuItem.Name = "öffneDumpToolStripMenuItem";
            this.öffneDumpToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.öffneDumpToolStripMenuItem.Text = "Öffne Dump";
            this.öffneDumpToolStripMenuItem.Click += new System.EventHandler(this.öffneDumpToolStripMenuItem_Click);
            // 
            // playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem
            // 
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem.Name = "playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem";
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem.Text = "Playlist aus Ordner erstellen und als Dump&Metadaten setzen";
            this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem.Click += new System.EventHandler(this.playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem_Click);
            // 
            // metadatenExportierenToolStripMenuItem
            // 
            this.metadatenExportierenToolStripMenuItem.Name = "metadatenExportierenToolStripMenuItem";
            this.metadatenExportierenToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.metadatenExportierenToolStripMenuItem.Text = "Metadaten exportieren";
            this.metadatenExportierenToolStripMenuItem.Click += new System.EventHandler(this.metadatenExportierenToolStripMenuItem_Click);
            // 
            // metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem
            // 
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem.Name = "metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem";
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem.Text = "Metadaten exportieren und als neuen Dump setzen";
            this.metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem.Click += new System.EventHandler(this.metadatenExportierenToolStripMenuItem_Click);
            // 
            // m3UEditorÖffnenToolStripMenuItem
            // 
            this.m3UEditorÖffnenToolStripMenuItem.Name = "m3UEditorÖffnenToolStripMenuItem";
            this.m3UEditorÖffnenToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.m3UEditorÖffnenToolStripMenuItem.Text = "M3U-Editor öffnen";
            this.m3UEditorÖffnenToolStripMenuItem.Click += new System.EventHandler(this.m3UEditorÖffnenToolStripMenuItem_Click);
            // 
            // weitereAnhängeToolStripMenuItem
            // 
            this.weitereAnhängeToolStripMenuItem.Name = "weitereAnhängeToolStripMenuItem";
            this.weitereAnhängeToolStripMenuItem.Size = new System.Drawing.Size(389, 22);
            this.weitereAnhängeToolStripMenuItem.Text = "Weitere Anhänge";
            this.weitereAnhängeToolStripMenuItem.Click += new System.EventHandler(this.weitereAnhängeToolStripMenuItem_Click);
            // 
            // metadataAndDumpOpenFileDialog
            // 
            this.metadataAndDumpOpenFileDialog.FileName = "openFileDialog1";
            this.metadataAndDumpOpenFileDialog.Filter = "Disk-Metadaten (*.m3u8;*.AnyDVDHD;*.m3u;*.gdi;*.cue)|*.m3u8;*.AnyDVDHD;*.m3u;*.gd" +
    "i;*.cue";
            // 
            // dumpOpenFileDialog
            // 
            this.dumpOpenFileDialog.Filter = "Mediendatei (*.gz;*.mkv;*.iso;*.zip;*.mp3;*.chd;*.epub;*.m4a;*.xci;*.wbfs;*.nsp;*" +
    ".vpk;*.tar)|*.gz;*.mkv;*.iso;*.zip;*.mp3;*.chd;*.epub;*.m4a;*.xci;*.wbfs;*.nsp;*" +
    ".vpk;*.tar";
            // 
            // filesystemIsoParseOpenFileDialog
            // 
            this.filesystemIsoParseOpenFileDialog.Filter = "ISO9660 (*.iso), DD (*.img), httpd-ack (*.gdi)|*.iso;*.img;*.gdi";
            // 
            // MediaLibraryControl
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MediaLibraryControl";
            this.Size = new System.Drawing.Size(866, 419);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.productTabMainPage.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.productTabCover.ResumeLayout(false);
            this.productTabMedia.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.productTabScreenshots.ResumeLayout(false);
            this.productTabLinks.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.mediaTabPages.ResumeLayout(false);
            this.mediaTabInfo.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mediaStorageSpaceId)).EndInit();
            this.mediaTabMetadata.ResumeLayout(false);
            this.mediaTabMetadata.PerformLayout();
            this.mediaTabGraphdata.ResumeLayout(false);
            this.graphDataContextMenuStrip.ResumeLayout(false);
            this.mediaTabFilesystem.ResumeLayout(false);
            this.filesystemContextMenuStrip.ResumeLayout(false);
            this.mediaMoreOptions.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListView productsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage productTabMainPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox productName;
        private System.Windows.Forms.CheckBox productComplete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker productPurchaseDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox productISBN;
        private System.Windows.Forms.Button productSave;
        private System.Windows.Forms.ToolStripMenuItem neuesProduktToolStripMenuItem;
        private System.Windows.Forms.TabPage productTabCover;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.TabPage productTabLinks;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox productPlatform;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox productSupplier;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox productCountryOfOrigin;
        private System.Windows.Forms.TabPage productTabScreenshots;
        private System.Windows.Forms.TabPage productTabMedia;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.ListView productMediaListView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Button productAddMediaButton;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.TabControl mediaTabPages;
        private System.Windows.Forms.TabPage mediaTabInfo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox mediaName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox mediaType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox mediaSku;
        private System.Windows.Forms.CheckBox mediaStillSealed;
        private System.Windows.Forms.NumericUpDown mediaStorageSpaceId;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox mediaDumpPath;
        private System.Windows.Forms.TabPage mediaTabMetadata;
        private System.Windows.Forms.TextBox mediaMetadata;
        private System.Windows.Forms.TabPage mediaTabGraphdata;
        private System.Windows.Forms.Button mediaMoreOptionsButton;
        private System.Windows.Forms.ContextMenuStrip mediaMoreOptions;
        private System.Windows.Forms.ToolStripMenuItem setMetadataAndDump;
        private System.Windows.Forms.Button mediaSave;
        private Control.PictureDrop productCover;
        private Control.PictureDrop productScreenshot;
        private Control.CurrencyConverterTextBox productCost;
        private System.Windows.Forms.ContextMenuStrip graphDataContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem automatischLadenToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog metadataAndDumpOpenFileDialog;
        private System.Windows.Forms.ToolStripMenuItem öffneDumpToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog dumpOpenFileDialog;
        private System.Windows.Forms.CheckBox productNSFW;
        private System.Windows.Forms.ToolStripMenuItem playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem;
        private System.Windows.Forms.TabPage mediaTabFilesystem;
        private System.Windows.Forms.TreeView mediaFilesystemTreeView;
        private System.Windows.Forms.OpenFileDialog filesystemIsoParseOpenFileDialog;
        private System.Windows.Forms.ImageList filesystemImageList;
        private System.Windows.Forms.ToolStripMenuItem metadatenExportierenToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bandcampKollektionImportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defekteM3UDateienReparierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metafilesAutomatischErgänzenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageOrdnerImportierenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vervollständigkeitsassistentToolStripMenuItem;
        private System.Windows.Forms.Button productRemoveMediaButton;
        private System.Windows.Forms.ToolStripMenuItem allesVervollständigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regalVervollständigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem produktVervollständigenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem m3UEditorÖffnenToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip filesystemContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem imageParsenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem discScannenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem weitereAnhängeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pSXISOBatchImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dVDBoxImportierenToolStripMenuItem;
        private GraphDataControl graphDataControl1;
    }
}
