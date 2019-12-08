using System.Collections.Generic;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Boundary
{
    static class TourService
    {
        public static bool IsTourKnown(long hash)
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_IsTourKnown(hash);
        }

        public static int CreateTour(long hash, int recordStart, string name)
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_InsertTourAndReturnId(hash, recordStart, name);
        }

        public static IEnumerable<Tour> GetAllTours()
        {
            return AzusaContext.GetInstance().DatabaseDriver.WarWalking_GetAllTours();
        }
    }
}
