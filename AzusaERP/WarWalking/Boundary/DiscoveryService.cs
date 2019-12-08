using System.Collections.Generic;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Boundary
{
    static class DiscoveryService
    {
        public static bool IsAccessPointKnown(string bssid)
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_IsAccessPointKnown(bssid);
        }

        public static Discovery GetByBssid(string bssid)
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_GetByBssid(bssid);
        }

        public static void UpdateDiscovery(Discovery discovery)
        {
            AzusaContext.GetInstance().DatabaseDriver.WarWalking_UpdateDiscovery(discovery); }

        public static void AddAccessPoint(Discovery discovery)
        {
            AzusaContext.GetInstance().DatabaseDriver.WarWalking_AddAccessPoint(discovery);
        }

        public static IEnumerable<Discovery> GetByTour(Tour tour)
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_GetDiscoveriesByTour(tour);
        }
    }
}
