﻿using System.Drawing;
using System.IO;
using libazustreamblob;
using moe.yo3explorer.azusa.Control.Galleria;

namespace moe.yo3explorer.azusa.Gelbooru.Control
{
    class GelbooruGalleriaModel : IGalleriaModel
    {
        public AzusaStreamBlob StreamBlob { get; set; }

        public int ImagesCount
        {
            get
            {
                if (Ids == null)
                    return 0;
                return Ids.Length;
            }
        }

        public Image GetImage(int ordinal)
        {
            try
            {
                byte[] buffer = StreamBlob.Get(1, 1, Ids[ordinal]);
                MemoryStream ms = new MemoryStream(buffer, false);
                return Image.FromStream(ms);
            }
            catch (IOException)
            {
                return null;
            }
            
        }

        public Galleria Galleria { get; set; }
        public int[] Ids { get; set; }
    }
}
