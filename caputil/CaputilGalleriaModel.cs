using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moe.yo3explorer.azusa.Control.Galleria;

namespace caputil
{
    class CaputilGalleriaModel : IGalleriaModel, IDisposable
    {
        public CaputilGalleriaModel()
        {
            fileInfos = new List<FileInfo>();
            lastOrdinal = -1;
        }

        private List<FileInfo> fileInfos;

        public List<FileInfo> FileInfos => fileInfos;

        public int ImagesCount => fileInfos.Count;

        private Image lastImage;
        private int lastOrdinal;
        public Image GetImage(int ordinal)
        {
            if (lastOrdinal != -1)
                lastImage.Dispose();
            lastImage = Image.FromFile(fileInfos[ordinal].FullName);
            lastOrdinal = ordinal;
            return lastImage;
        }

        public Galleria Galleria { get; set; }

        public void Sort()
        {
            fileInfos.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
        }

        public void Dispose()
        {
            if (lastImage != null)
                lastImage.Dispose();
        }
    }
}
