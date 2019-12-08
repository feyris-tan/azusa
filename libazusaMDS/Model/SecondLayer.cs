using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class SecondLayer
    {
        public CopyrightInfo Copyright { get; internal set; }
        public ManufacturingData Manufacturing { get; internal set; }
        public PhysicalInfo Physical { get; internal set; }
    }
}
