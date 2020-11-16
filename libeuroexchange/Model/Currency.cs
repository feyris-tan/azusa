using System;
using System.Collections.Generic;
using System.Text;

namespace libeuroexchange.Model
{
    public class Currency
    {
        public Currency(string name, double rate)
        {
            this.Name = name;
            this.ExchangeRate = rate;
        }

        public string Name { get; private set; }
        public double ExchangeRate { get; private set; }
    }
}
