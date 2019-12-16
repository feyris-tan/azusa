using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libazustreamblob;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    interface IStreamBlobOwner
    {
        AzusaStreamBlob GetStreamBlob();
    }
}
