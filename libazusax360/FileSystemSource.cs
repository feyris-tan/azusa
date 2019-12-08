using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace libazusax360
{
    public abstract class FileSystemSource
    {
        // Fields
        private string GameFolder;
        private string GameName;
        private bool hasdefaultexe;
        private bool HasMediaID;
        private bool HasRegionFlags;
        public XDvdFsFileSystemEntry isoXDvdFsFileSystem;
        private byte[] MediaID = new byte[0x10];
        private bool paddingchecked;
        private bool VideoChecked;
        private bool VideoPresent;
        private int VideoSize;
        private bool VideoSizeChecked;
        private bool zeropadding;
        
        public abstract bool CanStream();
        
        public static bool CheckPFI(byte[] PFI)
        {
            Trace.WriteLine("Checking PFI values");
            SCSIDevice.DiscData data = new SCSIDevice.DiscData(PFI, 0);
            if (data.StartSectorNumber == 0x30000)
            {
                Trace.WriteLine("PFI Validated (Start sector is ok)");
                return true;
            }
            Trace.WriteLine("PFI invalid! (Start sector is " + data.StartSectorNumber.ToString() + ")");
            return false;
        }
        
        public static bool CheckXbox1SS(byte[] SS)
        {
            if (SimpleCheckSS(SS))
            {
                Trace.WriteLine("Checking if Hitachi SS file");
                if (((SS[720] == 0) && (SS[0x2d1] == 0)) && ((SS[0x2d2] == 0) && (SS[0x2d3] == 0)))
                {
                    Trace.WriteLine("Hitachi SS found");
                    return false;
                }
                Trace.WriteLine("Hitachi SS not found");
                Trace.WriteLine("Checking SS Layerbreak");
                SCSIDevice.DiscData data = new SCSIDevice.DiscData(SS, 0);
                if (data.Layer0LastSectorLBA == 0x1a2db0)
                {
                    Trace.WriteLine("SS information valid - Layerbreak is correct");
                    return true;
                }
                Trace.WriteLine("SS information valid - Layerbreak is incorrect");
            }
            return false;
        }

        public abstract void Close();
        
        public abstract Format GetISOType();
        
        public static ISOPartitionDetails GetPartitionDetails(byte[] ss, byte[] pfi, Format ISOType, int VideoSize)
        {
            Trace.WriteLine("Getting partition Details from ISO");
            ISOPartitionDetails details = new ISOPartitionDetails();
            SCSIDevice.DiscData data = new SCSIDevice.DiscData(ss, 0);
            SCSIDevice.DiscData data2 = new SCSIDevice.DiscData(pfi, 0);
            details.VideoPartitionSize = data.StartSectorNumber - data2.StartSectorNumber;
            Trace.WriteLine("Video partition size: " + details.VideoPartitionSize.ToString());
            details.GamePartitionSize = data.Layer0LastSectorLBA * 2;
            Trace.WriteLine("Game partition size: " + details.GamePartitionSize.ToString());
            details.LayerBreak = data.Layer0LastSectorLBA + details.VideoPartitionSize;
            Trace.WriteLine("Layerbreak: " + details.LayerBreak.ToString());
            Trace.WriteLine("Video Layer 0 size: " + details.VideoLayer0Size.ToString());
            details.VideoFileSize = (uint)VideoSize;
            Trace.WriteLine("Video is size: " + VideoSize);
            if ((details.VideoFileSize <= 0) || (details.VideoFileSize > 0x16e360))
            {
                if (ISOType == Format.Xbox360)
                {
                    details.VideoFileSize = 0xdbf;
                    details.VideoLayer1Size = 0x32f;
                }
                else
                {
                    details.VideoFileSize = 0x1b50;
                    details.VideoLayer1Size = 0xa1;
                }
            }
            else
            {
                details.VideoLayer1Size = details.VideoFileSize - data2.Layer0LastSectorLBA;
            }
            details.VideoLayer0Size = data2.Layer0LastSectorLBA;
            details.VideoPadding = details.VideoPartitionSize - details.VideoLayer0Size;
            details.TotalSplitVidISOSize = ((details.VideoPartitionSize + details.GamePartitionSize) + details.VideoPadding) + details.VideoLayer1Size;
            details.TotalISOSize = details.VideoPartitionSize + details.GamePartitionSize;
            return details;
        }
        
        public abstract byte[] GetSS();
        
        public static int GetVideoSize(Stream vs)
        {
            Trace.WriteLine("Checking for video file size");
            vs.Seek(0x8000L, SeekOrigin.Begin);
            byte[] buffer = new byte[0x800];
            vs.Read(buffer, 0, 0x800);
            vs.Close();
            return BitConverter.ToInt32(buffer, 80);
        }
        
        public void ParseDefaultExecutable()
        {
            this.hasdefaultexe = false;
            this.HasMediaID = false;
            this.HasRegionFlags = false;
            XDvdFsFileSystemEntry sourceXDvdFsFile = new XDvdFsFileSystemEntry(new FSFolder("Temp"));
            if (this.GetISOType() == Format.Xbox360)
            {
                sourceXDvdFsFile = this.isoXDvdFsFileSystem.Files.GetFileByNameNoCase("default.xex");
            }
            else
            {
                sourceXDvdFsFile = this.isoXDvdFsFileSystem.Files.GetFileByNameNoCase("default.xbe");
            }
            if (sourceXDvdFsFile != null)
            {
                this.hasdefaultexe = true;
                this.HasMediaID = true;
                this.HasRegionFlags = true;
                Trace.WriteLine(sourceXDvdFsFile.FileName + " found, processing");
                if (!this.CanStream())
                {
                    FileWriter writer = new FileWriter();
                    Trace.WriteLine("Source Filesystem cannot be streamed from, copying executable to temp folder");
                    string destination = AppDomain.CurrentDomain.BaseDirectory + @"temp\" + Path.GetRandomFileName();
                    Trace.WriteLine("Copying to " + destination);
                    writer.SaveFileToDisk(sourceXDvdFsFile, destination);
                    sourceXDvdFsFile.SetInFile(new FSFile(destination));
                }
                if (this.GetISOType() == Format.Xbox360)
                {
                    Stream infile = sourceXDvdFsFile.GetStream();
                    XEXFile file = new XEXFile(infile);
                    Trace.WriteLine("Xex MediaID = " + BitConverter.ToString(file.MediaID));
                    file.MediaID.CopyTo(this.MediaID, 0);
                    infile.Close();
                }
                else
                {
                    Stream stream = sourceXDvdFsFile.GetStream();
                    XBEFile file2 = new XBEFile(stream);
                    this.MediaID = new byte[0x10];
                    stream.Close();
                }
            }
        }
        
        public abstract void SetSS(byte[] ss);

        public static bool SimpleCheckSS(byte[] SS)
        {
            Trace.WriteLine("Checking SS information..");
            SCSIDevice.DiscData data = new SCSIDevice.DiscData(SS, 0);
            if ((data.Layer0LastSectorLBA == 0x1b3880) || (data.Layer0LastSectorLBA == 0x1a2db0))
            {
                Trace.WriteLine("SS information valid - Layerbreak is correct");
                return true;
            }
            Trace.WriteLine("SS information valid - Layerbreak is incorrect");
            return false;
        }

        new public abstract string ToString();
    }
}