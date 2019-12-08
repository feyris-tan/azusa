using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.dex
{
    internal class ConsoleLogCallback : ILogCallback
    {
        public void LogEvent(string s)
        {
            Console.WriteLine(s);
        }
    }
}
