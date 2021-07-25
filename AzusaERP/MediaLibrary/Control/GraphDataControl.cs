using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dsMediaLibraryClient.GraphDataLib;
using NPlot;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class GraphDataControl : UserControl, ISidecarDisplayControl
    {
        public GraphDataControl()
        {
            InitializeComponent();
            mediaGraphData.MaxLength = Int32.MaxValue;
        }

        public bool Enabled
        {
            get
            {
                return graphDataPlot.Enabled;
            }
            set
            {
                graphDataPlot.Enabled = value;
                mediaGraphData.Enabled = value;
                tabControl4.Enabled = value;
            }
        }

        public Guid DisplayControlUuid => Guid.Parse("{E2AD4DBB-2D9F-4F75-B259-95A29E9C452C}");

        private string _data;
        public byte[] Data
        {
            get
            {
                return Encoding.UTF8.GetBytes(_data);
            }
            set
            {
                if (value == null)
                    value = new byte[0];
                _data = Encoding.UTF8.GetString(value);
                mediaGraphData.Text = _data;

                if (!string.IsNullOrEmpty(_data))
                {
                    try
                    {
                        GraphData gd = new GraphData(new StringReader(_data));
                        UpdateGraphdataPlot(gd);
                    }
                    catch (InvalidMagicException ime)
                    {
                        graphDataPlot.Clear();
                    }
                }
                else
                {
                    graphDataPlot.Clear();
                }
                graphDataPlot.Refresh();
                if (OnDataChanged != null)
                    OnDataChanged(value, isComplete(), MediumId);
            }
        }


        private void UpdateGraphdataPlot(GraphData gd)
        {
            graphDataPlot.Clear();
            gd.UpdateSyntheticLines();

            double[][] plotData = new double[6][];
            for (int i = 0; i < plotData.Length; i++)
                plotData[i] = new double[gd.NumberOfSamples];

            for (int i = 0; i < gd.NumberOfSamples; i++)
            {
                GraphDataSample sample = gd.GetSample(i);
                plotData[0][i] = sample.AverageCpuLoad;
                plotData[1][i] = sample.AverageReadSpeed;
                plotData[2][i] = sample.CpuLoad;
                plotData[3][i] = sample.ReadSpeed;
                plotData[4][i] = sample.SampleDistance;
                plotData[5][i] = sample.SectorNo;
            }

            graphDataPlot.Legend = new Legend();
            graphDataPlot.XAxis1 = new LinearAxis(0, gd.NumberOfSamples);
            graphDataPlot.YAxis1 = new LinearAxis(0, gd.SampleRate);

            Color[] colors = { Color.Cyan, Color.Yellow, Color.Pink, Color.Red, Color.Blue, Color.CornflowerBlue };
            string[] plotNames = { "Ø CPU", "Ø Read speed", "CPU", "Read Speed", "Distance", "SectorNo" };
            for (int i = 0; i < plotData.Length - 1; i++)
            {
                LinePlot linePlot = new LinePlot(plotData[i]);
                linePlot.Color = colors[i];
                linePlot.Label = plotNames[i];
                linePlot.ShowInLegend = true;
                graphDataPlot.Add(linePlot);
            }
        }


        public bool isComplete()
        {
            return !string.IsNullOrEmpty(_data);
        }

        public int MediumId { get; set; }

        public System.Windows.Forms.Control ToControl()
        {
            return this;
        }

        private uint enableMe;
        public void ForceEnabled()
        {
            Enabled = true;
            this.AllowDrop = true;
        }

        public event SidecarChange OnDataChanged;

        private void GraphDataControl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                object j = e.Data.GetData(DataFormats.FileDrop);
                string[] jStrings = (string[])j;
                if (jStrings.Length == 1)
                {
                    FileInfo fi = new FileInfo(jStrings[0]);
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void GraphDataControl_DragDrop(object sender, DragEventArgs e)
        {
            object j = e.Data.GetData(DataFormats.FileDrop);
            string[] jStrings = (string[])j;
            Data = File.ReadAllBytes(jStrings[0]);
        }
    }
}
