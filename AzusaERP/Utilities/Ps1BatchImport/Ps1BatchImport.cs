using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Boundary;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.Utilities.Ps1BatchImport
{
    class Ps1BatchImport
    {
        public void Run(DirectoryInfo indir, Shelf outShelf,double potprice)
        {
            if (!indir.Exists)
            {
                MessageBox.Show(String.Format("{0} existiert nicht.", indir.FullName));
                return;
            }

            DirectoryInfo[] directoryInfos = indir.GetDirectories();
            Game[] games = new Game[directoryInfos.Length];

            for (int i = 0; i < directoryInfos.Length; i++)
            {
                games[i] = new Game();
                games[i].GameDiscs = new List<GameDisc>();
                games[i].Name = directoryInfos[i].Name;

                FileInfo[] fileInfos = directoryInfos[i].GetFiles();
                List<FileInfo> ibgFiles = new List<FileInfo>();
                foreach (FileInfo fileInfo in fileInfos)
                {
                    string extension = fileInfo.Extension.ToLowerInvariant();
                    string name = Path.GetFileNameWithoutExtension(fileInfo.Name);
                    GameDisc gameDisc;
                    switch (extension)
                    {
                        case ".bin":
                            gameDisc = games[i].GetDiscByName(name);
                            gameDisc.BinFile = fileInfo;
                            break;
                        case ".cue":
                            gameDisc = games[i].GetDiscByName(name);
                            gameDisc.CueFile = fileInfo;
                            gameDisc.CueContent = File.ReadAllText(fileInfo.FullName);
                            break;
                        case ".md5":
                            gameDisc = games[i].GetDiscByName(name);
                            gameDisc.Md5File = fileInfo;
                            gameDisc.Md5Content = File.ReadAllText(fileInfo.FullName);
                            break;
                        case ".jpg":
                            games[i].Cover = File.ReadAllBytes(fileInfo.FullName);
                            break;
                        case ".ibg":
                            ibgFiles.Add(fileInfo);
                            break;
                        default:
                            MessageBox.Show(String.Format("Don't know about {0}...",extension));
                            return;
                    }
                }

                foreach (FileInfo ibgInfo in ibgFiles)
                {
                    long lookFor = ibgInfo.LastWriteTime.ToUnixMinute();
                    GameDisc gameDisc = games[i].GetByUnixMinute(lookFor);
                    gameDisc.IbgContent = File.ReadAllText(ibgInfo.FullName);
                }
            }

            for (int i = 0; i < games.Length; i++)
            {
                int productId = ProductService.CreateProduct(games[i].Name, outShelf);
                Product product = ProductService.GetProduct(productId);
                product.BoughtOn = PurchaseDate;
                product.Complete = true;
                product.CountryOfOriginId = CountryId;
                product.Consistent = true;
                product.NSFW = false;
                product.Picture = games[i].Cover;
                product.PlatformId = PlatformId;
                product.Price = Math.Round(potprice / games.Length, 2);
                product.SupplierId = SupplierId;
                ProductService.UpdateProduct(product);
                ProductService.SetCover(product);

                foreach (GameDisc gameDisc in games[i].GameDiscs)
                {
                    int mediaId = MediaService.CreateMedia(product, gameDisc.Name);
                    Media media = MediaService.GetSpecificMedia(mediaId);
                    media.GraphDataContent = gameDisc.IbgContent;
                    media.MediaTypeId = 0;
                    media.MetaFileContent = gameDisc.CueContent;
                    media.SKU = gameDisc.GuessSku();
                    media.isSealed = false;

                    FileInfo createdCue = CopyFile(gameDisc.CueFile);
                    media.SetDumpFile(createdCue);
                    MediaService.UpdateMedia(media);

                    FileInfo createdBin = CopyFile(gameDisc.BinFile);
                    Stream binStream = createdBin.OpenRead();
                    media.SetFilesystemMetadata(binStream);
                    binStream.Close();
                }
            }

            Console.WriteLine("Great!");
        }

        public DateTime PurchaseDate { get; set; }
        public int CountryId { get; set; }
        public int PlatformId { get; set; }
        public int SupplierId { get; set; }
        public DirectoryInfo OutputDirectory { get; set; }

        public FileInfo CopyFile(FileInfo infile)
        {
            string combine = Path.Combine(OutputDirectory.FullName, infile.Name);
            Console.WriteLine("Copy to: " + combine);

            if (!File.Exists(combine))
            {
                File.Copy(infile.FullName, combine);
            }

            return new FileInfo(combine);
        }
    }
}
