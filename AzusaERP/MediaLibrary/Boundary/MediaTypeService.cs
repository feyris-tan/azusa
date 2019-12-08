using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class MediaTypeService
    {
        public static IEnumerable<MediaType> GetMediaTypes()
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetMediaTypes();
        }
    }
}
