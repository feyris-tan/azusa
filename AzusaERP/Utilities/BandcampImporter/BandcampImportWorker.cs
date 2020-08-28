using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;
using libazuworker;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using TagLib;
using File = TagLib.File;

namespace moe.yo3explorer.azusa.Utilities.BandcampImporter
{
    class BandcampImportWorker : AzusaWorker
    {
        public BandcampImportWorker(DirectoryInfo sourceFolder)
        {
            SourceFolder = sourceFolder;
        }

        public override void DoWork()
        {
            FileInfo[] fileInfos = SourceFolder.GetFiles("*.zip");
            Context.DatabaseDriver.BeginTransaction();
            foreach (FileInfo fileInfo in fileInfos)
            {
                WorkerForm.InvokeNextStep(Path.GetFileName(fileInfo.Name));
                int productId = GetOrCreateProduct(fileInfo);
                Product product = Context.DatabaseDriver.GetProductById(productId);
                int mediaId = Context.DatabaseDriver.CreateMediaAndReturnId(productId, Path.GetFileNameWithoutExtension(fileInfo.Name));
                Media media = Context.DatabaseDriver.GetMediaById(mediaId);
                media.MediaTypeId = 14;
                ZipFile zf = ZipFile.Read(fileInfo.FullName);
                string albumName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                string outputPathString = Path.Combine(TargetFolder.FullName, albumName);
                DirectoryInfo albumOutputPath = new DirectoryInfo(outputPathString);
                EnsureDirectoryExists(albumOutputPath);

                ZipEntry[] zipEntries = zf.Entries.ToArray();
                List<BandcampAlbumEntry> albumEntries = new List<BandcampAlbumEntry>();

                for (int i = 0; i < zipEntries.Length; i++)
                {
                    ZipEntry zipEntry = zipEntries[i];
                    MemoryStream ms = new MemoryStream();
                    zipEntry.Extract(ms);
                    string outputFileName = zipEntry.FileName;
                    if (outputFileName.StartsWith(albumName))
                    {
                        outputFileName = outputFileName.Substring(albumName.Length);
                    }

                    if (outputFileName.StartsWith(" - "))
                    {
                        outputFileName = outputFileName.Substring(3);
                    }
                    outputFileName = Path.Combine(albumOutputPath.FullName, outputFileName);
                    FileInfo outputFile = new FileInfo(outputFileName);
                    System.IO.File.WriteAllBytes(outputFile.FullName, ms.ToArray());
                    outputFile.Refresh();
                    if (zipEntry.FileName.ToLowerInvariant().EndsWith(".flac"))
                    {
                        ArbitraryStreamFileAbstraction abstraction =
                            new ArbitraryStreamFileAbstraction(ms, zipEntry.FileName);
                        File file = File.Create(abstraction);
                        media.Name = file.Tag.Album;
                        if (!IsDiscography)
                        {
                            if (product.Picture == null)
                            {
                                if (file.Tag.Pictures.Length > 0)
                                {
                                    IPicture tagPicture = file.Tag.Pictures[0];
                                    product.Picture = tagPicture.Data.Data;
                                    Context.DatabaseDriver.SetCover(product);
                                }
                            }
                        }
                        BandcampAlbumEntry bandcampAlbumEntry = new BandcampAlbumEntry();
                        bandcampAlbumEntry.FileInfo = outputFile;
                        bandcampAlbumEntry.TrackNo = file.Tag.Track;
                        albumEntries.Add(bandcampAlbumEntry);
                        file.Dispose();
                    }

                    WorkerForm.InvokeSetCurrentStepProgress(i, zipEntries.Length);
                }

                if (albumEntries.Count > 1)
                {
                    albumEntries.Sort((x, y) => x.TrackNo.CompareTo(y.TrackNo));
                    FileInfo fi = new FileInfo(Path.Combine(albumOutputPath.FullName, "disc.m3u8"));
                    StreamWriter sw = new StreamWriter(fi.OpenWrite(), Encoding.UTF8);
                    albumEntries.ForEach(x => sw.WriteLine(String.Format("{1}",fi.Name,x.FileInfo.Name)));
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    fi.Refresh();
                    media.SetDumpFile(fi);
                    media.MetaFileContent = System.IO.File.ReadAllText(fi.FullName);
                }

                Context.DatabaseDriver.UpdateMedia(media);
            }

            Context.DatabaseDriver.EndTransaction(true);
            WorkerForm.InvokeClose();
        }

        private void EnsureDirectoryExists(DirectoryInfo di)
        {
            if (di.Exists)
                return;
            else
            {
                EnsureDirectoryExists(di.Parent);
                di.Create();
                di.Refresh();
            }
        }

        public int GetOrCreateProduct(FileInfo fi)
        {
            if (IsDiscography)
            {
                if (DiscographyProductId.HasValue)
                    return DiscographyProductId.Value;
                else
                {
                    DiscographyProductId = Context.DatabaseDriver.CreateProductAndReturnId(Shelf, DiscographyName);
                    return DiscographyProductId.Value;
                }
            }
            else
            {
                return Context.DatabaseDriver.CreateProductAndReturnId(Shelf, fi.Name);
            }
        }

        public override string Title => "Bandcamp Import";
        public override int InitialNumberOfSteps => SourceFolder.GetFiles("*.zip").Length;
        public override string InitalizingMessage => "Moment bitte..";
        public Shelf Shelf { get; set; }
        public DirectoryInfo SourceFolder { get; set; }
        public DirectoryInfo TargetFolder { get; set; }
        public bool IsDiscography { get; set; }
        public string DiscographyName { get; set; }
        public int? DiscographyProductId;
        public AzusaContext Context { get; set; }
    }
}
