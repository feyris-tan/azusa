using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa
{
    public interface IImageAcquisitionPlugin
    {
        bool CanStart();
        Image Acquire();

        string Name { get; }
    }
}
