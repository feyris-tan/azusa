using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psxdatacenterDumper
{
    class Game
    {
        public Game()
        {
            Language = "";
            Screenshots = new List<Screenshot>();
        }

        public string SKU { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public string CommonTitle { get; set; }
        public string Region { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Publisher { get; set; }
        public DateTime DateReleased { get; set; }
        public byte[] Cover { get; set; }
        public string Description { get; set; }
        public string Barcode { get; set; }
        public List<Screenshot> Screenshots { get; private set; }
        public bool AdditionalData { get; set; }
        public string Platform { get; set; }
    }

    internal class Screenshot
    {
        public Screenshot(byte[] data, string name)
        {
            this.Data = data;
            this.Name = name;
        }

        public byte[] Data { get; private set; }
        public string Name { get; private set; }
    }
}
