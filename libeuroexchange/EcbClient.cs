using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using libeuroexchange.Model;

namespace libeuroexchange
{
    public class EcbClient
    {
        private EcbClient() { }
        private static EcbClient client;
        private Cube cube;

        public static EcbClient GetInstance()
        {
            if (client == null)
            {
                client = new EcbClient();
            }
            return client;
        }


        public Cube GetCube()
        {
            if (cube == null)
            {
                cube = DownloadCube();
            }
            return cube;
        }

        public Cube DownloadCube()
        {
            WebClient wc = new WebClient();
            byte[] buffer = wc.DownloadData("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml");

            XmlReader xmlReader = XmlReader.Create(new MemoryStream(buffer));
            Cube result = null;
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    if (xmlReader.Name.Equals("Cube"))
                    {
                        string time = xmlReader.GetAttribute("time");
                        if (!string.IsNullOrEmpty(time))
                            result = new Cube(DateTime.Parse(time));

                        string currency = xmlReader.GetAttribute("currency");
                        string rate = xmlReader.GetAttribute("rate");
                        if (!string.IsNullOrEmpty(currency) && !string.IsNullOrEmpty("rate"))
                        {
                            Currency child = new Currency(currency, Double.Parse(rate,CultureInfo.InvariantCulture));
                            result.Currencies.Add(child);
                        }
                    }
                }
            }
            xmlReader.Close();
            return result;
        }

        public AzusifiedCube AzusifyCube(Cube cube)
        {
            AzusifiedCube result = new AzusifiedCube();
            result.CubeDate = cube.Date;
            result.DateAdded = DateTime.Now;
            result.GBP = cube.Currencies.Find(x => x.Name.Equals("GBP")).ExchangeRate;
            result.JPY = cube.Currencies.Find(x => x.Name.Equals("JPY")).ExchangeRate;
            result.USD = cube.Currencies.Find(x => x.Name.Equals("USD")).ExchangeRate;
            return result;
        }
    }
}
