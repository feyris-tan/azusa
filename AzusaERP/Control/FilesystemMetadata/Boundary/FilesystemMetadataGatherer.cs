using System;
using System.IO;
using System.Windows.Forms;
using com.dyndns_server.yo3explorer.yo3explorer.Archive;
using DiscUtils;
using DiscUtils.Fat;
using DiscUtils.Iso9660;
using DiscUtils.Udf;
using libazusax360;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary
{
    internal static class FilesystemMetadataGatherer
    {
        public static void Gather(Media medium, Stream infile)
        {
            string failed = "";
            XboxISOFileSource xboxIso = XboxISOFileSource.TryOpen(infile);
            if (xboxIso != null)
            {
                XDvdFsFileSystemEntry xdvdfs = xboxIso.GetFileSystem();
                Gather(medium, xdvdfs, null);
                infile.Dispose();
                return;
            }
            failed += "XDVDFS\n";

            if (CDReader.Detect(infile))
            {
                CDReader cdReader = new CDReader(infile, true, false);
                Gather(medium, cdReader.Root, null);
                infile.Dispose();
                return;
            }
            failed += "ISO9660\n";

            if (UdfReader.Detect(infile))
            {
                UdfReader udfReader = new UdfReader(infile);

                if (udfReader != null)
                {
                    try
                    {
                        Gather(medium, udfReader.Root, null);
                        return;
                    }
                    catch (Exception)
                    {
                        AzusaContext.GetInstance().DatabaseDriver.ForgetFilesystemContents(medium.Id);
                    }
                    
                }
            }
            failed += "UDF\n";

            if (FatFileSystem.Detect(infile))
            {
                FatFileSystem fat = new FatFileSystem(infile);
                Gather(medium, fat.Root, null);
                infile.Dispose();
                return;
            }
            failed += "FAT32\n";

            if (infile.Length < 3200)
            {
                FileStream fileStream = infile as FileStream;
                if (fileStream != null)
                {
                    fileStream.Dispose();
                    FileInfo fi = new FileInfo(fileStream.Name);
                    Stream gdRomStream = GDROMReader.BuildGdRomStream(fi);
                    if (CDReader.Detect(gdRomStream))
                    {
                        CDReader cdReader = new CDReader(gdRomStream, true, false);
                        Gather(medium, cdReader.Root, null);
                        infile.Dispose();
                        return;
                    }
                }
            }
            failed += "GD-ROM\n";

            MessageBox.Show("Konnte kein Dateisystem erkennen. Versucht wurden:" + failed);
        }

        public static void Gather(Media medium, DriveInfo driveInfo)
        {
            Gather(medium, driveInfo.RootDirectory,null);
        }

        private static void Gather(Media medium, XDvdFsFileSystemEntry entry, FilesystemMetadataEntity parent)
        {
            IDatabaseDriver dbDriver = AzusaContext.GetInstance().DatabaseDriver;

            FilesystemMetadataEntity dirEntity = new FilesystemMetadataEntity();
            dirEntity.FullName = entry.FullPath;
            dirEntity.IsDirectory = entry.IsFolder;
            dirEntity.MediaId = medium.Id;
            dirEntity.Modified = null;
            dirEntity.ParentId = parent != null ? parent.Id : -1;
            if (entry.IsFolder)
            {
                dbDriver.AddFilesystemInfo(dirEntity);
                foreach (XDvdFsFileSystemEntry subfile in entry.Files)
                {
                    Gather(medium, subfile, dirEntity);
                }
            }
            else
            {
                int readSize = (int)Math.Min(2048, entry.LogicalFileSize);
                byte[] buffer = new byte[readSize];
                Stream inStream = entry.GetStream();
                readSize = inStream.Read(buffer, 0, readSize);
                inStream.Close();
                Array.Resize(ref buffer, readSize);
                dirEntity.Header = buffer;
                dbDriver.AddFilesystemInfo(dirEntity);
            }
        }

        private static void Gather(Media medium, DiscDirectoryInfo ddi, FilesystemMetadataEntity parent)
        {
            IDatabaseDriver dbDriver = AzusaContext.GetInstance().DatabaseDriver;

            FilesystemMetadataEntity dirEntity = new FilesystemMetadataEntity();
            dirEntity.FullName = ddi.FullName;
            dirEntity.IsDirectory = true;
            dirEntity.MediaId = medium.Id;
            dirEntity.Modified = ddi.LastWriteTime;
            dirEntity.ParentId = parent != null ? parent.Id : -1;
            dbDriver.AddFilesystemInfo(dirEntity);

            foreach (DiscDirectoryInfo subdir in ddi.GetDirectories())
            {
                Gather(medium, subdir, dirEntity);
            }

            foreach (DiscFileInfo file in ddi.GetFiles())
            {
                FilesystemMetadataEntity fileEntity = new FilesystemMetadataEntity();
                fileEntity.FullName = file.FullName;
                fileEntity.IsDirectory = false;
                fileEntity.MediaId = medium.Id;
                fileEntity.Modified = file.LastWriteTime;
                fileEntity.ParentId = dirEntity.Id;
                fileEntity.Size = file.Length;

                int readSize = (int)Math.Min(2048, file.Length);
                byte[] buffer = new byte[readSize];
                Stream inStream = file.OpenRead();
                readSize = inStream.Read(buffer, 0, readSize);
                inStream.Close();
                Array.Resize(ref buffer, readSize);
                fileEntity.Header = buffer;

                dbDriver.AddFilesystemInfo(fileEntity);
            }
        }

        private static void Gather(Media medium, DirectoryInfo di, FilesystemMetadataEntity parent)
        {
            IDatabaseDriver dbDriver = AzusaContext.GetInstance().DatabaseDriver;

            FilesystemMetadataEntity dirEntity = new FilesystemMetadataEntity();
            dirEntity.FullName = di.FullName.Replace(di.Root.FullName, "");
            dirEntity.IsDirectory = true;
            dirEntity.MediaId = medium.Id;
            dirEntity.Modified = di.LastWriteTime;
            dirEntity.ParentId = parent != null ? parent.Id : -1;
            dbDriver.AddFilesystemInfo(dirEntity);

            foreach (DirectoryInfo subdir in di.GetDirectories())
            {
                Gather(medium, subdir, dirEntity);
            }

            foreach (FileInfo file in di.GetFiles())
            {
                FilesystemMetadataEntity fileEntity = new FilesystemMetadataEntity();
                fileEntity.FullName = file.FullName;
                fileEntity.IsDirectory = false;
                fileEntity.MediaId = medium.Id;
                fileEntity.Modified = file.LastWriteTime;
                fileEntity.ParentId = dirEntity.Id;
                fileEntity.Size = file.Length;

                int readSize = (int)Math.Min(2048, file.Length);
                byte[] buffer = new byte[readSize];
                Stream inStream = file.OpenRead();
                readSize = inStream.Read(buffer, 0, readSize);
                inStream.Close();
                Array.Resize(ref buffer, readSize);
                fileEntity.Header = buffer;

                dbDriver.AddFilesystemInfo(fileEntity);
            }
        }
    }
}
