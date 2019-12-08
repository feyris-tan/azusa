using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Steamdump
{
    public struct SteamdumpGameMetadata
    {

        public int appId;
        public string name;
        public FileInfo acfFile;
        public DirectoryInfo rootDirectory;

        public SteamdumpGameMetadata(int appId, string name, FileInfo acfFile, DirectoryInfo rootDirectory)
        {
            this.appId = appId;
            this.name = name;
            this.acfFile = acfFile;
            this.rootDirectory = rootDirectory;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
