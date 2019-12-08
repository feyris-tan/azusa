using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa
{
    public abstract class AzusaPlugin : IDisposable
    {
        private AzusaContext context;
        protected AzusaContext GetContext()
        {
            if (context == null)
                context = AzusaContext.GetInstance();
            return context;
        }

        public abstract bool IsExecutable { get; }
        public abstract string DisplayName { get; }

        public virtual void OnLoad()
        {

        }

        public virtual void Execute()
        {

        }

        public virtual void Dispose()
        {

        }
    }
}
