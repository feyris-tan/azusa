using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DiscUtils;
using DiscUtils.Iso9660;
using dsMediaLibraryClient.GraphDataLib;
using libazuworker;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.MediaLibrary.Control;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.Utilities;
using moe.yo3explorer.azusa.Utilities.BandcampImporter;
using moe.yo3explorer.azusa.Utilities.FolderMapper.Boundary;
using moe.yo3explorer.azusa.Utilities.FolderMapper.Control;
using moe.yo3explorer.azusa.Utilities.Ps1BatchImport;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    public partial class MediaLibraryControl : UserControl
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

        private List<Platform> platforms;
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
            }

            currentShelf = ((ShelfTabPage) tabControl1.TabPages[0]).Shelf;

            context.Splash.SetLabel("Abfragen von Plattformen...");
            platforms = PlatformService.GetAllPlatforms().ToList();
            foreach (Platform platform in platforms)
            {
                productPlatform.Items.Add(platform);
            }

            context.Splash.SetLabel("Abfragen von Händlern...");
            foreach (Shop shop in ShopService.GetAllShops())
            {
                productSupplier.Items.Add(shop);
            }

            context.Splash.SetLabel("Abfragen von Länderdaten...");
            foreach (Country country in CountryService.GetCountries())
            {
                productCountryOfOrigin.Items.Add(country);
            }

            context.Splash.SetLabel(String.Format("Abfragen von Inhalt von Regal {0}", currentShelf.Name));
            tabControl1_SelectedIndexChanged(tabControl1, new EventArgs());
        }

        #endregion Azusa Module Driver

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Shelf shelf = ((ShelfTabPage) tabControl1.SelectedTab).Shelf;
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

            currentProdInShelf = (ProductInShelf) productsListView.SelectedItems[0];
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
            productNSFW.Checked = currentProduct.NSFW;
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

            foreach (Shop s in productSupplier.Items)
            {
                if (s.Id == currentProduct.SupplierId)
                {
                    productSupplier.SelectedItem = s;
                    break;
                }
            }

            foreach (Country c in productCountryOfOrigin.Items)
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
                MediaInProduct singleMedium = (MediaInProduct) productMediaListView.Items[0];
                currentMedia = MediaService.GetSpecificMedia(singleMedium.MediaId);
                UpdateMediaSidebar();
            }
        }

        private void UpdateProductTabMedia()
        {
            productMediaListView.Items.Clear();
            foreach (MediaInProduct mts in MediaService.GetMediaFromProduct(currentProduct))
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
            graphDataControl1.Enabled = enabled;
            mediaMoreOptionsButton.Enabled = enabled;
            mediaSave.Enabled = enabled;
            imageParsenToolStripMenuItem.Enabled = enabled;
            discScannenToolStripMenuItem.Enabled = enabled;
            mediaFilesystemTreeView.Enabled = enabled;

            productRemoveMediaButton.Enabled = enabled && productMediaListView.Items.Count > 1;

            if (!enabled)
                return;

            mediaName.Text = currentMedia.Name;
            foreach (MediaType mt in mediaType.Items)
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
            mediaMetadata.Text = currentMedia.MetaFileContent.unix2dos();
            if (!string.IsNullOrEmpty(currentMedia.GraphDataContent))
            {
                graphDataControl1.Data = Encoding.UTF8.GetBytes(currentMedia.GraphDataContent.unix2dos());
            }
            else
            {
                graphDataControl1.Data = new byte[0];
            }

            UpdateFilesystemTreeView();
        }

        private void productMediaListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMedia = null;
            if (productMediaListView.SelectedItems.Count > 0)
            {
                MediaInProduct mip = (MediaInProduct) productMediaListView.SelectedItems[0];
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
            TextBox target = (TextBox) sender;

            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[]) j;
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
            currentProduct.PlatformId = ((Platform) productPlatform.SelectedItem).Id;
            currentProduct.SupplierId = ((Shop) productSupplier.SelectedItem).Id;
            currentProduct.CountryOfOriginId = ((Country) productCountryOfOrigin.SelectedItem).ID;
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
            currentMedia.Name = mediaName.Text;
            currentMedia.MediaTypeId = ((MediaType) mediaType.SelectedItem).Id;
            currentMedia.SKU = mediaSku.Text;
            currentMedia.isSealed = mediaStillSealed.Checked;
            currentMedia.DumpStorageSpaceId = (int) mediaStorageSpaceId.Value;
            currentMedia.DumpStorageSpacePath = mediaDumpPath.Text;
            currentMedia.MetaFileContent = mediaMetadata.Text;
            currentMedia.GraphDataContent = Encoding.UTF8.GetString(graphDataControl1.Data);

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

            foreach (MediaInProduct mip in productMediaListView.Items)
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
                        string result = string.Format("Soll die Datei {0} jetzt importiert werden?",
                            ibgFiles[0].FullName);
                        if (MessageBox.Show(this, result, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                            DialogResult.Yes)
                        {
                            importedFromImgBurn = true;
                            graphDataControl1.Data = File.ReadAllBytes(ibgFiles[0].FullName);
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

            DirectoryInfo rootDir = fi.Directory;
            DeleteIfNecessary(rootDir, "Thumbs.db");

            string extension = Path.GetExtension(filename).ToLowerInvariant();
            if (extension.Equals(".cue"))
            {
                if (TestMediaAttachment(currentMedia, "CUE Sheet"))
                {
                    SetMediaAttachment(currentMedia,"CUE Sheet",Encoding.UTF8.GetBytes("mediaMetadata.Text"));
                }
            }
        }

        private void SetMediaAttachment(Media media, string attachmentTypeName, byte[] data)
        {
            if (data == null)
                return;
            if (data.Length == 0)
                return;

            AttachmentType attachmentType = context.DatabaseDriver.GetAllMediaAttachmentTypes().First(x => x.name.Equals(attachmentTypeName));
            if (attachmentType == null)
                return;

            Attachment attachment = context.DatabaseDriver.GetAllMediaAttachments(media).First(x => x._TypeId == attachmentType.id);
            if (attachment == null)
            {
                attachment = new Attachment();
                attachment._Buffer = data;
                attachment._Complete = true;
                attachment._TypeId = attachmentType.id;
                attachment._MediaId = media.Id;
                context.DatabaseDriver.InsertAttachment(attachment);
            }
            else
            {
                attachment._Buffer = data;
                attachment._Complete = true;
                attachment._DateUpdated = DateTime.Now;
                attachment._TypeId = attachmentType.id;
                attachment._MediaId = media.Id;
                context.DatabaseDriver.UpdateAttachment(attachment);
            }
        }

        private bool TestMediaAttachment(Media media, string attachmentTypeName)
        {
            AttachmentType attachmentType = context.DatabaseDriver.GetAllMediaAttachmentTypes().First(x => x.name.Equals(attachmentTypeName));
            if (attachmentType == null)
                return false;

            List<Attachment> attachments = context.DatabaseDriver.GetAllMediaAttachments(media).ToList();
            if (attachments.Count == 0)
                return false;

            if (!attachments.Any(x => x._TypeId == attachmentType.id))
                return false;

            Attachment attachment = attachments.First(x => x._TypeId == attachmentType.id);
            if (!attachment._Complete)
                return false;

            if (attachment._Buffer == null)
                return false;

            return attachment._Buffer.Length > 0;
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
            }
        }

        private string lastPath = null;

        private void playlistAusOrdnerErstellenUndAlsDumpMetadatenSetzenToolStripMenuItem_Click(object sender,
            EventArgs e)
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
            entites.Sort((x, y) => x.ParentId.Value.CompareTo(y.ParentId.Value));

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
                    FilesystemMetadataTreeViewItem parent =
                        treeNodes.First(x => x.Entity.Id == treeNodes[i].Entity.ParentId);
                    parent.Nodes.Add(treeNodes[i]);
                }
            }

            if (entites.Count > 2)
            {
                treeNodes[0].Expand();
            }
        }

        private void imageParsenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mediaFilesystemTreeView.Nodes.Count > 0)
            {
                DialogResult confirm = MessageBox.Show(this,
                    "Das Dateisystem für dieses Medium wurde schon eingelesen. Sollen die Informationen überschrieben werden?",
                    "", MessageBoxButtons.YesNo);
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

            File.WriteAllText(saveFileDialog1.FileName, mediaMetadata.Text);

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
                DialogResult confirm = MessageBox.Show(this,
                    "Das Dateisystem für dieses Medium wurde schon eingelesen. Sollen die Informationen überschrieben werden?",
                    "", MessageBoxButtons.YesNo);
                if (confirm != DialogResult.Yes)
                    return;
            }

            context.DatabaseDriver.BeginTransaction();
            context.DatabaseDriver.ForgetFilesystemContents(currentMedia.Id);
            try
            {
                FilesystemMetadataGatherer.Gather(currentMedia, candidates[0]);
                context.DatabaseDriver.EndTransaction(true);

                UpdateFilesystemTreeView();
            }
            catch (Exception exception)
            {
                context.DatabaseDriver.EndTransaction(false);
                MessageBox.Show(exception.Message);
            }
            
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

            MessageBox.Show(String.Format("{0} M3U Dateien wurden durch M3U8 Dateien ersetzt.",
                findBrokenBandcampImports.Length));
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
                        MessageBox.Show(String.Format("Autofix fehlgeschlagen: Unbekannte Dateierweiterung: {0}",
                            extension));
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

        #region Vervollständigungsassistent
        private Thread complationAssistantThread;
        private CompletionAssistantMode completionWizardMode;
        private enum CompletionAssistantMode
        {
            EVERYTHING,
            SHELF,
            PRODUCT
        }

        private void allesVervollständigen_Click(object sender, EventArgs e)
        {
            completionWizardMode = CompletionAssistantMode.EVERYTHING;
            complationAssistantThread = new Thread(WrapCompletionAssistant);
            complationAssistantThread.Name = "Vervollständigungsassistent";
            complationAssistantThread.Start();
        }

        private void regalVervollständigen_Click(object sender, EventArgs e)
        {
            completionWizardMode = CompletionAssistantMode.SHELF;
            complationAssistantThread = new Thread(WrapCompletionAssistant);
            complationAssistantThread.Name = "Vervollständigungsassistent";
            complationAssistantThread.Start();
        }

        private void produktVervollständigen_Click(object sender, EventArgs e)
        {
            completionWizardMode = CompletionAssistantMode.PRODUCT;
            complationAssistantThread = new Thread(WrapCompletionAssistant);
            complationAssistantThread.Name = "Vervollständigungsassistent";
            complationAssistantThread.Start();
        }

        private void BlockUi()
        {
            System.Windows.Forms.Control[] lockables = new System.Windows.Forms.Control[]
                {menuStrip1, tabControl1, productsListView, tabControl2, mediaTabPages};
            Invoke((MethodInvoker)delegate
            {
                foreach (var lockable in lockables)
                    lockable.Enabled = false;
            });
        }

        private void UnblockUi()
        {
            System.Windows.Forms.Control[] lockables = new System.Windows.Forms.Control[]
                {menuStrip1, tabControl1, productsListView, tabControl2, mediaTabPages};
            if (IsHandleCreated)
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (var lockable in lockables)
                        lockable.Enabled = true;
                });
            }
        }

        private void WrapCompletionAssistant()
        {
            BlockUi();
            CompletionAssistant();
            UnblockUi();
        }

        private void CompletionAssistant()
        {

            switch (completionWizardMode)
            {
                case CompletionAssistantMode.EVERYTHING:
                    CompletetionAssistantEverything();
                    break;
                case CompletionAssistantMode.SHELF:
                    CompletionAssistantShelf();
                    break;
                case CompletionAssistantMode.PRODUCT:
                    CompletionAssistantProduct();
                    break;
                default:
                    throw new NotImplementedException(completionWizardMode.ToString());
            }
        }

        private void CompletetionAssistantEverything()
        {
            foreach (TabPage shelfTabPage in tabControl1.TabPages)
            {
                Invoke((MethodInvoker)delegate { tabControl1.SelectedTab = shelfTabPage; });
                if (CompletionAssistantShelf() == AutoCompleteResult.ABORT)
                    return;
            }

        }

        private AutoCompleteResult CompletionAssistantShelf()
        {
            for (int i = 0; i < productsListView.Items.Count; i++)
            {
                Invoke((MethodInvoker)delegate
                {
                    productsListView.SelectedIndices.Clear();
                    productsListView.SelectedIndices.Add(i);
                    productsListView.EnsureVisible(i);
                });
                if (CompletionAssistantProduct() == AutoCompleteResult.ABORT)
                    return AutoCompleteResult.ABORT;
            }

            return AutoCompleteResult.CONTINUE;
        }

        private AutoCompleteResult CompletionAssistantProduct()
        {
            for (int j = 0; j < productMediaListView.Items.Count; j++)
            {
                Invoke((MethodInvoker)delegate
                {
                    productMediaListView.SelectedIndices.Clear();
                    productMediaListView.SelectedIndices.Add(j);
                    productMediaListView.EnsureVisible(j);
                    tabControl2.SelectedIndex = 2;
                });
                if (TryAutocomplete() == AutoCompleteResult.ABORT)
                    return AutoCompleteResult.ABORT;
            }
            return AutoCompleteResult.CONTINUE;
        }

        enum AutoCompleteResult
        {
            CONTINUE,
            ABORT
        }

        private AutoCompleteResult TryAutocomplete()
        {
            if (currentProduct.Consistent)
                return AutoCompleteResult.CONTINUE;

            string label = String.Format("{0} - {1}", currentProduct.Name, currentMedia.Name);

            if (string.IsNullOrEmpty(currentMedia.DumpStorageSpacePath))
            {
                MessageBox.Show(String.Format("Dump für {0} fehlt.",label));
                return AutoCompleteResult.ABORT;
            }

            FileInfo physicalDumpFileInfo = AzusaStorageSpaceDrive.FindFileOnConnectedSpaces(currentMedia.DumpStorageSpacePath);
            if (physicalDumpFileInfo == null)
            {
                MessageBox.Show(String.Format("Die Datei für den Dump von {0} existiert nicht.",label));
                return AutoCompleteResult.ABORT;
            }
            if (!physicalDumpFileInfo.Exists)
            {
                MessageBox.Show(String.Format("Die Datei {0} existiert nicht.", physicalDumpFileInfo.FullName));
                return AutoCompleteResult.ABORT;
            }

            if (currentProduct.PlatformId == 0)
            {
                if (!TryAutofillPlattform())
                {
                    MessageBox.Show(String.Format("Plattformeinstellung für {0} fehlt.", label));
                    return AutoCompleteResult.ABORT;
                }
            }

            if (productScreenshot.Data == null && currentShelf.ScreenshotRequired)
            {
                MessageBox.Show(String.Format("Screenshot für {0} fehlt.", label));
                return AutoCompleteResult.ABORT;
            }

            if (productCover.Data == null)
            {
                MessageBox.Show(String.Format("Coverbild für {0} fehlt.", label));
                return AutoCompleteResult.ABORT;
            }

            if (currentMedia == null)
            {
                if (productMediaListView.Items.Count == 1)
                    return AutoCompleteResult.CONTINUE;
                else
                    return AutoCompleteResult.ABORT;
            }
            if (currentMedia.DumpStorageSpaceId == 0)
            {
                MessageBox.Show(String.Format("Dump für {0} fehlt.", label));
                return AutoCompleteResult.ABORT;
            }

            if (string.IsNullOrEmpty(currentMedia.GraphDataContent))
            {
                MediaType currentMediaType = mediaTypes.Find(x => x.Id == currentMedia.MediaTypeId);
                if (currentMediaType.GraphData)
                {
                    MessageBox.Show(String.Format("Graphdaten für {0} fehlen.",label));
                    return AutoCompleteResult.ABORT;
                }
            }

            if (mediaFilesystemTreeView.Nodes.Count == 0)
            {
                MediaType currentMediaType = mediaTypes.Find(x => x.Id == currentMedia.MediaTypeId);
                if (currentMediaType.HasFilesystem)
                {
                    Platform platform = platforms.Find(x => x.Id == currentProduct.PlatformId);
                    string mediaExtension = Path.GetExtension(currentMedia.DumpStorageSpacePath).ToLowerInvariant();
                    if (mediaExtension.Equals(".iso"))
                    {
                        FileInfo isoInfo = AzusaStorageSpaceDrive.FindFileOnConnectedSpaces(currentMedia.DumpStorageSpacePath);
                        string q = String.Format("Soll das Dateisystem im Abbild \"{0}\" für {1} gesetzt werden?", isoInfo.FullName, label);
                        DialogResult qdr = MessageBox.Show(q, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (qdr == DialogResult.Yes)
                        {
                            FileStream isoStream = isoInfo.OpenRead();

                            context.DatabaseDriver.BeginTransaction();
                            context.DatabaseDriver.ForgetFilesystemContents(currentMedia.Id);
                            FilesystemMetadataGatherer.Gather(currentMedia, isoStream);
                            context.DatabaseDriver.EndTransaction(true);

                            isoStream.Close();
                            Invoke((MethodInvoker)delegate { UpdateFilesystemTreeView(); });
                            TryAutocomplete();
                        }
                        else
                        {
                            return AutoCompleteResult.ABORT;
                        }
                    }
                    else if (/*!platform.LongName.Equals("Audio CD") &&*/ !IsAudioCd())
                    {
                        MessageBox.Show(String.Format("Dateisystem von {0} wurde noch nicht geparst.",label));
                        return AutoCompleteResult.ABORT;
                    }
                }
            }

            return AutoCompleteResult.CONTINUE;
        }

        private bool IsAudioCd()
        {
            string extension = Path.GetExtension(currentMedia.DumpStorageSpacePath).ToLowerInvariant();
            bool ism3u8 = extension.Equals(".m3u8");
            bool isMp3 = extension.Equals(".mp3");
            if (ism3u8)
            {
                if (currentMedia.MetaFileContent.ToLowerInvariant().Contains(".mkv"))
                    return false;
                else
                    return true;
            }
            else if (isMp3)
                return true;
            else if (!string.IsNullOrEmpty(currentMedia.MetaFileContent))
            {
                return currentMedia.MetaFileContent.ToLowerInvariant().Contains(".flac");
            }
            else if (extension.Equals(".mkv"))
            {
                return false;
            }
            else if (extension.Equals(".iso"))
            {
                return false;
            }
            else
            {
                throw new Exception("lolwat?");
            }
        }
        private bool TryAutofillPlattform()
        {
            string label = String.Format("{0} - {1}", currentProduct.Name, currentMedia.Name);

            MediaType currentMediaType = mediaTypes.Find(x => x.Id == currentMedia.MediaTypeId);
            bool isMkv = Path.GetExtension(currentMedia.DumpStorageSpacePath).ToLowerInvariant().Equals(".mkv");
            bool isIso = Path.GetExtension(currentMedia.DumpStorageSpacePath).ToLowerInvariant().Equals(".iso");
            bool isMp3 = Path.GetExtension(currentMedia.DumpStorageSpacePath).ToLowerInvariant().Equals(".mp3");
            bool isTvShow = false;
            bool isMusicCd = false;
            if (!string.IsNullOrEmpty(currentMedia.MetaFileContent))
            {
                isTvShow = currentMedia.MetaFileContent.ToLowerInvariant().Contains(".mkv");
                isMusicCd = currentMedia.MetaFileContent.ToLowerInvariant().Contains(".flac");
            }
            Platform proposedPlattform = null;
            if (isMkv && currentMediaType.ShortName.Equals("DVD"))
            {
                proposedPlattform = platforms.Find(x => x.ShortName.Equals("DVD"));
            }
            else if (isMkv && currentMediaType.ShortName.Equals("Blu-Ray"))
            {
                proposedPlattform = platforms.Find(x => x.ShortName.Equals("Blu-Ray"));
            }
            else if ((isMusicCd || isMp3) && currentMediaType.ShortName.Equals("CD"))
            {
                proposedPlattform = platforms.Find(x => x.ShortName.Equals("CD"));
            }
            else if (isTvShow && currentMediaType.ShortName.Equals("DVD"))
            {
                proposedPlattform = platforms.Find(x => x.ShortName.Equals("DVD"));
            }
            else if (isTvShow && currentMediaType.ShortName.Equals("Blu-Ray"))
            {
                proposedPlattform = platforms.Find(x => x.ShortName.Equals("Blu-Ray"));
            }
            else if (isIso && currentMediaType.ShortName.Equals("CD"))
            {
                FileInfo fi = AzusaStorageSpaceDrive.FindFileOnConnectedSpaces(currentMedia.DumpStorageSpacePath);
                if (fi != null)
                {
                    FileStream isoStream = fi.OpenRead();
                    if (CDReader.Detect(isoStream))
                    {
                        CDReader cdromReader = new CDReader(isoStream, true, true);
                        FileExtensionDictionary extensions = new FileExtensionDictionary();
                        ScanIsoDirectory(cdromReader.Root, extensions);
                        cdromReader.Dispose();
                        double mp3Percentage = extensions.GetPercentageOfExtension(".mp3");
                        if (mp3Percentage > 90)
                        {
                            proposedPlattform = platforms.Find(x => x.ShortName.Equals("MP3"));
                        }
                    }
                    isoStream.Dispose();
                }
            }

            if (proposedPlattform == null)
            {
                return false;
            }

            if (!context.DatabaseDriver.CanUpdateExchangeRates)
            {
                MessageBox.Show(String.Format("Die Plattform für {1} fehlt. Wahrscheinlich ist es {0}.", proposedPlattform.ShortName, label), null, MessageBoxButtons.OK);
                return false;
            }

            DialogResult dialogResult = MessageBox.Show(String.Format("Ist die Plattform {0} korrekt im Falle von {1}?", proposedPlattform.ShortName, label), null, MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.No)
                return false;

            Invoke((MethodInvoker) delegate
            {
                productPlatform.SelectedItem = proposedPlattform;
                productSave_Click(vervollständigkeitsassistentToolStripMenuItem, new EventArgs());
            });
            return true;
        }

        private void ScanIsoDirectory(DiscDirectoryInfo ddi, FileExtensionDictionary extensions)
        {
            foreach (DiscDirectoryInfo subdir in ddi.GetDirectories())
            {
                ScanIsoDirectory(subdir, extensions);
            }
            foreach (DiscFileInfo file in ddi.GetFiles())
            {
                extensions.CountFile(new FileInfo(file.Name));
            }
        }
        #endregion

        private void productRemoveMediaButton_Click(object sender, EventArgs e)
        {
            string rant = String.Format("Soll das Medium {0} wirklich aus dem Produkt {1} entfernt werden?", currentMedia.Name, currentProduct.Name);
            DialogResult dr = MessageBox.Show(rant, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr != DialogResult.Yes)
                return;

            MediaInProduct mip = (MediaInProduct)productMediaListView.Items[0];

            context.DatabaseDriver.RemoveMedia(currentMedia);
            RefreshAndEnsureSelectedProduct(sender, e, currentProduct.Id);
            RefreshAndEnsureSelectedMedium(sender, e, mip.MediaId);
        }

        private void m3UEditorÖffnenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m3uGeneratorFileDrop generator = new m3uGeneratorFileDrop();
            if (generator.ShowDialog(this) != DialogResult.OK)
                return;

            setMetadataAndDump_Click(generator.FileName);
        }

        public IEnumerable<ToolStripItem> DestroyMenuStrip()
        {
            while (menuStrip1.Items.Count > 0)
            {
                yield return menuStrip1.Items[0];
            }

            Controls.Remove(menuStrip1);
        }

        private void weitereAnhängeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AttachmentEditor attachmentEditor = new AttachmentEditor(currentMedia, context);
            attachmentEditor.ShowDialog(FindForm());
        }

        #region PSX Batch Import
        private void pSXISOBatchImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string indirKey = context.ReadIniKey("psxbatchimport", "indir", "");
            if (string.IsNullOrEmpty(indirKey))
            {
                MessageBox.Show("Eingabeverzeichnis nicht definiert.");
                return;
            }

            string outdirKey = context.ReadIniKey("psxbatchimport", "outdir", "");
            if (string.IsNullOrEmpty(outdirKey))
            {
                MessageBox.Show("Ausgabeverzeichnis nicht definiert");
                return;
            }

            string potpriceKey = context.ReadIniKey("psxbatchimport", "potprice", "");
            if (string.IsNullOrEmpty(potpriceKey))
            {
                MessageBox.Show("Pottpreis nicht defininiert");
                return;
            }

            string purchaseDateKey = context.ReadIniKey("psxbatchimport", "purchased", "");
            if (string.IsNullOrEmpty(purchaseDateKey))
            {
                MessageBox.Show("Kaufdatum nicht definiert");
                return;
            }

            DirectoryInfo indir = new DirectoryInfo(indirKey);
            if (!indir.Exists)
            {
                MessageBox.Show("Eingabeverzeichnis existiert nicht!");
                return;
            }

            DirectoryInfo outdir = new DirectoryInfo(outdirKey);
            if (!outdir.Exists)
            {
                MessageBox.Show("Ausgabeverzeichnis existiert nicht!");
                return;
            }

            double potprice = -1;
            bool potpriceValid = double.TryParse(potpriceKey, out potprice);
            if (!potpriceValid)
            {
                MessageBox.Show("Pottpreis ist ungültig.");
                return;
            }

            DateTime purchaseDate = DateTime.MinValue;
            bool purchaseDateValid = DateTime.TryParse(purchaseDateKey, out purchaseDate);
            if (!purchaseDateValid)
            {
                MessageBox.Show("Kaufdatum ist ungültig.");
                return;
            }

            int countryId = context.ReadIniKey("psxbatchimport", "country", -1);
            if (countryId == -1)
            {
                MessageBox.Show("Herkunftsland ist ungültig.");
                return;
            }

            int platformId = context.ReadIniKey("psxbatchimport", "platform", -1);
            if (platformId == -1)
            {
                MessageBox.Show("Plattform ist ungültig.");
                return;
            }

            int supplierId = context.ReadIniKey("psxbatchimport", "supplier", -1);
            if (supplierId == -1)
            {
                MessageBox.Show("Händler ist ungültig.");
                return;
            }

            ThreadStart psxBatchImportThreadStart = new ThreadStart(() =>
            {
                BlockUi();
                PsxBatchImport(indir, outdir, currentShelf, potprice, purchaseDate, countryId, platformId, supplierId);
                Invoke((MethodInvoker) delegate { tabControl1_SelectedIndexChanged(sender, e); });
                UnblockUi();
            });

            Thread psxBatchImporThread = new Thread(psxBatchImportThreadStart);
            psxBatchImporThread.Priority = ThreadPriority.Lowest;
            psxBatchImporThread.Name = "PSXISO Batch Import";
            psxBatchImporThread.Start();
        }

        private void PsxBatchImport(DirectoryInfo indir, DirectoryInfo outDir, Shelf shelf, double potprice,
            DateTime purchaseDate, int countryId, int plarformId, int supplierId)
        {
            Ps1BatchImport batchImport = new Ps1BatchImport();
            batchImport.PurchaseDate = purchaseDate;
            batchImport.CountryId = countryId;
            batchImport.PlatformId = plarformId;
            batchImport.SupplierId = supplierId;
            batchImport.OutputDirectory = outDir;
            batchImport.Run(indir, shelf, potprice);
        }
        #endregion

        private void dVDBoxImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TheAvengersImport tai = new TheAvengersImport();

            string indirKey = context.ReadIniKey("dvdboximport", "indir", "");
            if (string.IsNullOrEmpty(indirKey))
            {
                MessageBox.Show("Eingabeverzeichnis nicht definiert.");
                return;
            }

            DirectoryInfo indir = new DirectoryInfo(indirKey);
            if (!indir.Exists)
            {
                MessageBox.Show("Eingabeverzeichnis existiert nicht!");
                return;
            }

            ThreadStart taiThreadStart = new ThreadStart(() =>
            {
                BlockUi();
                tai.Run(indir, currentShelf);
                UnblockUi();
            });

            Thread taiImportThread = new Thread(taiThreadStart);
            taiImportThread.Priority = ThreadPriority.Lowest;
            taiImportThread.Name = "DVD-Box Import";
            taiImportThread.Start();
        }
    }
}