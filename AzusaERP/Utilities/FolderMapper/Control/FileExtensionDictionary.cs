using System.Collections.Generic;
using System.IO;

namespace moe.yo3explorer.azusa.Utilities.FolderMapper.Control
{
    public class FileExtensionDictionary
    {
        public FileExtensionDictionary()
        {
            wrapped = new Dictionary<string, int>();
            assigned = new Dictionary<string, FileInfo>();
        }

        private Dictionary<string, int> wrapped;
        private Dictionary<string, FileInfo> assigned;

        public void CountFile(FileInfo fi)
        {
            string ext = Path.GetExtension(fi.Name).ToLowerInvariant();
            if (wrapped.ContainsKey(ext))
                wrapped[ext]++;
            else
                wrapped[ext] = 1;

            assigned[ext] = fi;
        }

        public int CountExtensions(string extension)
        {
            extension = extension.ToLowerInvariant();
            if (wrapped.ContainsKey(extension))
                return wrapped[extension];
            else
                return 0;
        }

        public FileInfo GetFileFromExtension(string extension)
        {
            extension = extension.ToLowerInvariant();
            if (CountExtensions(extension) == 1)
                return assigned[extension];
            else
                return null;
        }

        public DirectoryInfo RootDirectory
        {
            get
            {
                foreach (var keyValuePair in assigned)
                    if (keyValuePair.Value != null)
                        if (keyValuePair.Value.Exists)
                            if (keyValuePair.Value.Directory != null)
                                if (keyValuePair.Value.Directory.Exists)
                                    return keyValuePair.Value.Directory;
                return null;
            }
        }

        public bool HasExtension(string extension)
        {
            extension = extension.ToLowerInvariant();
            return CountExtensions(extension) > 0;
        }

        public int DifferentExtension => wrapped.Count;

        public int TotalFiles
        {
            get
            {
                int result = 0;
                foreach(KeyValuePair<string,int> entry in wrapped)
                {
                    result += entry.Value;
                }
                return result;
            }
        }

        public double GetPercentageOfExtension(string extension)
        {
            if (!wrapped.ContainsKey(extension))
                return 0;

            double totalFiles = (double)TotalFiles;
            double numFiles = (double)wrapped[extension];
            return (numFiles * 100.0) / totalFiles;
        }
    }
}
