using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace libazusax360
{
    public class XboxISOFileSource : FileSystemSource
    {
        // Fields
        private XDvdFsFileSystemEntry CurrentDir;
        private long fileSystemOffset;
        private bool hasDMI;
        private bool hasPFI;
        private bool hasSS;
        private readonly string _inputFileName;
        private readonly long inputFileSize;
        private Stream InStream;
        private Format ISOType;
        private XDvdFsFileSystemEntry RootDir;
        private int _rootSectorBlock;
        private int _rootSectorBlockSize;

        public XboxISOFileSource(Stream InputISO, string FileName, long FileSize)
        {
            if (!InputISO.CanRead || !InputISO.CanSeek)
            {
                throw new SystemException("Input stream cannot be used for XBOX ISO");
            }
            this.InStream = InputISO;
            this.inputFileSize = FileSize;
            this._inputFileName = FileName;
            this.InitISO();
        }

        public override bool CanStream()
        {
            return true;
        }
        
        private bool CheckDMIExists()
        {
            return this.CheckRange(this.fileSystemOffset - 0x1000L);
        }

        private bool CheckPFIExists()
        {
            return this.CheckRange(this.fileSystemOffset - 0x1800L);
        }

        private bool CheckRange(long StartByte)
        {
            if (this.fileSystemOffset != 0L)
            {
                this.InStream.Seek(StartByte, SeekOrigin.Begin);
                byte[] buffer = new byte[0x800];
                this.InStream.Read(buffer, 0, 0x800);
                for (int i = 0; i < 0x800; i++)
                {
                    if (buffer[i] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckSSExists()
        {
            return this.CheckRange(this.fileSystemOffset - 0x800L);
        }

        public override void Close()
        {
            this.InStream.Close();
        }

        private void FindISOType()
        {
            Trace.WriteLine("Determining Xbox ISO Type");
            if (this.fileSystemOffset == 0xfd90000L)
            {
                this.SetISOType(Format.Xbox360);
            }
            else if (this.fileSystemOffset == 0x18300000L)
            {
                this.SetISOType(Format.Xbox1);
            }
            else if (this.RootDir.Files.FileExistsNoCase("default.xex"))
            {
                this.SetISOType(Format.Xbox360);
            }
            else if (this.RootDir.Files.FileExistsNoCase("default.xbe"))
            {
                this.SetISOType(Format.Xbox1);
            }
            else
            {
                this.SetISOType(Format.None);
            }
            Trace.WriteLine("ISO Type is set to: " + this.GetISOType().ToString());
        }
        
        public XDvdFsFileSystemEntry GetFileSystem()
        {
            XDvdFsFileSystemEntry tofolder = this.RootDir.ShallowCopy();
            this.RecurseGetFS(this.RootDir, tofolder);
            base.isoXDvdFsFileSystem = tofolder;
            base.ParseDefaultExecutable();
            return tofolder;
        }
        
        public long GetFileSystemOffset()
        {
            return this.fileSystemOffset;
        }
        
        public override Format GetISOType()
        {
            if ((this.CurrentDir != null) && (this.CurrentDir.Files.Count != 0))
            {
                if (this.CurrentDir.Files.FileExistsNoCase("default.xex"))
                {
                    this.SetISOType(Format.Xbox360);
                }
                else if (this.CurrentDir.Files.FileExistsNoCase("default.xbe"))
                {
                    this.SetISOType(Format.Xbox1);
                }
                else
                {
                    this.SetISOType(Format.None);
                }
            }
            return this.ISOType;
        }
        
        public override byte[] GetSS()
        {
            this.InStream.Seek(this.fileSystemOffset - 0x800L, SeekOrigin.Begin);
            byte[] buffer = new byte[0x800];
            this.InStream.Read(buffer, 0, 0x800);
            return buffer;
        }

        public Stream GetStream()
        {
            return this.InStream;
        }
        
        private void InitISO()
        {
            Trace.WriteLine("");
            Trace.WriteLine("New File Loaded by XboxISOFileSource");
            Trace.WriteLine("Initialising XBOX iso");
            Trace.WriteLine("");
            Trace.WriteLine("Finding Filesystem Offset");
            this.fileSystemOffset = new FSOffsetSearcher(this.InStream, this.inputFileSize).FindFSOffset();
            if (this.fileSystemOffset != 0L)
            {
                Trace.WriteLine("Filesystem has a non zero offset");
                Trace.WriteLine("Checking existence of Security Sectors");
                this.hasSS = this.CheckSSExists();
                Trace.WriteLine("SS: " + this.hasSS.ToString());
                this.hasPFI = this.CheckPFIExists();
                Trace.WriteLine("PFI: " + this.hasPFI.ToString());
                this.hasDMI = this.CheckDMIExists();
                Trace.WriteLine("DMI: " + this.hasDMI.ToString());
            }
            this.ReadISOHeader();
            Trace.WriteLine("Reading XDVD FileSystem");
            this.ReadFileSystem();
            this.FindISOType();
        }
        
        private void ReadFileSystem()
        {
            XFSInputFile ifile = new XFSInputFile(this, this.RootSectorBlockSize * 0x800, this.RootSectorBlock);
            this.RootDir = new XDvdFsFileSystemEntry(ifile);
            this.CurrentDir = this.RootDir;
            ISOFileStreamer ifs = new ISOFileStreamer(this.fileSystemOffset, new BinaryReader(this.GetStream()));
            ifs.GetNewFile(ifile);
            byte[] buffer = new byte[this.RootSectorBlockSize * 0x800];
            ifs.Read(buffer, 0, this.RootSectorBlockSize * 0x800);
            this.ReadFileSystemData(buffer, 0, this.RootDir, ifs);
            List<XDvdFsFileSystemEntry> fileList = this.RootDir.GetFileList();
            fileList.Sort(new FSOutSectorComparer());
            XDvdFsFileSystemEntry fse = new XDvdFsFileSystemEntry(new FSFolder("Temp"));
            foreach (XDvdFsFileSystemEntry entry2 in fileList)
            {
                if (((fse.StartSector == entry2.StartSector) && (fse.LogicalFileSize == entry2.LogicalFileSize)) && !entry2.IsFolder)
                {
                    entry2.SetLinkedTo(fse);
                }
                fse = entry2;
            }
        }

        private void ReadFileSystemData(byte[] buffer, int fsOffset, XDvdFsFileSystemEntry parentEntry, ISOFileStreamer ifs)
        {
            if (fsOffset > buffer.Length)
            {
                throw new SystemException("Disaster");
            }
            XFSInputFile ifile = new XFSInputFile(this, buffer, fsOffset);
            XDvdFsFileSystemEntry entry = new XDvdFsFileSystemEntry(ifile);
            entry.ParentNode = parentEntry;
            ushort num = BitConverter.ToUInt16(buffer, fsOffset);
            ushort num2 = BitConverter.ToUInt16(buffer, fsOffset + 2);
            if (ifile.StartSector == -1)
            {
                throw new SystemException("Invalid TOC entry");
            }
            if (num != 0)
            {
                if ((num * 4) > buffer.Length)
                {
                    throw new SystemException("Disaster 2!");
                }
                this.ReadFileSystemData(buffer, num * 4, parentEntry, ifs);
            }
            if (ifile.IsFolder())
            {
                if ((ifile.StartSector == 0) || (ifile.GetFileLength() == 0L))
                {
                    parentEntry.Files.Add(entry);
                }
                else
                {
                    byte[] buffer2 = new byte[ifile.GetFileLength()];
                    ifs.GetNewFile(ifile);
                    ifs.Read(buffer2, 0, (int)ifile.GetFileLength());
                    parentEntry.Files.Add(entry);
                    this.ReadFileSystemData(buffer2, 0, entry, ifs);
                }
            }
            else
            {
                parentEntry.Files.Add(entry);
            }
            if (num2 != 0)
            {
                if ((num2 * 4) > buffer.Length)
                {
                    throw new SystemException("Disaster 3!");
                }
                this.ReadFileSystemData(buffer, num2 * 4, parentEntry, ifs);
            }
        }

        private void ReadISOHeader()
        {
            Trace.WriteLine("Reading ISO header");
            this.InStream.Seek(this.fileSystemOffset + 0x10000L, SeekOrigin.Begin);
            byte[] buffer = new byte[0x800];
            this.InStream.Read(buffer, 0, 0x800);
            Trace.WriteLine("Reading disc magic");
            Encoding.ASCII.GetString(buffer, 0, 20);
            if (Encoding.ASCII.GetString(buffer, 0, 20) != "MICROSOFT*XBOX*MEDIA")
            {
                Trace.WriteLine("Magic not found");
                throw new InvalidDataException();
            }
            Trace.WriteLine("MICROSOFT*XBOX*MEDIA string found, reading details");
            this._rootSectorBlock = BitConverter.ToInt32(buffer, 20);
            Trace.WriteLine("Root sector block at " + this._rootSectorBlock.ToString());
            int num = BitConverter.ToInt32(buffer, 0x18);
            Trace.WriteLine("Root size is " + num.ToString());
            if ((num % 0x800) != 0)
            {
                num += 0x800 - (num % 0x800);
            }
            this._rootSectorBlockSize = num / 0x800;
        }

        private void RecurseGetFS(XDvdFsFileSystemEntry fromfolder, XDvdFsFileSystemEntry tofolder)
        {
            foreach (XDvdFsFileSystemEntry entry in fromfolder.Files)
            {
                XDvdFsFileSystemEntry entry2 = entry.ShallowCopy();
                tofolder.Files.Add(entry2);
                if (entry.IsFolder)
                {
                    this.RecurseGetFS(entry, entry2);
                }
            }
        }
        
        public void SetISOType(Format ISOType)
        {
            this.ISOType = ISOType;
        }

        public override void SetSS(byte[] ss)
        {
            if (this.fileSystemOffset == 0L)
            {
                throw new SystemException("Cant set ss if there is no video partition");
            }
            if (ss.Length != 0x800)
            {
                throw new SystemException("SS is wrong size");
            }
            this.InStream.Seek(this.fileSystemOffset - 0x800L, SeekOrigin.Begin);
            this.InStream.Write(ss, 0, 0x800);
            this.hasSS = true;
        }

        public override string ToString()
        {
            return this.InputFileName;
        }

        // Properties
        public string InputFileName
        {
            get
            {
                return this._inputFileName;
            }
        }
        
        public int RootSectorBlock
        {
            get
            {
                return this._rootSectorBlock;
            }
        }

        public int RootSectorBlockSize
        {
            get
            {
                return this._rootSectorBlockSize;
            }
        }

        public static XboxISOFileSource TryOpen(Stream inStream)
        {
            string fname = "azusa_dummy.iso";
            if (inStream is FileStream fileStream)
            {
                inStream.Position = 0;
                fname = Path.GetFileName(fileStream.Name);
            }

            try
            {
                XboxISOFileSource result = new XboxISOFileSource(inStream, fname, inStream.Length);
                return result;
            }
            catch (Exception)
            {
                inStream.Position = 0;
                return null;
            }
            
        }
    }
}