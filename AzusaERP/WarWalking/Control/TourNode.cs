using System.Windows.Forms;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Control
{
    class TourNode : TreeNode
    {
        public TourNode(Tour tour)
        {
            this.Tour = tour;
            this.Text = tour.Name;
        }

        public Tour Tour { get; private set; }
    }
}
