using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace moe.yo3explorer.azusa.Control.Galleria
{
    class DefaultGalleriaModel : IGalleriaModel, IList<Image>
    {
        public DefaultGalleriaModel()
        {
            images = new List<Image>();
        }
        private List<Image> images;

        public Galleria Galleria { get; set; }
        public int ImagesCount => images.Count;
        public Image GetImage(int ordinal)
        {
            return images[ordinal];
        }

        public IEnumerator<Image> GetEnumerator()
        {
            return images.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return images.GetEnumerator();
        }

        public void Add(Image item)
        {
            if (images.Count == 0)
                Galleria.CurrentImageNo = 0;
            images.Add(item);
            Galleria.UpdateControls();
        }

        public void AddRange(IEnumerable<Image> items)
        {
            if (images.Count == 0)
                Galleria.CurrentImageNo = 0;
            foreach (Image image in items)
                images.Add(image);
            Galleria.UpdateControls();
        }

        public void Clear()
        {
            foreach (Image image in images)
            {
                image.Dispose();
            }
            images.Clear();
            Galleria.UpdateControls();
        }

        public bool Contains(Image item)
        {
            return images.Contains(item);
        }

        public void CopyTo(Image[] array, int arrayIndex)
        {
            images.CopyTo(array, arrayIndex);
        }

        public int Count => images.Count;
        public bool IsReadOnly => false;
        public int IndexOf(Image item)
        {
            return images.IndexOf(item);
        }

        public bool Remove(Image item)
        {
            item.Dispose();
            return images.Remove(item);
        }

        public void Insert(int index, Image item)
        {
            images.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            images[index].Dispose();
            images.RemoveAt(index);
        }

        public Image this[int index]
        {
            get => images[index];
            set => images[index] = value;
        }
    }

    class EmptyGalleriaModel : IGalleriaModel
    {
        public int ImagesCount => 0;
        public Image GetImage(int ordinal)
        {
            return null;
        }

        public Galleria Galleria { get; set; }
    }
}
