using System;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class ProductInShelf : ListViewItem
    {
        public int Id { get; set; }
        public int IconId { get; set; }
        public new string Name { get; set; }
        public double Price { get; set; }
        public int NumberOfDiscs { get; set; }
        public bool ContainsUndumped { get; set; }
        public Shelf relatedShelf { get; set; }
        public DateTime BoughtOn { get; set; }
        public long ScreenshotSize { get; set; }
        public int MissingGraphData { get; set; }
        public long CoverSize { get; set; }
        public bool NSFW { get; set; }

        public string Errors()
        {
            string result = "";
            if (CoverSize == 0)
                result += "C";
            if (ContainsUndumped)
                result += "D";
            if (MissingGraphData > 0)
                result += "G";
            if (ScreenshotSize == 0 && relatedShelf.ScreenshotRequired)
                result += "S";
            if (NumberOfDiscs == 0)
                result += "0";
            return result;
        }

        public void UpdateListViewItem()
        {
            this.Text = Name;
            this.ImageIndex = IconId;

            while (SubItems.Count > 1)
                SubItems.RemoveAt(1);

            if (Price > 0)
            {
                double rounded = Math.Round(Price, 2);
                this.SubItems.Add(String.Format("{0} €", rounded));
            }
            else
                this.SubItems.Add("");

            this.SubItems.Add(NumberOfDiscs.ToString());

            if (BoughtOn.Year > 1980)
                this.SubItems.Add(BoughtOn.ToShortDateString());
            else
                this.SubItems.Add("");

            this.SubItems.Add(Errors());
        }
    }
}
