using System;
using System.Collections.Generic;
using System.Text;
using libeuroexchange.Model;

namespace libeuroexchange
{
    public class Cube
    {
        public Cube(DateTime date)
        {
            this.Date = date;
            this.Currencies = new List<Currency>();
        }

        public DateTime Date { get; private set; }
        public List<Currency> Currencies { get; private set; }
        
    }
}
