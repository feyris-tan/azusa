using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class ProductService
    {
        public static int CreateProduct(string name, Shelf shelf)
        {
            AzusaContext context = AzusaContext.GetInstance();
            return context.DatabaseDriver.CreateProductAndReturnId(shelf, name);
        }

        public static Product GetProduct(int id)
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetProductById(id);
        }

        public static void UpdateProduct(Product product)
        {
            AzusaContext.GetInstance().DatabaseDriver.UpdateProduct(product);
        }

        public static void SetCover(Product product)
        {
            AzusaContext.GetInstance().DatabaseDriver.SetCover(product);
        }

        public static void SetScreenshot(Product product)
        {
            AzusaContext.GetInstance().DatabaseDriver.SetScreenshot(product);
        }
    }
}
