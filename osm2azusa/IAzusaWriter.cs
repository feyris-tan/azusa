using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osm2azusa.Model;

namespace osm2azusa
{
    interface IAzusaWriter : IDisposable
    {
        void WriteNode(Node node);
        void WriteWay(Way way);
        void WriteRelation(Relation relation);
    }
}
