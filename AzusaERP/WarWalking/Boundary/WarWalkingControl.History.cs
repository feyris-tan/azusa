using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.WarWalking.Control;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Boundary
{
    public partial class WarWalkingControl
    {
        public void OnLoad()
        {
            AzusaContext.GetInstance().Splash.SetLabel("Abfragen der Warwalking Touren...");

            TreeNode toursRoot = new TreeNode("Touren");
            TreeNode lastTour = null;
            foreach (Tour tour in TourService.GetAllTours())
            {
                lastTour = new TourNode(tour);
                toursRoot.Nodes.Add(lastTour);
            }
            toursRoot.Expand();

            treeView1.Nodes.Add(toursRoot);
            if (lastTour != null)
                treeView1.SelectedNode = lastTour;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (e.Node != null)
                return;

            TourNode tourNode = (TourNode)e.Node;
            historyListView.Items.Clear();
            foreach(Discovery discovery in DiscoveryService.GetByTour(tourNode.Tour))
            {
                historyListView.Items.Add(new DiscoveryListViewItem(discovery));
            }
        }

        private void historyListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
