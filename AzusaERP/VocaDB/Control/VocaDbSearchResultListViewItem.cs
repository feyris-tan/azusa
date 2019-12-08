using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.VocaDB.Entity;

namespace moe.yo3explorer.azusa.VocaDB.Control
{
    class VocaDbSearchResultListViewItem : ListViewItem
    {
        public VocaDbSearchResultListViewItem(VocadbSearchResult wrapped)
        {
            this.wrapped = wrapped;
            this.Text = wrapped.Name;
            this.SubItems.Add(wrapped.ArtistString);
            this.SubItems.Add(wrapped.DiscType);
            this.SubItems.Add(wrapped.CatalogNumber);
            if (wrapped.ReleaseDate != DateTime.MinValue)
                this.SubItems.Add(wrapped.ReleaseDate.ToShortDateString());
            else
                this.SubItems.Add("");

            if (string.IsNullOrEmpty(wrapped.CatalogNumber))
                this.ImageIndex = 1;
            else
                this.ImageIndex = 0;
        }

        public VocadbSearchResult wrapped { get; set; }
    }
}
