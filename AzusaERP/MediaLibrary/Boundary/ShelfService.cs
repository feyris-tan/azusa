using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class ShelfService
    {
        public static IEnumerable<Shelf> GetShelves()
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetAllShelves();
        }

        public static IEnumerable<ProductInShelf> GetProducts(Shelf shelf)
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetProductsInShelf(shelf);
        }
    }
}
