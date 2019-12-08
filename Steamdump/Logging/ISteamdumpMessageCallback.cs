using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Steamdump.Logging
{
    public interface ISteamdumpMessageCallback
    {
        void SendMessage(string message, params object[] args);
        void SetProgress(ZipProgressEventArgs e);
    }
}
