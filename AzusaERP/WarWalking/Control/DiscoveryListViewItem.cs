using System.Windows.Forms;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Control
{
    class DiscoveryListViewItem : ListViewItem
    {
        public DiscoveryListViewItem(Discovery discovery)
        {
            Discovery = discovery;
            Text = discovery.Bssid;
            SubItems.Add(discovery.Ssid);
            SubItems.Add(discovery.Rssi.ToString());
            SubItems.Add(discovery.DefaultCipherAlgorithm.ToString());
            SubItems.Add(discovery.DefaultAuthAlgorithm.ToString());

        }

        public Discovery Discovery { get; private set; }
    }
}
