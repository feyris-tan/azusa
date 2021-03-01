using System;
using System.Collections.Generic;
using System.IO;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Boundary;
using moe.yo3explorer.azusa.Utilities.Ps1BatchImport;

namespace moe.yo3explorer.azusa.Utilities
{
    class TheAvengersImport
    {
        public void Run(DirectoryInfo indir, Shelf outShelf)
        {
            if (!indir.Exists)
            {
                MessageBox.Show(String.Format("{0} existiert nicht.", indir.FullName));
                return;
            }
            
            Game game = new Game();
            game.GameDiscs = new List<GameDisc>();
            game.Name = String.Format("Batch-Import {0}", DateTime.Now.ToString());

            FileInfo[] fileInfos = indir.GetFiles();
            List<FileInfo> ibgFiles = new List<FileInfo>();

            foreach (FileInfo fileInfo in fileInfos)
            {
                string extension = fileInfo.Extension.ToLowerInvariant();
                string name = Path.GetFileNameWithoutExtension(fileInfo.Name);
                GameDisc gameDisc;
                switch (extension)
                {
                    case ".iso":    //yes
                        gameDisc = game.GetDiscByName(name);
                        gameDisc.BinFile = fileInfo;
                        break;
                    case ".dvd":    //yes
                        gameDisc = game.GetDiscByName(name);
                        gameDisc.CueFile = fileInfo;
                        gameDisc.CueContent = File.ReadAllText(fileInfo.FullName);
                        break;
                    case ".md5":    //yes
                        gameDisc = game.GetDiscByName(name);
                        gameDisc.Md5File = fileInfo;
                        gameDisc.Md5Content = File.ReadAllText(fileInfo.FullName);
                        break;
                    case ".mds":    //yes
                        gameDisc = game.GetDiscByName(name);
                        gameDisc.MdsFile = fileInfo;
                        gameDisc.MdsContent = File.ReadAllBytes(fileInfo.FullName);
                        break;
                    case ".ibg":    //eyes
                        ibgFiles.Add(fileInfo);
                        break;
                    default:
                        MessageBox.Show(String.Format("Don't know about {0}...", extension));
                        return;
                }
            }

            foreach (FileInfo ibgInfo in ibgFiles)
            {
                long lookFor = ibgInfo.LastWriteTime.ToUnixMinute();
                GameDisc gameDisc = game.GetByUnixMinute(lookFor);
                gameDisc.IbgContent = File.ReadAllText(ibgInfo.FullName);
            }

            game.GameDiscs.Sort(new OrderByIsoDate());

            for (int i = 0; i < game.GameDiscs.Count; i++)
            {
                game.GameDiscs[i].Name = String.Format("Disc {0}", i + 1);
            }

            int productId = ProductService.CreateProduct(game.Name, outShelf);
            Product product = ProductService.GetProduct(productId);
            product.Complete = true;
            product.Consistent = true;
            product.NSFW = false;
            ProductService.UpdateProduct(product);
            
            foreach (GameDisc gameDisc in game.GameDiscs)
            {
                int mediaId = MediaService.CreateMedia(product, gameDisc.Name);
                Media media = MediaService.GetSpecificMedia(mediaId);
                media.ChecksumContent = gameDisc.Md5Content;
                media.CueSheetContent = gameDisc.CueContent;
                media.GraphDataContent = gameDisc.IbgContent;
                media.MdsContent = gameDisc.MdsContent;
                media.MediaTypeId = 1;
                media.isSealed = false;

                MediaService.UpdateMedia(media);
                
                Stream binStream = gameDisc.BinFile.OpenRead();
                media.SetFilesystemMetadata(binStream);
                binStream.Close();
            }

            Console.WriteLine("Great!");
        }
        
        class OrderByIsoDate : IComparer<GameDisc>
        {
            public int Compare(GameDisc x, GameDisc y)
            {
                return x.BinFile.CreationTime.CompareTo(y.BinFile.CreationTime);
            }
        }
    }
}
