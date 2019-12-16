using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace libazuworker
{
    public abstract class AzusaWorker
    {
        public void Run(Form owner = null)
        {
            libazuworker.WorkerForm form = new WorkerForm(this);
            form.ShowDialog(owner);
        }

        internal void SetWorkerForm(WorkerForm wf)
        {
            this.workerForm = wf;
        }
        private WorkerForm workerForm;

        protected WorkerForm WorkerForm
        {
            get
            {
                return workerForm;
            }
        }

        public abstract void DoWork();
        public abstract string Title { get; }
        public abstract int InitialNumberOfSteps { get; }
        public abstract string InitalizingMessage { get; }
    }
}
