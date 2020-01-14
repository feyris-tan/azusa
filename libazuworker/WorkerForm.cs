using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace libazuworker
{
    public partial class WorkerForm : Form
    {
        public WorkerForm(AzusaWorker worker)
        {
            InitializeComponent();
            this.worker = worker;
            this.worker.SetWorkerForm(this);
            this.label1.Text = "";
        }

        private AzusaWorker worker;
        private Thread thread;

        private void WorkerForm_Shown(object sender, EventArgs e)
        {
            Debug.WriteLine("shown");
            this.Text = worker.Title;
            this.totalProgressBar.Maximum = worker.InitialNumberOfSteps;
            this.NextStep(worker.InitalizingMessage);
            this.totalProgressBar.Value--;
            thread = new Thread(worker.DoWork);
            thread.Name = worker.Title;
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        private void NextStep(string next)
        {
            if (listView1.Items.Count > 0)
            {
                listView1.Items[listView1.Items.Count - 1].ImageIndex = 2;
            }

            ListViewItem newLvi = new ListViewItem(next, 1);
            listView1.Items.Add(newLvi);
            listView1.EnsureVisible(listView1.Items.Count - 1);
            currentStepProgressBar.Value = 0;
            if (totalProgressBar.Value == totalProgressBar.Maximum)
                totalProgressBar.Maximum++;
            totalProgressBar.Value++;
        }

        public void InvokeNextStep(string next)
        {
            Invoke((MethodInvoker) delegate { NextStep(next); });
        }

        private void RequireMoreSteps(int more)
        {
            if (totalProgressBar.Value == 0)
            {
                totalProgressBar.Maximum += more;
                return;
            }

            more++;
            int totalSteps = totalProgressBar.Maximum + more;
            double divisor = (double) (totalSteps) / (double) totalProgressBar.Maximum;
            double done = (double) totalProgressBar.Value * divisor;
            done = Math.Floor(done);
            totalProgressBar.Maximum = totalSteps;
            totalProgressBar.Value = Convert.ToInt32(done);
        }

        public void InvokeRequireMoreSteps(int more)
        {
            Invoke((MethodInvoker)delegate { RequireMoreSteps(more); });
        }

        private void SetCurrentStepProgress(int value, int maximum)
        {
            currentStepProgressBar.Maximum = maximum;
            if (value <= maximum)
            {
                currentStepProgressBar.Value = value;
            }
        }

        public void InvokeSetCurrentStepProgress(int value, int maximum = 100)
        {
            if (!Visible)
                return;

            Invoke((MethodInvoker) delegate { SetCurrentStepProgress(value, maximum); });
        }

        public void InvokeClose()
        {
            Invoke((MethodInvoker) delegate
            {
                while (currentStepProgressBar.Maximum > 9000)
                {
                    currentStepProgressBar.Value /= 10;
                    currentStepProgressBar.Maximum /= 10;
                }
                while (currentStepProgressBar.Maximum > currentStepProgressBar.Value)
                {
                    currentStepProgressBar.Maximum--;
                    currentStepProgressBar.PerformStep();
                    Thread.Sleep(1);
                }
                while (totalProgressBar.Maximum > totalProgressBar.Value)
                {
                    totalProgressBar.PerformStep();
                    Thread.Sleep(10);
                }
            });
            Invoke((MethodInvoker) delegate { Close(); });
        }

        public void InvokeNextStepWithoutText()
        {
            Invoke((MethodInvoker)delegate { totalProgressBar.Value++; });
        }

        public void InvokeSetText(string text)
        {
            if (Visible)
            {
                Invoke((MethodInvoker) delegate { label1.Text = text; });
            }
        }

        public void InvokeSetText(string text, params object[] args)
        {
            InvokeSetText(String.Format(text, args));
        }

        public bool LooksFinished
        {
            get { return totalProgressBar.Value >= totalProgressBar.Maximum; }
        }
    }
}
