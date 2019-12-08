using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using osm2azusa.Model;
using System.Threading;
using System.IO;

namespace osm2azusa
{
    class ProxyWriter : IAzusaWriter
    {
        IAzusaWriter child;
        Stream sourceOsmStream;

        public ProxyWriter(IAzusaWriter child,Stream str)
        {
            this.sourceOsmStream = str;
            this.child = child;
            Thread thread = new Thread(StatThread);
            thread.Start();
        }

        public void WriteNode(Node node)
        {
            child.WriteNode(node);
            writtenNodes++;
        }
        
        public void Dispose()
        {
            child.Dispose();
            disposed = true;
        }

        private ulong writtenNodes, writtenWays, writtenRelations;
        private TimeSpan elapsed;
        private Thread thread;
        private bool disposed;

        private void StatThread()
        {
            while (!disposed)
            {
                Thread.Sleep(1000);
                if (disposed)
                    return;
                double percentage = (double)sourceOsmStream.Position * 100.0 / (double)sourceOsmStream.Length;
                percentage = Math.Round(percentage, 2);
                elapsed = elapsed.Add(new TimeSpan(0, 0, 1));

                Console.WriteLine("{0}, {4}% completed, {1} nodes, {2} ways, {3} relations", elapsed, writtenNodes, writtenWays, writtenRelations, percentage);
            }
        }

        public void WriteWay(Way way)
        {
            child.WriteWay(way);
            writtenWays++;
        }
        
        public void WriteRelation(Relation relation)
        {
            child.WriteRelation(relation);
            writtenRelations++;
        }
    }
}
