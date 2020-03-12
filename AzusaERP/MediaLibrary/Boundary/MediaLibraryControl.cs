using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using dsMediaLibraryClient.GraphDataLib;
using libazuworker;
using moe.yo3explorer.azusa.BandcampImporter;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.FolderMapper.Boundary;
using moe.yo3explorer.azusa.MediaLibrary.Control;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using NPlot;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    public partial class MediaLibraryControl : UserControl, IAzusaModule
    {
        private List<MediaType> mediaTypes;
        private Shelf currentShelf;
        private ProductInShelf currentProdInShelf;
        private Product currentProduct;
        private AzusaContext context;
        private Media currentMedia;
        private int[] iconMapping;

        public MediaLibraryControl()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
            mediaMetadata.MaxLength = Int32.MaxValue;
            mediaGraphData.MaxLength = Int32.MaxValue;
            UpdateProductSidebar();
        }

        #region Azusa Module Driver
        public string IniKey => "azusa";
        public string Title
        {
            get { return "eigene Medienbibliothek"; }
        }

        public int Priority
        {
            get { return 1; }
        }

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            
            context.Splash.SetLabel("Abfragen von Medientypen...");
            mediaTypes = new List<MediaType>();
            MediaType[] allMediaTypes = MediaTypeService.GetMediaTypes().ToArray();
            iconMapping = new int[allMediaTypes.Max(x => x.Id) + 1];
            foreach (MediaType mediaType in allMediaTypes)
            {
                mediaTypes.Add(mediaType);
                imageList1.Images.Add(Image.FromStream(new MemoryStream(mediaType.Icon)));
                iconMapping[mediaType.Id] = imageList1.Images.Count - 1;
                this.mediaType.Items.Add(mediaType);
            }

            context.Splash.SetLabel("Abfragen von Regalen...");
            foreach (Shelf shelf in ShelfService.GetShelves())
            {
                tabControl1.TabPages.Add(new ShelfTabPage(shelf));
                productInShelf.Items.Add(shelf);
            }
            currentShelf = ((ShelfTabPage)tabControl1.TabPages[0]).Shelf;

            context.Splash.SetLabel("Abfragen von Plattformen...");
            foreach(Platform platform in PlatformService.GetAllPlatforms())
            {
                productPlatform.Items.Add(platform);
            }

            context.Splash.SetLabel("Abfragen von Händlern...");
            foreach(Shop shop in ShopService.GetAllShops())
            {
                productSupplier.Items.Add(shop);
            }

            context.Splash.SetLabel("Abfragen von Länderdaten...");
            foreach(Country country in CountryService.GetCountries())
            {
                productCountryOfOrigin.Items.Add(country);
            }

            context.Splash.SetLabel(String.Format("Abfragen von Inhalt von Regal {0}", currentShelf.Name));
            tabControl1_SelectedIndexChanged(tabControl1, new EventArgs());
        }
        #endregion Azusa Module Driver

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shelf shelf = ((ShelfTabPage)tabControl1.SelectedTab).Shelf;
            productsListView.Items.Clear();
            foreach (ProductInShelf p in ShelfService.GetProducts(shelf))
            {
                p.relatedShelf = shelf;
                p.UpdateListViewItem();
                p.ImageIndex = iconMapping[p.ImageIndex];
                productsListView.Items.Add(p);
            }

            currentShelf = shelf;
            currentProdInShelf = null;
            currentProduct = null;
            UpdateProductSidebar();
        }
        
        private void neuesProduktToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string newName = TextInputForm.Prompt("Produktname?", context.MainForm);
            if (string.IsNullOrEmpty(newName))
                return;

            context.DatabaseDriver.BeginTransaction();

            int newProd = ProductService.CreateProduct(newName, currentShelf);

            Product prodWrapper = new Product();
            prodWrapper.Id = newProd;
            MediaService.CreateMedia(prodWrapper, "Disk 1");

            context.DatabaseDriver.EndTransaction(true);

            RefreshAndEnsureSelectedProduct(sender, e, newProd);
        }

        private void RefreshAndEnsureSelectedProduct(object sender, EventArgs e, int newProd)
        {
            tabControl1_SelectedIndexChanged(sender, e);

            foreach (ProductInShelf item in productsListView.Items)
            {
                if (item.Id == newProd)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    break;
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (productsListView.SelectedItems.Count != 1)
                return;

            currentProdInShelf = (ProductInShelf)productsListView.SelectedItems[0];
            currentProduct = ProductService.GetProduct(currentProdInShelf.Id);
            UpdateProductSidebar();

            if (productMediaListView.Items.Count != 1)
            {
                currentMedia = null;
                UpdateMediaSidebar();
            }
        }

        private void UpdateProductSidebar()
        {
            bool prod = currentProduct != null;
            productName.Enabled = prod;
            productComplete.Enabled = prod;
            productCost.Enabled = prod;
            productPurchaseDate.Enabled = prod;
            productISBN.Enabled = prod;
            productPlatform.Enabled = prod;
            productSupplier.Enabled = prod;
            productCountryOfOrigin.Enabled = prod;
            productCover.Enabled = prod;
            productScreenshot.Enabled = prod;
            productSave.Enabled = prod;
            productAddMediaButton.Enabled = prod;
            productNSFW.Enabled = prod;

            if (!prod)
            {
                productScreenshot.Data = null;
                productCover.Data = null;
            }

            productMediaListView.Items.Clear();

            if (!prod)
                return;

            productName.Text = currentProduct.Name;
            productComplete.Checked = currentProduct.Complete;
            productCost.Value = currentProduct.Price;
            if (currentProduct.BoughtOn != DateTime.MinValue)
            {
                productPurchaseDate.Value = currentProduct.BoughtOn;
            }
            productISBN.Text = currentProduct.Sku;
            productId.Value = currentProduct.Id;
            productNSFW.Checked = currentProduct.NSFW;
            if (currentProduct.DateAdded != DateTime.MinValue)
            {
                productDateInserted.Value = currentProduct.DateAdded;
            }
            foreach(Shelf s in productInShelf.Items)
            {
                if (s.Id == currentProduct.InShelf)
                {
                    productInShelf.SelectedItem = s;
                    break;
                }
            }

            productCover.Data = currentProduct.Picture;
            productCover.DataChanged = false;

            foreach (Platform p in productPlatform.Items)
            {
                if (p.Id == currentProduct.PlatformId)
                {
                    productPlatform.SelectedItem = p;
                    break;
                }
            }
            foreach(Shop s in productSupplier.Items)
            {
                if (s.Id == currentProduct.SupplierId)
                {
                    productSupplier.SelectedItem = s;
                    break;
                }
            }
            foreach(Country c in productCountryOfOrigin.Items)
            {
                if (c.ID == currentProduct.CountryOfOriginId)
                {
                    productCountryOfOrigin.SelectedItem = c;
                    break;
                }
            }

            productScreenshot.Data = currentProduct.Screenshot;
            productScreenshot.DataChanged = false;

            UpdateProductTabMedia();

            if (productMediaListView.Items.Count == 1)
            {
                 MediaInProduct singleMedium = (MediaInProduct)productMediaListView.Items[0];
                 currentMedia = MediaService.GetSpecificMedia(singleMedium.MediaId);
                 UpdateMediaSidebar();
            }
        }

        private void UpdateProductTabMedia()
        {
            productMediaListView.Items.Clear();
            foreach(MediaInProduct mts in MediaService.GetMediaFromProduct(currentProduct))
            {
                mts.ImageIndex = iconMapping[mts.ImageIndex];
                productMediaListView.Items.Add(mts);
            }

            if (productMediaListView.Items.Count == 0)
                return;
        }

        private void UpdateMediaSidebar()
        {
            bool enabled = currentMedia != null;
            mediaName.Enabled = enabled;
            mediaType.Enabled = enabled;
            mediaSku.Enabled = enabled;
            mediaStillSealed.Enabled = enabled;
            mediaStorageSpaceId.Enabled = enabled;
            mediaDumpPath.Enabled = enabled;
            mediaMetadata.Enabled = enabled;
            mediaGraphData.Enabled = enabled;
            graphDataPlot.Enabled = enabled;
            mediaCueSheet.Enabled = enabled;
            mediaCdText.Enabled = enabled;
            mediaChecksum.Enabled = enabled;
            mediaOriginalPlaylist.Enabled = enabled;
            mediaLogfile.Enabled = enabled;
            mediaMds.Enabled = enabled;
            mediaMoreOptionsButton.Enabled = enabled;
            mediaSave.Enabled = enabled;
            imageParsenToolStripMenuItem.Enabled = enabled;
            discScannenToolStripMenuItem.Enabled = enabled;
            mediaFilesystemTreeView.Enabled = enabled;

            if (!enabled)
                return;

            mediaName.Text = currentMedia.Name;
            foreach(MediaType mt in mediaType.Items)
            {
                if (mt.Id == currentMedia.MediaTypeId)
                {
                    mediaType.SelectedItem = mt;
                    break;
                }
            }
            mediaSku.Text = currentMedia.SKU;
            mediaStillSealed.Checked = currentMedia.isSealed;
            mediaStorageSpaceId.Value = currentMedia.DumpStorageSpaceId;
            mediaDumpPath.Text = currentMedia.DumpStorageSpacePath;
            mediaID.Value = currentMedia.Id;
            mediaProductId.Value = currentMedia.RelatedProductId;
            if (currentMedia.DateAdded > mediaDateAdded.MinDate)
                mediaDateAdded.Value = currentMedia.DateAdded;
            mediaMetadata.Text = currentMedia.MetaFileContent.unix2dos();
            mediaGraphData.Text = currentMedia.GraphDataContent.unix2dos();

            if (!string.IsNullOrEmpty(currentMedia.GraphDataContent))
            {
                try
                {
                    GraphData gd = new GraphData(new StringReader(currentMedia.GraphDataContent));
                    UpdateGraphdataPlot(gd);
                }
                catch (InvalidMagicException ime)
                {
                    graphDataPlot.Clear();
                }
            }

            mediaCueSheet.Text = currentMedia.CueSheetContent.unix2dos();
            mediaCdText.Data = currentMedia.CdTextContent;
            mediaChecksum.Text = currentMedia.ChecksumContent.unix2dos();
            mediaOriginalPlaylist.Text = currentMedia.PlaylistContent.unix2dos();
            mediaLogfile.Text = currentMedia.LogfileContent.unix2dos();
            mediaMds.Data = currentMedia.MdsContent;
            mediaFauxHash.Text = currentMedia.FauxHash.ToString();

            UpdateFilesystemTreeView();
        }
        
        private void UpdateGraphdataPlot(GraphData gd)
        {
            graphDataPlot.Clear();
            gd.UpdateSyntheticLines();

            double[][] plotData = new double[6][];
            for (int i = 0; i < plotData.Length; i++) plotData[i] = new double[gd.NumberOfSamples];

            for (int i = 0; i < gd.NumberOfSamples; i++)
            {
                GraphDataSample sample = gd.GetSample(i);
                plotData[0][i] = sample.AverageCpuLoad;
                plotData[1][i] = sample.AverageReadSpeed;
                plotData[2][i] = sample.CpuLoad;
                plotData[3][i] = sample.ReadSpeed;
                plotData[4][i] = sample.SampleDistance;
                plotData[5][i] = sample.SectorNo;
            }
            
            graphDataPlot.Legend = new Legend();
            graphDataPlot.XAxis1 = new LinearAxis(0,gd.NumberOfSamples);
            graphDataPlot.YAxis1 = new LinearAxis(0, gd.SampleRate);

            Color[] colors = {Color.Cyan, Color.Yellow, Color.Pink, Color.Red, Color.Blue, Color.CornflowerBlue};
            string[] plotNames = {"Ø CPU", "Ø Read speed", "CPU", "Read Speed", "Distance", "SectorNo"};
            for (int i = 0; i < plotData.Length - 1; i++)
            {
                LinePlot linePlot = new LinePlot(plotData[i]);
                linePlot.Color = colors[i];
                linePlot.Label = plotNames[i];
                linePlot.ShowInLegend = true;
                graphDataPlot.Add(linePlot);
            }
        }

        private void productMediaListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMedia = null;
            if (productMediaListView.SelectedItems.Count > 0)
            {
                MediaInProduct mip = (MediaInProduct)productMediaListView.SelectedItems[0];
                currentMedia = MediaService.GetSpecificMedia(mip.MediaId);
            }
            UpdateMediaSidebar();
        }

        private void Global_DragEnter(object sender, DragEventArgs args)
        {
            if (args.Data.GetDataPresent(DataFormats.FileDrop))
                args.Effect = DragDropEffects.Copy;
        }

        private void TextBox_DragAndDrop(object sender, DragEventArgs e)
        {
            TextBox target = (TextBox)sender;

            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            target.MaxLength = Int32.MaxValue;
            target.Text = File.ReadAllText(jStrings[0]);
        }

        private void mediaMds_DataChanged(object sender, byte[] newData)
        {
            if (newData != null)
            {
                if (newData.Length > 0)
                {
                    //TODO: implement new MDS Parser
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mediaMoreOptions.Show(Cursor.Position);
        }

        private void productSave_Click(object sender, EventArgs e)
        {
            currentProduct.Name = productName.Text;
            currentProduct.Complete = productComplete.Checked;
            currentProduct.Picture = productCover.Data;
            currentProduct.BoughtOn = productPurchaseDate.Value.Date;
            currentProduct.Sku = productISBN.Text;
            currentProduct.PlatformId = ((Platform)productPlatform.SelectedItem).Id;
            currentProduct.SupplierId = ((Shop)productSupplier.SelectedItem).Id;
            currentProduct.CountryOfOriginId = ((Country)productCountryOfOrigin.SelectedItem).ID;
            currentProduct.Screenshot = productScreenshot.Data;
            currentProduct.NSFW = productNSFW.Checked;
            if (!productCost.Value.HasValue)
            {
                MessageBox.Show("Ungültiger Wert bei Preis.");
                return;
            }
            currentProduct.Price = productCost.Value.Value;

            ProductService.UpdateProduct(currentProduct);
            if (productCover.DataChanged)
            {
                ProductService.SetCover(currentProduct);
                productCover.DataChanged = false;
            }
            if (productScreenshot.DataChanged)
            {
                ProductService.SetScreenshot(currentProduct);
                productScreenshot.DataChanged = false;
            }

            RefreshAndEnsureSelectedProduct(sender, e, currentProduct.Id);
        }

        private void mediaSave_Click(object sender, EventArgs e)
        {
            currentMedia.Id = (int)mediaID.Value;
            currentMedia.RelatedProductId = (int)mediaProductId.Value;
            currentMedia.Name = mediaName.Text;
            currentMedia.MediaTypeId = ((MediaType)mediaType.SelectedItem).Id;
            currentMedia.SKU = mediaSku.Text;
            currentMedia.isSealed = mediaStillSealed.Checked;
            currentMedia.DumpStorageSpaceId = (int)mediaStorageSpaceId.Value;
            currentMedia.DumpStorageSpacePath = mediaDumpPath.Text;
            currentMedia.MetaFileContent = mediaMetadata.Text;
            currentMedia.DateAdded = mediaDateAdded.Value;
            currentMedia.CueSheetContent = mediaCueSheet.Text;
            currentMedia.ChecksumContent = mediaChecksum.Text;
            currentMedia.PlaylistContent = mediaOriginalPlaylist.Text;
            currentMedia.CdTextContent = mediaCdText.Data;
            currentMedia.LogfileContent = mediaLogfile.Text;
            currentMedia.MdsContent = mediaMds.Data;
            currentMedia.GraphDataContent = mediaGraphData.Text;
            currentMedia.FauxHash = Convert.ToInt64(mediaFauxHash.Text);

            Media temp = currentMedia;

            MediaService.UpdateMedia(currentMedia);

            RefreshAndEnsureSelectedProduct(sender, e, currentMedia.RelatedProductId);
            RefreshAndEnsureSelectedMedium(sender, e, temp.Id);

            if (importedFromImgBurn)
            {
                if (deleteMe != null)
                {
                    deleteMe.Delete();
                    importedFromImgBurn = false;
                    deleteMe = null;
                }
            }
        }

        private void RefreshAndEnsureSelectedMedium(object sender, EventArgs e, int mediaId)
        {
            UpdateProductTabMedia();
            
            foreach(MediaInProduct mip in productMediaListView.Items)
            {
                if (mip.MediaId == mediaId)
                {
                    mip.Selected = true;
                    mip.EnsureVisible();
                    break;
                }
            }
        }

        private void addMediaButton_Click(object sender, EventArgs e)
        {
            string mName = String.Format("Disk {0}", productMediaListView.Items.Count + 1);
            int newMedium = MediaService.CreateMedia(currentProduct, mName);

            RefreshAndEnsureSelectedProduct(sender, e, currentProduct.Id);
            RefreshAndEnsureSelectedMedium(sender, e, newMedium);
        }

        bool importedFromImgBurn = false;
        FileInfo deleteMe = null;

        private void automatischLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string appDataDir = Environment.GetEnvironmentVariable("APPDATA");
            if (!string.IsNullOrEmpty(appDataDir))
            {
                DirectoryInfo di = new DirectoryInfo(Path.Combine(appDataDir, "ImgBurn", "Graph Data Files"));
                if (di.Exists)
                {
                    FileInfo[] ibgFiles = di.GetFiles("*.ibg", SearchOption.TopDirectoryOnly);
                    if (ibgFiles.Length == 1)
                    {
                        string result = string.Format("Soll die Datei {0} jetzt importiert werden?", ibgFiles[0].FullName);
                        if (MessageBox.Show(this, result, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            importedFromImgBurn = true;
                            mediaGraphData.MaxLength = Int32.MaxValue;
                            mediaGraphData.Text = File.ReadAllText(ibgFiles[0].FullName);
                            deleteMe = ibgFiles[0];
                        }
                    }
                }
            }
        }

        private void setMetadataAndDump_Click(object sender, EventArgs e)
        {
            if (metadataAndDumpOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                setMetadataAndDump_Click(metadataAndDumpOpenFileDialog.FileName);
            }
        }

        private void setMetadataAndDump_Click(string filename)
        {
            mediaMetadata.Text = File.ReadAllText(filename);
            FileInfo fi = new FileInfo(filename);
            string fullname = fi.FullName.Substring(fi.Directory.Root.FullName.Length);
            mediaDumpPath.Text = fullname;
            AutoSetStorageSpaceId(fi.Directory.Root.FullName);
            SetHeaderCrc32(fi);

            DirectoryInfo rootDir = fi.Directory;
            DeleteIfNecessary(rootDir, "Thumbs.db");

            string extension = Path.GetExtension(filename).ToLowerInvariant();
            if (extension.Equals(".cue"))
            {
                if (string.IsNullOrEmpty(mediaCueSheet.Text))
                {
                    mediaCueSheet.Text = mediaMetadata.Text;
                }
            }
        }

        private void SetHeaderCrc32(FileInfo fi)
        {
            byte[] buffer = new byte[512];
            FileStream fs = fi.OpenRead();
            int headerLen = fs.Read(buffer, 0, 512);
            fs.Dispose();

            long result = 0;
            for (int i = 0; i < headerLen; i++)
            {
                result += buffer[i];
                result <<= 1;
            }

            mediaFauxHash.Text = result.ToString();
        }
        
        private void AutoSetStorageSpaceId(string rootName)
        {
            string mediaId = Path.Combine(rootName, "azusa_storagespace_id.xml");
            if (File.Exists(mediaId))
            {
                var ami = AzusaStorageSpace.Load(new FileInfo(mediaId));
                if (ami.MediaNo > 0)
                {
                    mediaStorageSpaceId.Value = ami.MediaNo;
                }
            }
        }

        private void öffneDumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dumpOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dumpOpenFileDialog.FileName);
                string fullname = fi.FullName.Substring(fi.Directory.Root.FullName.Length);
                mediaDumpPath.Text = fullname;
                AutoSetStorageSpaceId(fi.Directory.Root.FullName);
                SetHeaderCrc32(fi);
            }
        }

        private string lastPath = null;

        private void playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (!string.IsNullOrEmpty(lastPath))
                fbd.SelectedPath = lastPath;

            if (fbd.ShowDialog(this) != DialogResult.OK)
                return;
            
            DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);

            DeleteIfNecessary(di, "Thumbs.db");

            FileInfo[] files = di.GetFiles();
            List<FileInfo> fi = files.OrderBy(x => x.Name).ToList();
            FileInfo folderJpg = fi.FirstOrDefault(x => x.Name.ToLower().Equals("folder.jpg"));
            if (folderJpg != null)
            {
                fi.Remove(folderJpg);
                files = fi.ToArray();
            }
            string[] result = Array.ConvertAll(files, x => x.Name);
            string outFileName = Path.Combine(di.FullName, String.Format("{0}.m3u8", di.Name));
            File.WriteAllLines(outFileName, result);

            setMetadataAndDump_Click(outFileName);
            lastPath = fbd.SelectedPath;
        }

        private void DeleteIfNecessary(DirectoryInfo di, string filename)
        {
            FileInfo fi = new FileInfo(Path.Combine(di.FullName, filename));
            if (fi.Exists)
            {
                string msg = String.Format("Soll {0} vorher gelöscht werden?", filename);
                DialogResult dr = MessageBox.Show(msg, "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                    fi.Delete();
            }
        }

        #region Alles, was mit dem Dateisystembaum zusammenhängt
        private void filesystemContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void UpdateFilesystemTreeView()
        {
            mediaFilesystemTreeView.Nodes.Clear();
            List<FilesystemMetadataEntity> entites = new List<FilesystemMetadataEntity>();

            entites.AddRange(context.DatabaseDriver.GetFilesystemMetadata(currentMedia.Id, true));
            entites.AddRange(context.DatabaseDriver.GetFilesystemMetadata(currentMedia.Id, false));

            FilesystemMetadataTreeViewItem[] treeNodes = new FilesystemMetadataTreeViewItem[entites.Count];
            for (int i = 0; i < entites.Count; i++)
            {
                treeNodes[i] = new FilesystemMetadataTreeViewItem(entites[i]);
                if (treeNodes[i].Entity.ParentId == -1)
                {
                    mediaFilesystemTreeView.Nodes.Add(treeNodes[i]);
                }
                else
                {
                    FilesystemMetadataTreeViewItem parent = treeNodes.First(x => x.Entity.Id == treeNodes[i].Entity.ParentId);
                    parent.Nodes.Add(treeNodes[i]);
                }
            }
        }

        private void imageParsenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mediaFilesystemTreeView.Nodes.Count > 0)
            {
                DialogResult confirm = MessageBox.Show(this,"Das Dateisystem für dieses Medium wurde schon eingelesen. Sollen die Informationen überschrieben werden?","",MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes)
                    return;
            }

            DialogResult dialogResult = filesystemIsoParseOpenFileDialog.ShowDialog(this);
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            string ext = Path.GetExtension(filesystemIsoParseOpenFileDialog.FileName);
            ext = ext.ToLowerInvariant();
            Stream theFile;
            switch (ext)
            {
                default:
                    theFile = filesystemIsoParseOpenFileDialog.OpenFile();
                    break;
            }

            context.DatabaseDriver.BeginTransaction();
            context.DatabaseDriver.ForgetFilesystemContents(currentMedia.Id);
            FilesystemMetadataGatherer.Gather(currentMedia, theFile);
            context.DatabaseDriver.EndTransaction(true);

            UpdateFilesystemTreeView();
        }
        #endregion

        private void metadatenExportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Metadaten exportieren";
            saveFileDialog1.OverwritePrompt = true;
            saveFileDialog1.FileName = currentMedia.Name + ".m3u8";
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            File.WriteAllText(saveFileDialog1.FileName,mediaMetadata.Text);

            if (sender == metadatenExportenUndAlsNeuenDumpSetzenToolStripMenuItem)
            {
                setMetadataAndDump_Click(saveFileDialog1.FileName);
            }
        }

        private void discScannenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<DriveInfo> candidates = new List<DriveInfo>();
            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                if (driveInfo.IsReady && driveInfo.DriveType == DriveType.CDRom && driveInfo.TotalSize > 2048)
                {
                    candidates.Add(driveInfo);
                }
            }

            if (candidates.Count == 0)
            {
                MessageBox.Show("Es wurde keine Disc gefunden.");
                return;
            }

            if (candidates.Count > 1)
            {
                MessageBox.Show("Es wurde mehr als ein möglicher Kandidat gefunden. Dies wird nicht unterstützt.");
                return;
            }

            if (mediaFilesystemTreeView.Nodes.Count > 0)
            {
                DialogResult confirm = MessageBox.Show(this, "Das Dateisystem für dieses Medium wurde schon eingelesen. Sollen die Informationen überschrieben werden?", "", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes)
                    return;
            }
            
            context.DatabaseDriver.BeginTransaction();
            context.DatabaseDriver.ForgetFilesystemContents(currentMedia.Id);
            FilesystemMetadataGatherer.Gather(currentMedia, candidates[0]);
            context.DatabaseDriver.EndTransaction(true);

            UpdateFilesystemTreeView();
        }

        private void bandcampKollektionImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new BandcampImportForm().ShowDialog(this.FindForm());
        }

        private void defekteM3UDateienReparierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Media[] findBrokenBandcampImports = context.DatabaseDriver.findBrokenBandcampImports();
            if (findBrokenBandcampImports.Length == 0)
            {
                MessageBox.Show("Nichts kaputt!");
                return;
            }

            BrokenImportRepairWorker birw = new BrokenImportRepairWorker(findBrokenBandcampImports);

            WorkerForm wf = new WorkerForm(birw);
            wf.ShowDialog(this);

            MessageBox.Show(String.Format("{0} M3U Dateien wurden durch M3U8 Dateien ersetzt.", findBrokenBandcampImports.Length));
        }

        private void metafilesAutomatischErgänzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Media[] autofixableMetafiles = context.DatabaseDriver.FindAutofixableMetafiles();
            int totalFixed = 0;
            foreach (Media media in autofixableMetafiles)
            {
                string extension = Path.GetExtension(media.DumpStorageSpacePath).ToLowerInvariant();
                switch (extension)
                {
                    case ".mkv":
                    case ".mp3":
                    case ".epub":
                    case ".iso":
                    case ".wbfs":
                    case ".m4a":
                    case ".flac":
                    case ".zip":
                    case ".gz":
                    case ".xci":
                    case ".nsp":
                    case ".3ds":
                        continue;
                    case ".m3u8":
                        break;
                    default:
                        MessageBox.Show(String.Format("Autofix fehlgeschlagen: Unbekannte Dateierweiterung: {0}", extension));
                        return;
                }

                FileInfo fileInfo = AzusaStorageSpaceDrive.FindFileOnConnectedSpaces(media.DumpStorageSpacePath);
                media.MetaFileContent = File.ReadAllText(fileInfo.FullName);
                media.SetDumpFile(fileInfo);
                context.DatabaseDriver.UpdateMedia(media);
                totalFixed++;
            }

            MessageBox.Show(String.Format("{0} Metadaten ergänzt.", totalFixed));
        }

        private void imageOrdnerImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MapperWorker mapperWorker = new MapperWorker();
            mapperWorker.Run(this.FindForm());
        }
    }
}
