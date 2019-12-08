using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psxdatacenterDumper
{
    interface IPdcSink : IDisposable
    {
        void HandleGame(Game child);
        bool GameKnown(string sku);
    }
}
