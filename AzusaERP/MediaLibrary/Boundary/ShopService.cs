using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class ShopService
    {
        public static IEnumerable<Shop> GetAllShops()
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetAllShops();
        }
    }
}
