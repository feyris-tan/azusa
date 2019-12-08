using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class MediaService
    {
        public static IEnumerable<MediaInProduct> GetMediaFromProduct(Product prod)
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetMediaByProduct(prod);
        }

        public static Media GetSpecificMedia(int id)
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetMediaById(id);
        }

        public static void UpdateMedia(Media media)
        {
            AzusaContext.GetInstance().DatabaseDriver.UpdateMedia(media);
        }

        public static int CreateMedia(Product p, string name)
        {
            return AzusaContext.GetInstance().DatabaseDriver.CreateMediaAndReturnId(p.Id, name);
        }
    }
}
