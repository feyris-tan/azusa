using System;
using System.Collections.Generic;
using System.IO;

namespace libazusax360
{
    [Serializable]
    public class XDvdFsFileSystemEntry
    {
        // Fields
        private byte _attributes;
        private OutputMethod _copymethod;
        private string filename;
        public FileSystemFolder Files;
        private InputFile infile;
        private XDvdFsFileSystemEntry linkedTo;
        private long logicalfilesize;
        public XDvdFsFileSystemEntry ParentNode;
        private int startsector;

        // Methods
        public XDvdFsFileSystemEntry(FSFolder fsfolder)
        {
            this.startsector = -1;
            this.setinfile(fsfolder);
        }

        public XDvdFsFileSystemEntry(InputFile ifile)
        {
            this.setinfile(ifile);
        }

        public XDvdFsFileSystemEntry(XFSInputFile ifile)
        {
            this.StartSector = ifile.StartSector;
            this.setinfile(ifile);
        }

        public List<XDvdFsFileSystemEntry> GetFileList()
        {
            return this.GetFileList(false);
        }

        public List<XDvdFsFileSystemEntry> GetFileList(bool FoldersLast)
        {
            List<XDvdFsFileSystemEntry> fileSystemArray = new List<XDvdFsFileSystemEntry>();
            fileSystemArray.Add(this);
            this.WalkDir(this, fileSystemArray, FoldersLast);
            return fileSystemArray;
        }

        public InputFile GetInfile()
        {
            return this.infile;
        }

        public string GetPathAsString(bool ShowRoot)
        {
            XDvdFsFileSystemEntry parentNode = this;
            string str = "";
            while (parentNode.ParentNode != null)
            {
                parentNode = parentNode.ParentNode;
                if (ShowRoot || (parentNode.ParentNode != null))
                {
                    str = parentNode.FileName + "/" + str;
                }
            }
            return str;
        }

        public Stream GetStream()
        {
            return this.infile.GetStream();
        }

        private void setinfile(InputFile ifile)
        {
            this.infile = ifile;
            this.filename = this.infile.GetFileName();
            if (!ifile.IsFolder())
            {
                this.logicalfilesize = this.infile.GetFileLength();
            }
            else
            {
                this.Files = new FileSystemFolder(this);
            }
            this._attributes = this.infile.GetAttributes();
        }

        public void SetInFile(InputFile ifile)
        {
            this.infile = ifile;
        }

        public void SetLinkedTo(XDvdFsFileSystemEntry fse)
        {
            if (this._copymethod != OutputMethod.Crosslink)
            {
                this.ParentNode.Files.UpdateSizes(0, 0, (long)(((int)this.LogicalFileSize) * -1));
            }
            this.linkedTo = fse;
            this._copymethod = OutputMethod.Crosslink;
        }

        public XDvdFsFileSystemEntry ShallowCopy()
        {
            XDvdFsFileSystemEntry entry = new XDvdFsFileSystemEntry(this.GetInfile());
            entry.Attributes = this.Attributes;
            entry.StartSector = this.StartSector;
            entry.CopyMethod = this.CopyMethod;
            if (this.CopyMethod == OutputMethod.Crosslink)
            {
                entry.SetLinkedTo(this.linkedTo);
            }
            entry.FileName = this.FileName;
            return entry;
        }

        private void UpdateParent()
        {
            if (this.ParentNode != null)
            {
                this.ParentNode.Files.UpdateFolder();
            }
        }
        
        private void WalkDir(XDvdFsFileSystemEntry fn, List<XDvdFsFileSystemEntry> FileSystemArray, bool folderslast)
        {
            foreach (XDvdFsFileSystemEntry entry in fn.Files)
            {
                if (!entry.IsFolder || !folderslast)
                {
                    FileSystemArray.Add(entry);
                    if (entry.IsFolder && (entry.Files.Count != 0))
                    {
                        this.WalkDir(entry, FileSystemArray, folderslast);
                    }
                }
            }
            if (folderslast)
            {
                foreach (XDvdFsFileSystemEntry entry2 in fn.Files)
                {
                    if (entry2.IsFolder)
                    {
                        FileSystemArray.Add(entry2);
                        if (entry2.Files.Count != 0)
                        {
                            this.WalkDir(entry2, FileSystemArray, folderslast);
                        }
                    }
                }
            }
        }

        // Properties
        public byte Attributes
        {
            get => this._attributes;
            set => this._attributes = value;
        }
        
        public OutputMethod CopyMethod
        {
            get => this._copymethod;
            set => this._copymethod = value;
        }

        public string FileName
        {
            get => this.filename;
            set
            {
                if (this.ParentNode != null)
                {
                    this.ParentNode.Files.RenameFile(this.filename, value);
                    this.filename = value;
                    this.UpdateParent();
                }
                else
                {
                    this.filename = value;
                }
            }
        }

        public string FullPath => (this.GetPathAsString(false) + "/" + this.filename);

        public bool IsFolder
        {
            get
            {
                if ((this.infile != null) && !this.infile.IsFolder())
                {
                    return false;
                }
                return true;
            }
        }

        public long LogicalFileSize
        {
            get
            {
                if (this._copymethod == OutputMethod.Crosslink)
                {
                    return this.linkedTo.LogicalFileSize;
                }
                if (this.IsFolder)
                {
                    return this.Files.LogicalDirTableSize;
                }
                return this.logicalfilesize;
            }
            set
            {
                if (this._copymethod == OutputMethod.Crosslink)
                {
                    this.linkedTo.LogicalFileSize = value;
                }
                else if (!this.IsFolder)
                {
                    this.logicalfilesize = value;
                }
            }
        }
        
        public long SourceFileLength => this.infile.GetFileLength();

        public int StartSector
        {
            get
            {
                if (this._copymethod == OutputMethod.Crosslink)
                {
                    return this.linkedTo.StartSector;
                }
                return this.startsector;
            }
            set
            {
                if ((this._copymethod == OutputMethod.Special) || (this._copymethod == OutputMethod.UnMoveable))
                {
                    throw new SystemException("Special files are UNMOVABLE!");
                }
                if (this._copymethod == OutputMethod.Crosslink)
                {
                    this.linkedTo.StartSector = value;
                }
                else
                {
                    this.startsector = value;
                    this.UpdateParent();
                }
            }
        }

        public override string ToString()
        {
            return this.filename;
        }
    }
}