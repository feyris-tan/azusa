using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DiscUtils.Iso9660;
using DiscUtils.Streams;
using libazuworker;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.MediaLibrary.Boundary;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.Utilities.FolderMapper.Control;
using moe.yo3explorer.azusa.Utilities.Ps1BatchImport;

namespace moe.yo3explorer.azusa.Utilities.FolderMapper.Boundary
{
    public class MapperWorker : AzusaWorker
    {
        public override void DoWork()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.Desktop;
            fbd.ShowNewFolderButton = false;
            fbd.Description = "Quellordner auswählen";
            DialogResult dialogResult = DialogResult.Abort;
            WorkerForm.Invoke((MethodInvoker)delegate { dialogResult = fbd.ShowDialog(WorkerForm); });
            if (dialogResult != DialogResult.OK)
            {
                WorkerForm.InvokeClose();
                return;
            }

            FileExtensionDictionary fileExtensionDictionary = new FileExtensionDictionary();
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(fbd.SelectedPath);
            if (!sourceDirectoryInfo.Exists)
            {
                WorkerForm.InvokeMessageBox("Quellordner existiert nicht!");
                WorkerForm.InvokeClose();
                return;
            }

            WorkerForm.InvokeSetCurrentStepProgress(1);
            foreach (FileInfo fileInfo in sourceDirectoryInfo.GetFiles())
                fileExtensionDictionary.CountFile(fileInfo);

            FileInfo singleFileOutput = null;
            DirectoryInfo outputDir;
            string singleFileFilter = null;
            bool singleFileMode = false;
            string outputExtension = null;
            bool isAudiobook = false;
            if (fileExtensionDictionary.HasExtension(".dvd") && fileExtensionDictionary.HasExtension(".iso"))
            {
                singleFileMode = true;
                singleFileFilter = "DVDISO (*.iso)|*.iso";
                outputExtension = ".iso";
            }
            else if (fileExtensionDictionary.CountExtensions(".iso") == 1 && 
                     fileExtensionDictionary.CountExtensions(".md5") == 1 &&
                     fileExtensionDictionary.CountExtensions(".ibg") == 1)
            {
                singleFileMode = true;
                singleFileFilter = "DVDISO (*.iso)|*.iso";
                outputExtension = ".iso";
            }
            else if (fileExtensionDictionary.CountExtensions(".flac") > 1 &&
                     fileExtensionDictionary.HasExtension(".m3u8"))
            {
                singleFileMode = false;
            }
            else if (fileExtensionDictionary.CountExtensions(".mkv") > 1 &&
                     fileExtensionDictionary.HasExtension(".m3u8"))
            {
                singleFileMode = false;
            }
            else if (fileExtensionDictionary.HasExtension(".cue") && fileExtensionDictionary.HasExtension(".bin"))
            {
                singleFileMode = true;
                singleFileFilter = "CDROM RAW (*.bin)|*.bin";
                outputExtension = ".bin";
            }
            else if (fileExtensionDictionary.HasExtension(".3ds") && fileExtensionDictionary.HasExtension(".bin"))
            {
                singleFileMode = true;
                singleFileFilter = "3DS ROM (*.3ds)|*.3ds";
                outputExtension = ".3ds";
            }
            else if (fileExtensionDictionary.HasExtension(".xci"))
            {
                singleFileMode = true;
                singleFileFilter = "Nintendo Switch ROM (*.xci)|*.xci";
                outputExtension = ".xci";
            }
            else if (fileExtensionDictionary.CountExtensions(".mkv") == 1 && fileExtensionDictionary.DifferentExtension == 1)
            {
                singleFileMode = true;
                singleFileFilter = "MatrosKa Video (*.mkv)|*.mkv";
                outputExtension = ".mkv";
            }
            else if (fileExtensionDictionary.CountExtensions(".wav") == 1 && fileExtensionDictionary.HasExtension(".ibg"))
            {
                isAudiobook = true;
                singleFileMode = true;
                singleFileFilter = "MP3 (*.mp3)|*.mp3";
                outputExtension = ".mp3";
            }
            else
            {
                MessageBox.Show("Failed to detect Dump Type");
                WorkerForm.InvokeClose();
                return;
            }

            WorkerForm.InvokeSetCurrentStepProgress(2);
            MediaPickerForm mpf = new MediaPickerForm();
            WorkerForm.Invoke((MethodInvoker)delegate { dialogResult = mpf.ShowDialog(WorkerForm); });
            if (dialogResult != DialogResult.OK)
            {
                WorkerForm.InvokeClose();
                return;
            }

            if (singleFileMode)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.Filter = singleFileFilter;
                ofd.FileName = GuessFilename(mpf, outputExtension);
                if (fileExtensionDictionary.CountExtensions(".iso") == 1)
                {
                    FileInfo fileInfo = fileExtensionDictionary.GetFileFromExtension(".iso");
                    string ps2sku = PlaystationSkuDetector.DetectPs2Sku(fileInfo);
                    if (ps2sku != null)
                    {
                        ofd.FileName = ps2sku + "." + ofd.FileName;
                    }
                }

                if (fileExtensionDictionary.CountExtensions(".xci") == 1)
                {
                    ofd.FileName = fileExtensionDictionary.GetFileFromExtension(".xci").Name;
                }
                WorkerForm.Invoke((MethodInvoker) delegate { dialogResult = ofd.ShowDialog(WorkerForm); });
                if (dialogResult != DialogResult.OK) {
                    WorkerForm.InvokeClose();
                    return;
                }
                singleFileOutput = new FileInfo(ofd.FileName);
                outputDir = singleFileOutput.Directory;
            }
            else
            {
                fbd = new FolderBrowserDialog();
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.SelectedPath = GuessFolderName(mpf);
                fbd.ShowNewFolderButton = true;
                fbd.Description = GuessFolderName(mpf);
                WorkerForm.Invoke((MethodInvoker) delegate { dialogResult = fbd.ShowDialog(WorkerForm); });
                if (dialogResult != DialogResult.OK)
                {
                    WorkerForm.InvokeClose();
                    return;
                }
                outputDir = new DirectoryInfo(fbd.SelectedPath);
            }

            WorkerForm.InvokeSetCurrentStepProgress(3);
            AzusaContext azusaContext = AzusaContext.GetInstance();
            IDatabaseDriver databaseDriver = azusaContext.DatabaseDriver;
            Media mediaById = databaseDriver.GetMediaById(mpf.SelectedMedia.MediaId);

            foreach (FileInfo fileInfo in sourceDirectoryInfo.GetFiles())
            {
                fileInfo.Refresh();
                if (!fileInfo.Exists)
                    continue;

                WorkerForm.InvokeNextStep(fileInfo.Name);
                string ext = fileInfo.Extension.ToLower();
                switch (ext)
                {
                    case ".3ds":
                        if (!string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                            continue;

                        if (singleFileMode)
                        {
                            CopyFile(fileInfo, singleFileOutput);
                            mediaById.SetDumpFile(singleFileOutput);
                            databaseDriver.UpdateMedia(mediaById);
                            AttemptDelete(fileInfo);
                        }
                        else
                        {
                            throw new NotImplementedException("3ds in multi file mode.");
                        }
                        break;
                    case ".iso":
                        if (!string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                            continue;

                        if (singleFileMode)
                        {
                            CopyFile(fileInfo, singleFileOutput);
                            mediaById.SetDumpFile(singleFileOutput);
                            FileStream fileStream = singleFileOutput.OpenRead();
                            mediaById.SetFilesystemMetadata(fileStream);
                            fileStream.Close();
                            databaseDriver.UpdateMedia(mediaById);
                            AttemptDelete(fileInfo);
                        }
                        else
                        {
                            throw new NotImplementedException("iso in multi file mode.");
                        }
                        break;
                    case ".xci":
                        if (!string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                            continue;

                        if (singleFileMode)
                        {
                            CopyFile(fileInfo, singleFileOutput);
                            mediaById.SetDumpFile(singleFileOutput);
                            databaseDriver.UpdateMedia(mediaById);
                            AttemptDelete(fileInfo);
                        }
                        else
                        {
                            throw new NotImplementedException("xci in multi file mode.");
                        }
                        break;
                    case ".dvd":
                        if (!string.IsNullOrEmpty(mediaById.CueSheetContent))
                            continue;

                        mediaById.CueSheetContent = File.ReadAllText(fileInfo.FullName);
                        databaseDriver.UpdateMedia(mediaById);
                        AttemptDelete(fileInfo);
                        break;
                    case ".md5":
                        if (!string.IsNullOrEmpty(mediaById.ChecksumContent))
                            continue;

                        mediaById.ChecksumContent = File.ReadAllText(fileInfo.FullName);
                        databaseDriver.UpdateMedia(mediaById);
                        AttemptDelete(fileInfo);
                        break;
                    case ".ibg":
                        if (!string.IsNullOrEmpty(mediaById.GraphDataContent))
                            continue;

                        mediaById.GraphDataContent = File.ReadAllText(fileInfo.FullName);
                        databaseDriver.UpdateMedia(mediaById);
                        AttemptDelete(fileInfo);
                        break;
                    case ".flac":
                        if (singleFileMode)
                        {
                            throw new NotImplementedException("single file .flac");
                        }
                        else
                        {
                            FileInfo outputFileInfo = new FileInfo(Path.Combine(outputDir.FullName, fileInfo.Name));
                            CopyFile(fileInfo, outputFileInfo);
                            AttemptDelete(fileInfo);
                        }
                        break;
                    case ".mkv":
                        if (singleFileMode)
                        {
                            CopyFile(fileInfo, singleFileOutput);
                            mediaById.SetDumpFile(singleFileOutput);
                            databaseDriver.UpdateMedia(mediaById);
                            AttemptDelete(fileInfo);
                        }
                        else
                        {
                            FileInfo outputFileInfo = new FileInfo(Path.Combine(outputDir.FullName, fileInfo.Name));
                            CopyFile(fileInfo, outputFileInfo);
                            AttemptDelete(fileInfo);
                        }
                        break;
                    case ".log":
                        if (!string.IsNullOrEmpty(mediaById.LogfileContent))
                            continue;

                        mediaById.LogfileContent = File.ReadAllText(fileInfo.FullName);
                        databaseDriver.UpdateMedia(mediaById);
                        AttemptDelete(fileInfo);
                        break;
                    case ".cdt":
                        mediaById.CdTextContent = File.ReadAllBytes(fileInfo.FullName);
                        databaseDriver.UpdateMedia(mediaById);
                        AttemptDelete(fileInfo);
                        break;
                    case ".m3u8":
                        if (string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                        {
                            FileInfo outputFileInfo = new FileInfo(Path.Combine(outputDir.FullName, fileInfo.Name));
                            CopyFile(fileInfo, outputFileInfo);
                        }
                        string playlist = File.ReadAllText(fileInfo.FullName);
                        bool deletem3u8 = false;

                        if (string.IsNullOrEmpty(mediaById.PlaylistContent))
                        {
                            mediaById.PlaylistContent = playlist;
                            deletem3u8 = true;
                        }

                        if (string.IsNullOrEmpty(mediaById.MetaFileContent))
                        {
                            mediaById.MetaFileContent = playlist;
                            deletem3u8 = true;
                        }

                        if (deletem3u8)
                            AttemptDelete(fileInfo);

                        databaseDriver.UpdateMedia(mediaById);
                        break;
                    case ".bin":
                        if (fileExtensionDictionary.HasExtension(".3ds") && fileInfo.Length < 1024)
                        {
                            /*if (fileInfo.Name.ToLowerInvariant().EndsWith("-priv.bin"))
                            {
                                mediaById.Priv = File.ReadAllBytes(fileInfo.FullName);
                                databaseDriver.UpdateMedia(mediaById);
                                AttemptDelete(fileInfo);
                                continue;
                            }

                            if (fileInfo.Name.ToLowerInvariant().Equals("jedecid_and_sreg.bin"))
                            {
                                mediaById.JedecId = File.ReadAllBytes(fileInfo.FullName);
                                databaseDriver.UpdateMedia(mediaById);
                                AttemptDelete(fileInfo);
                                continue;
                            }*/
                        }
                        if (!string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                            continue;

                        if (singleFileMode)
                        {
                            CopyFile(fileInfo, singleFileOutput);
                            AttemptDelete(fileInfo);
                        }
                        else
                        {
                            throw new NotImplementedException("bin in multi file mode.");
                        }
                        break;
                    case ".cue":
                        if (string.IsNullOrEmpty(mediaById.DumpStorageSpacePath))
                        {
                            FileInfo outputFileInfo = new FileInfo(Path.Combine(outputDir.FullName, fileInfo.Name));
                            if (!isAudiobook)
                            {
                                CopyFile(fileInfo, outputFileInfo);
                                mediaById.SetDumpFile(outputFileInfo);
                            }
                        }
                        string cuefile = File.ReadAllText(fileInfo.FullName);
                        bool deleteCue = false;

                        if (string.IsNullOrEmpty(mediaById.CueSheetContent))
                        {
                            mediaById.CueSheetContent = cuefile;
                            deleteCue = true;
                        }

                        if (string.IsNullOrEmpty(mediaById.MetaFileContent))
                        {
                            mediaById.MetaFileContent = cuefile;
                            deleteCue = true;
                        }

                        if (deleteCue)
                            AttemptDelete(fileInfo);

                        databaseDriver.UpdateMedia(mediaById);
                        break;
                    case ".txt":
                        if (fileInfo.Name.ToLowerInvariant().Equals("log.txt"))
                            goto case ".log";
                        else
                            MessageBox.Show("Don't know how to deal with TXT file:" + fileInfo.Name);
                        break;
                    case ".wav":
                        if (singleFileMode)
                        {
                            string mp3Filename = Path.ChangeExtension(fileInfo.FullName, ".mp3");
                            string wavFilename = fileInfo.FullName;
                            string lamePath = azusaContext.ReadIniKey("ripkit", "lamePath", null);
                            if (string.IsNullOrEmpty(lamePath))
                            {
                                MessageBox.Show("LAME ist nicht vorhanden!");
                                WorkerForm.InvokeClose();
                                return;
                            }

                            System.Diagnostics.Process lame = new System.Diagnostics.Process();
                            lame.StartInfo.FileName = lamePath;
                            lame.StartInfo.Arguments = String.Format("\"{0}\" \"{1}\"", wavFilename, mp3Filename);
                            lame.Start();
                            WorkerForm.InvokeNextStep(String.Format("Encode: {0}", mp3Filename));
                            lame.WaitForExit();

                            WorkerForm.InvokeNextStep("Kopiere MP3 Datei...");
                            FileInfo mp3Info = new FileInfo(mp3Filename);
                            CopyFile(mp3Info, singleFileOutput);
                            mediaById.SetDumpFile(singleFileOutput);
                            databaseDriver.UpdateMedia(mediaById);

                            System.Threading.Thread.Sleep(100);
                            AttemptDelete(fileInfo);
                            AttemptDelete(mp3Info);

                        }
                        else
                        {
                            throw new NotImplementedException("wav in multi file mode.");
                        }
                        break;
                    default:
                        MessageBox.Show("Don't know how to deal with file:" + fileInfo.Name);
                        break;
                }
            }

            sourceDirectoryInfo.Refresh();
            FileInfo[] filesAfterComplete = sourceDirectoryInfo.GetFiles();
            if (filesAfterComplete.Length == 0)
            {
                sourceDirectoryInfo.Delete(false);
            }

            WorkerForm.InvokeClose();
            MessageBox.Show("Erfolg!");
        }

        private string GuessFolderName(MediaPickerForm mpf)
        {
            if (mpf.NumMediaInProduct == 1)
            {
                return String.Format("{0}", mpf.SelectedProduct.Name);
            }
            else
            {
                return String.Format("{0} - {1}", mpf.SelectedProduct.Name, mpf.SelectedMedia.MediaName);
            }
        }

        private void CopyFile(FileInfo infile, FileInfo outfile, int blocksize = (UInt16.MaxValue + 1) * 2)
        {
            FileStream inStream = infile.OpenRead();
            FileStream outStream = outfile.OpenWrite();
            length = inStream.Length;

            StreamPump streamPump = new StreamPump(inStream, outStream, 0);
            streamPump.SparseCopy = false;
            streamPump.BufferSize = blocksize;
            streamPump.SparseChunkSize = blocksize * 2;
            streamPump.ProgressEvent += StreamPump_ProgressEvent;
            streamPump.Run();
            inStream.Close();
            outStream.Flush(true);
            outStream.Close();
        }

        private void StreamPump_ProgressEvent(object sender, PumpProgressEventArgs e)
        {
            long a = length;
            long b = e.BytesRead;
            while (a > Int32.MaxValue)
            {
                a /= 2;
                b /= 2;
            }
            WorkerForm.InvokeSetCurrentStepProgress((int)b, (int)a);
        }

        private void AttemptDelete(FileInfo fi)
        {
            try
            {
                fi.Delete();
            }
            catch (Exception e)
            {
                MessageBox.Show("Konnte nicht löschen:" + fi.Name);
            }
        }

        private string GuessFilename(MediaPickerForm mpf, string extension)
        {
            if (mpf.NumMediaInProduct == 1)
            {
                return String.Format("{0}{1}", mpf.SelectedProduct.Name, extension);
            }
            else
            {
                return String.Format("{0} - {1}{2}", mpf.SelectedProduct.Name, mpf.SelectedMedia.MediaName, extension);
            }
        }
        
        private FileInfo FindIsoFile(DirectoryInfo directoryInfo)
        {
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                if (fileInfo.Extension.ToLowerInvariant() == ".iso")
                {
                    return fileInfo;
                }
            }

            return null;
        }

        private long length;
        public override string Title => "Automatischer Dateiimport";
        public override int InitialNumberOfSteps => 100;
        public override string InitalizingMessage => "Vorbereiten...";
    }
}
