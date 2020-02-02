using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using libazuworker;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using TagLib;
using File = TagLib.File;

namespace moe.yo3explorer.azusa.BandcampImporter
{
    class BrokenImportRepairWorker : AzusaWorker
    {
        public BrokenImportRepairWorker(Media[] media)
        {
            this.media = media;
        }

        private Media[] media;
        private AzusaStorageSpaceDrive storageSpace;

        public override void DoWork()
        {
            AzusaContext azusaContext = AzusaContext.GetInstance();
            
            for (int i = 0; i < media.Length; i++)
            {
                Media currentMedium = media[i];
                WorkerForm.InvokeNextStep(currentMedium.Name);

                FindStorageSpace(currentMedium.DumpStorageSpaceId);
                if (storageSpace == null)
                {
                    MessageBox.Show(String.Format("Medium #{0} ist nicht angeschlossen!", currentMedium.DumpStorageSpaceId));
                    WorkerForm.InvokeClose();
                    return;
                }

                string m3uFileName = Path.Combine(storageSpace.RootDirectory.FullName, currentMedium.DumpStorageSpacePath);
                FileInfo m3uFileInfo = new FileInfo(m3uFileName);
                if (m3uFileInfo.Exists)
                    m3uFileInfo.Delete();
                DirectoryInfo m3uDirectory = m3uFileInfo.Directory;
                FileInfo[] flacFiles = m3uDirectory.GetFiles("*.flac");
                List<BandcampAlbumEntry> albumEntries = new List<BandcampAlbumEntry>();
                for (int j = 0; j < flacFiles.Length; j++)
                {
                    WorkerForm.InvokeSetCurrentStepProgress(j, flacFiles.Length);
                    FileInfo flacFile = flacFiles[j];
                    File file = File.Create(flacFile.FullName);
                    BandcampAlbumEntry bae = new BandcampAlbumEntry();
                    bae.FileInfo = flacFile;
                    bae.TrackNo = file.Tag.Track;
                    albumEntries.Add(bae);
                    file.Dispose();
                }

                if (albumEntries.Count > 1)
                {
                    albumEntries.Sort((x, y) => x.TrackNo.CompareTo(y.TrackNo));
                    FileInfo fi = new FileInfo(Path.Combine(m3uDirectory.FullName, "disc.m3u8"));
                    StreamWriter sw = new StreamWriter(fi.OpenWrite(), Encoding.UTF8);
                    albumEntries.ForEach(x => sw.WriteLine(String.Format("{1}", fi.Name, x.FileInfo.Name)));
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                    fi.Refresh();
                    currentMedium.SetDumpFile(fi);
                }

                azusaContext.DatabaseDriver.UpdateMedia(currentMedium);
            }
            WorkerForm.InvokeClose();
        }

        private void FindStorageSpace(int id)
        {
            if (storageSpace != null)
            {
                if (storageSpace.StorageSpace.MediaNo == id)
                    return;
            }

            foreach (AzusaStorageSpaceDrive spaceDrive in AzusaStorageSpaceDrive.FindConnectedStorageSpaces())
            {
                if (spaceDrive.StorageSpace.MediaNo == id)
                {
                    storageSpace = spaceDrive;
                    return;
                }
            }

            storageSpace = null;
        }

        public override string Title => "Defekte M3U Dateien reparieren";
        public override int InitialNumberOfSteps => media.Length;
        public override string InitalizingMessage => "Vorbereiten";
    }
}
