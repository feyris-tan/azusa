using System;
using System.Collections.Generic;
using System.Text;

namespace libeuroexchange.Model
{
    public class AzusifiedCube
    {
        public DateTime DateAdded { get; set; }
        public DateTime CubeDate { get; set; }
        public double USD { get; set; }
        public double JPY { get; set; }
        public double GBP { get; set; }
    }
}
