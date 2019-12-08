using osm2azusa.Model;

namespace osm2azusa
{
    class NullWriter : IAzusaWriter
    {
        public void Dispose()
        {
        }

        public void WriteNode(Node node)
        {
        }

        public void WriteWay(Way way)
        {
        }
        
        public void WriteRelation(Relation relation)
        {
        }
    }
}