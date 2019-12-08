using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class PlatformService
    {
        public static IEnumerable<Platform> GetAllPlatforms()
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetAllPlatforms();
        }
    }
}
