using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psxdatacenterDumper
{
    class NullPdcSink : IPdcSink
    {
        public NullPdcSink()
        {
            shots = new HashSet<string>();
        }
        private HashSet<string> shots;

        public void HandleGame(Game child)
        {
            Console.WriteLine(child.Title);
            foreach (Screenshot screenshot in child.Screenshots)
            {
                if (shots.Contains(screenshot.Name))
                    Console.WriteLine("-> used screenshot: " + screenshot.Name);
                else
                    shots.Add(screenshot.Name);
            }
        }
        
        public bool GameKnown(string sku)
        {
            return false;
        }

        public void Dispose()
        {
        }
    }
}
