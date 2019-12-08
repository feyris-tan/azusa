using System.Collections.Generic;
using System.ComponentModel;

namespace libazusax360
{
    public class FileSystemFolder : BindingList<XDvdFsFileSystemEntry>
    {
        // Fields
        private Dictionary<string, XDvdFsFileSystemEntry> FileNameLookup = new Dictionary<string, XDvdFsFileSystemEntry>(1);
        private XDvdFsFileSystemEntry fse;
        private long _logicalfilesinfoldersize;
        private bool modified = true;
        private byte[] rawFolder;
        private int _totaldircount;
        private int _totalfilecount;

        // Methods
        public FileSystemFolder(XDvdFsFileSystemEntry fse)
        {
            this.fse = fse;
        }

        new public void Add(XDvdFsFileSystemEntry entry)
        {
            int fileCountInc = 0;
            long num2 = 0L;
            int dirCountInc = 0;
            long fileSizeInc = 0L;
            int num5 = 1;
            if (this.FileNameLookup.ContainsKey(entry.FileName))
            {
                while (this.FileNameLookup.ContainsKey(entry.FileName + num5.ToString()))
                {
                    num5++;
                }
                entry.FileName = entry.FileName + num5.ToString();
            }
            entry.ParentNode = this.fse;
            if (entry.IsFolder)
            {
                dirCountInc++;
                fileSizeInc += entry.Files.LogicalSizeOfFilesInFolder;
                num2 += entry.Files.PhysicalSizeOfFilesInFolder;
                fileCountInc += entry.Files.TotalFileCount;
                dirCountInc += entry.Files.TotalDirCount;
                fileSizeInc += entry.Files.LogicalDirTableSize;
                num2 += FSManipulator.roundup2048(entry.Files.LogicalDirTableSize);
            }
            else
            {
                fileCountInc++;
                fileSizeInc += entry.SourceFileLength;
                num2 += FSManipulator.roundup2048(entry.SourceFileLength);
            }
            this._logicalfilesinfoldersize += fileSizeInc;
            this._totalfilecount += fileCountInc;
            this._totaldircount += dirCountInc;
            base.Add(entry);
            this.FileNameLookup.Add(entry.FileName, entry);
            if (this.fse.ParentNode != null)
            {
                this.fse.ParentNode.Files.UpdateSizes(fileCountInc, dirCountInc, fileSizeInc);
            }
            this.UpdateFolder();
        }
        
        public bool FileExistsNoCase(string FileName)
        {
            string str = FileName.ToUpper();
            foreach (string str2 in this.FileNameLookup.Keys)
            {
                if (str2.ToUpper() == str)
                {
                    return true;
                }
            }
            return false;
        }

        public XDvdFsFileSystemEntry GetFileByNameNoCase(string FileName)
        {
            string str = FileName.ToUpper();
            foreach (string str2 in this.FileNameLookup.Keys)
            {
                if (str2.ToUpper() == str)
                {
                    return this.FileNameLookup[str2];
                }
            }
            return null;
        }

        public byte[] GetRawFolder()
        {
            if (this.modified)
            {
                this.modified = false;
                this.rawFolder = new FSFolderOutputter().DirContentsToByteArray(this);
            }
            return this.rawFolder;
        }

        public void RenameFile(string FileName, string NewFileName)
        {
            XDvdFsFileSystemEntry entry = this.FileNameLookup[FileName];
            this.FileNameLookup.Remove(FileName);
            this.FileNameLookup.Add(NewFileName, entry);
        }

        public void UpdateFolder()
        {
            this.modified = true;
            if (this.fse.ParentNode != null)
            {
                this.fse.ParentNode.Files.UpdateFolder();
            }
        }

        public void UpdateSizes(int fileCountInc, int dirCountInc, long fileSizeInc)
        {
            this._totaldircount += dirCountInc;
            this._totalfilecount += fileCountInc;
            this._logicalfilesinfoldersize += fileSizeInc;
            if (this.fse.ParentNode != null)
            {
                this.fse.ParentNode.Files.UpdateSizes(fileCountInc, dirCountInc, fileSizeInc);
            }
        }

        // Properties
        public long LogicalDirTableSize
        {
            get
            {
                if (!this.modified)
                {
                    return (long)this.rawFolder.Length;
                }
                return (long)this.GetRawFolder().Length;
            }
        }

        public long LogicalSizeOfFilesInFolder
        {
            get
            {
                return this._logicalfilesinfoldersize;
            }
        }

        public long PhysicalSizeOfFilesInFolder
        {
            get
            {
                long num = FSManipulator.roundup2048(this.LogicalDirTableSize);
                foreach (XDvdFsFileSystemEntry entry in this)
                {
                    if (entry.IsFolder)
                    {
                        num += entry.Files.PhysicalSizeOfFilesInFolder;
                    }
                    else if (entry.CopyMethod != OutputMethod.Crosslink)
                    {
                        num += FSManipulator.roundup2048(entry.LogicalFileSize);
                    }
                }
                return num;
            }
        }

        public int TotalDirCount
        {
            get
            {
                return this._totaldircount;
            }
        }

        public int TotalFileCount
        {
            get
            {
                return this._totalfilecount;
            }
        }
    }
}