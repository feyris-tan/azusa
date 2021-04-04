using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.OfflineReaders.MyFigureCollection.Entity;

namespace moe.yo3explorer.azusa.OfflineReaders.MyFigureCollection.Boundary
{
    class FigureListViewItem : ListViewItem
    {
        public FigureListViewItem(int ordinal, Figure figure, bool lookCheaper)
        {
            this.ordinal = ordinal;
            this.figure = figure;

            this.Text = figure.Name;
            this.SubItems.Add(figure.RootName);
            this.SubItems.Add(figure.CategoryName);
            this.SubItems.Add(figure.Barcode);
            this.SubItems.Add(figure.ReleaseDate.HasValue ? figure.ReleaseDate.Value.ToShortDateString() : "");
            if (figure.Price != 0)
                if (lookCheaper)
                    this.SubItems.Add(String.Format("{0:#.##} €", (double)(figure.Price / 100) * 0.99));
                else
                    this.SubItems.Add(String.Format("{0} ¥", figure.Price));

            this.ImageIndex = ordinal;
            if (ordinal < 15)
                this.StateImageIndex = ordinal;
        }

        private int ordinal;
        private Figure figure;

        public Figure WrappedFigure => figure;
    }
}
