using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    class ShelfTabPage : TabPage
    {
        public ShelfTabPage(Shelf shelf)
        {
            this.shelf = shelf;
        }

        private Shelf shelf;

        public override string Text
        {
            get
            {
                return shelf.Name;
            }
            set
            {
            }
        }

        public Shelf Shelf
        {
            get
            {
                return shelf;
            }
        }
    }
}
