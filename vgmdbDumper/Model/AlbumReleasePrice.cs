using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vgmdbDumper.Model
{
    class AlbumReleasePrice
    {
        public string currency;
        public string price;

        public double? ParsePrice()
        {
            if (string.IsNullOrEmpty(price))
                return null;

            if (price.Equals("Unknown"))
                return null;

            if (price.Equals("Name Your Price"))
                return null;

            if (price.Equals("Not for Sale"))
                return null;

            if (price.Equals("Free"))
            {
                currency = "";
                return 0;
            }

            if (string.IsNullOrEmpty(currency))
                currency = "";

            if (price.Contains(",") && price.Contains("."))
                price = price.Replace(",", "");

            return Convert.ToDouble(price);
        }
    }
}
