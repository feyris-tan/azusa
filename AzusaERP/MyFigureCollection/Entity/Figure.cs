using System;

namespace moe.yo3explorer.azusa.MyFigureCollection.Entity
{
    /**
     * SELECT figure.id, root.name, category.name, figure.barcode, figure.name, figure.release_date, figure.price, figurephoto.thumbnail

     */
    public class Figure
    {
        public int ID { get; set; }
        public string RootName { get; set; }
        public string CategoryName { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public double Price { get; set; }
        public byte[] Thumbnail { get; set; }

    }
}
