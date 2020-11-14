using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    interface ISidecarDisplayControl
    {
        Guid DisplayControlUuid { get; }
        byte[] Data { get; set; }
        bool isComplete();
        int MediumId { get; set; }
    }
}
