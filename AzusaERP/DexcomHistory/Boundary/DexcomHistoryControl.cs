using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.DexcomHistory.Control;
using NPlot;

namespace moe.yo3explorer.azusa.DexcomHistory.Boundary
{
    public partial class DexcomHistory : UserControl, IAzusaModule
    {
        public DexcomHistory()
        {
            InitializeComponent();
        }

        public string IniKey => "dexcom";
        public string Title { get { return "Continuos Glucose Management"; } }

        public int Priority { get { return 2; } }

        System.Windows.Forms.Control IAzusaModule.GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            AzusaContext context = AzusaContext.GetInstance();
            context.Splash.SetLabel("Frage CGM Tage ab...");
            UpdateDates();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private AzusaDexTimeline LoadTimelineFromDevice(SerialPort sp)
        {
            DexcomDevice device = new DexcomDevice(sp.BaseStream);
            device.LogCallback = new AzusaErpLogCallback();
            AzusaDexTimeline result = AzusaDexTimeline.LoadFrom(device);
            return result;
        }

        private void gerätespeicherInAzusaDexXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm mf = AzusaContext.GetInstance().MainForm;
            SerialPort sp = SelectComPortForm.ShowSelectionFor(mf);
            if (sp == null)
                return;
            gerätespeicherInAzusaDexXMLToolStripMenuItem.Enabled = false;
            gerätespeicherAuslesenToolStripMenuItem.Enabled = false;
            Thread thread = new Thread(ReadToXmlThread);
            thread.Start(sp);
        }

        private void ReadToXmlThread(object rawSerialPort)
        {
            SerialPort serialPort = (SerialPort)rawSerialPort;
            if (serialPort == null)
                return;
            serialPort.Open();
            AzusaDexTimeline result = LoadTimelineFromDevice(serialPort);
            serialPort.Close();
            var fileInfo = new System.IO.FileInfo(DateTime.Now.Ticks.ToString() + ".xml");
            result.SaveTo(fileInfo);
            MainForm mainForm = AzusaContext.GetInstance().MainForm;
            mainForm.SetStatusBar("Fertig!");
            mainForm.Invoke((MethodInvoker)delegate
           {
               gerätespeicherAuslesenToolStripMenuItem.Enabled = true;
               gerätespeicherInAzusaDexXMLToolStripMenuItem.Enabled = true;
           });
        }

        private void azusaDexXMLImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(AzusaContext.GetInstance().MainForm) != DialogResult.OK)
                return;

            FileInfo fi = new FileInfo(openFileDialog1.FileName);
            AzusaDexTimeline timeline = AzusaDexTimeline.LoadFrom(fi);
            new Thread(DoImport).Start(timeline);
        }

        private void DoImport(object rawTimeline)
        {
            AzusaDexTimeline timeline = (AzusaDexTimeline)rawTimeline;
            MainForm mainForm = AzusaContext.GetInstance().MainForm;
            DexcomHistoryService.Import(timeline);
            Invoke((MethodInvoker)UpdateDates);
        }

        private void UpdateDates()
        {
            listBox1.Items.Clear();
            foreach(DateTime dt in DexcomHistoryService.GetDates())
            {
                listBox1.Items.Add(new ListBoxDateWrapper(dt));
            }
        }

        double GuessYValue(List<DexTimelineEntry> glucoseEvents, DateTime xValue)
        {
            DexTimelineEntry dte = glucoseEvents.FirstOrDefault(x => x.Timestamp > xValue);
            if (dte == null)
                return 200.0;
            else
                return (double)dte.Glucose.Value;
        }


        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ListBoxDateWrapper dateWrapper = (ListBoxDateWrapper)listBox1.SelectedItem;
            List<DexTimelineEntry> entries = DexcomHistoryService.GetDexTimelineEntries(dateWrapper.Value).ToList();
            entries.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));

            List<DexTimelineEntry> glucoseEvents = entries.FindAll(x => x.GlucoseSpecified);
            List<DexTimelineEntry> events = entries.FindAll(x => x.InsulinSpecified || x.CarbsSpecified ||x.EventTypeSpecified || x.MeterGlucoseSpecified || x.SessionStateSpecified);

            plotSurface2D1.Clear();

            if (glucoseEvents.Count == 0)
            {
                plotSurface2D1.Refresh();
                plotSurface2D1.Update();
                return;
            }

            plotSurface2D1.XAxis1 = new DateTimeAxis(dateWrapper.Value.Date, dateWrapper.Value.Date.AddHours(24));
            plotSurface2D1.YAxis1 = new LinearAxis(0, 400);

            DateTime[] xValues;
            double[] yValues;

            xValues = new DateTime[glucoseEvents.Count];
            yValues = new double[glucoseEvents.Count];
            for (int i = 0; i < glucoseEvents.Count; i++)
            {
                xValues[i] = glucoseEvents[i].Timestamp;
                yValues[i] = (double)glucoseEvents[i].Glucose.Value;
            }

            DateTime lastEvent = DateTime.MaxValue;
            double angle = 90.0;
            foreach(DexTimelineEntry theEvent in events)
            {
                string msg = "???";
                if (theEvent.CarbsSpecified)
                    msg = String.Format("{0} BE", theEvent.Carbs.Value / 12);
                else if (theEvent.InsulinSpecified)
                    msg = String.Format("{0} IE", theEvent.Insulin.Value);
                else if (theEvent.SessionStateSpecified)
                    msg = theEvent.SessionState.Value.ToString();
                else if (theEvent.ExerciseEventSpecified)
                    msg = String.Format("{0} Exercise", theEvent.ExerciseEvent.Value);
                else if (theEvent.HealthEventSpecified)
                    msg = theEvent.HealthEvent.ToString();
                else if (theEvent.MeterGlucoseSpecified)
                    msg = String.Format("Calibration: {0}", theEvent.MeterGlucose.Value);

                if ((theEvent.Timestamp - lastEvent).TotalMinutes < 10)
                    angle += 10;

                PointD pt = new PointD(theEvent.Timestamp.Ticks, GuessYValue(glucoseEvents, theEvent.Timestamp));
                ArrowItem annotation = new ArrowItem(pt, angle, msg);
                angle += 5;
                plotSurface2D1.Add(annotation);
            }

            LinePlot glucosePlot = new LinePlot(yValues, xValues);
            plotSurface2D1.Add(glucosePlot);

            plotSurface2D1.Refresh();
            plotSurface2D1.Update();
            
            /*chart1.ChartAreas[0].AxisX.Minimum = dateWrapper.Value.Date.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = (dateWrapper.Value.Date + new TimeSpan(24, 0, 0)).ToOADate();
            chart1.Series.Clear();
            chart1.Annotations.Clear();

            Series glucoseSeries = new Series();
            glucoseSeries.ChartType = SeriesChartType.Line;
            glucoseSeries.XValueType = ChartValueType.Time;
            chart1.Series.Add(glucoseSeries);
            Series eventSeries = new Series();
            eventSeries.ChartType = SeriesChartType.Point;
            eventSeries.XValueType = ChartValueType.Time;
            chart1.Series.Add(eventSeries);
            DataPoint lastDataPoint = new DataPoint(0, 200);
            foreach (DexTimelineEntry dte in entries)
            {
                if (dte.Glucose != null)
                {
                    lastDataPoint = new DataPoint();
                    lastDataPoint.XValue = dte.Timestamp.ToOADate();
                    lastDataPoint.YValues = new double[] { dte.Glucose.Value };
                    glucoseSeries.Points.Add(lastDataPoint);
                }
                if (dte.Insulin != null)
                {
                    DataPoint eventPoint = new DataPoint();
                    eventPoint.XValue = dte.Timestamp.ToOADate();
                    eventPoint.YValues = lastDataPoint.YValues;
                    eventSeries.Points.Add(eventPoint);
                    CalloutAnnotation insulinAnnotation = new CalloutAnnotation();
                    insulinAnnotation.Text = String.Format("{0} IE", dte.Insulin);
                    insulinAnnotation.AnchorDataPoint = eventPoint;
                    chart1.Annotations.Add(insulinAnnotation);
                }
                if (dte.Carbs != null)
                {
                    DataPoint eventPoint = new DataPoint();
                    eventPoint.XValue = dte.Timestamp.ToOADate();
                    eventPoint.YValues = lastDataPoint.YValues;
                    eventSeries.Points.Add(eventPoint);
                    CalloutAnnotation insulinAnnotation = new CalloutAnnotation();
                    insulinAnnotation.Text = String.Format("{0} BE", dte.Carbs / 12);
                    insulinAnnotation.AnchorDataPoint = eventPoint;
                    chart1.Annotations.Add(insulinAnnotation);
                }
                if (dte.EventType != null)
                {
                    DataPoint eventPoint = new DataPoint();
                    eventPoint.XValue = dte.Timestamp.ToOADate();
                    eventPoint.YValues = lastDataPoint.YValues;
                    eventSeries.Points.Add(eventPoint);
                    switch (dte.EventType)
                    {
                        case moe.yo3explorer.azusa.dex.Schema.Enums.EventType.Exercise:
                            CalloutAnnotation exerciseAnnotation = new CalloutAnnotation();
                            exerciseAnnotation.Text = String.Format(dte.ExerciseEvent.Value.ToString());
                            exerciseAnnotation.AnchorDataPoint = eventPoint;
                            chart1.Annotations.Add(exerciseAnnotation);
                            break;
                        case moe.yo3explorer.azusa.dex.Schema.Enums.EventType.Health:
                            CalloutAnnotation healthAnnotation = new CalloutAnnotation();
                            healthAnnotation.Text = String.Format(dte.HealthEvent.Value.ToString());
                            healthAnnotation.AnchorDataPoint = eventPoint;
                            chart1.Annotations.Add(healthAnnotation);
                            break;
                    }
                }
                if (dte.MeterGlucose != null)
                {
                    DataPoint eventPoint = new DataPoint();
                    eventPoint.XValue = dte.Timestamp.ToOADate();
                    eventPoint.YValues = new double[] { dte.MeterGlucose.Value };
                    eventSeries.Points.Add(eventPoint);
                    CalloutAnnotation calibrationAnnotation = new CalloutAnnotation();
                    calibrationAnnotation.Text = String.Format("Kalibration: {0}", dte.MeterGlucose);
                    calibrationAnnotation.AnchorDataPoint = eventPoint;
                    chart1.Annotations.Add(calibrationAnnotation);
                }
                if (dte.SessionState != null && lastDataPoint != null)
                {
                    DataPoint eventPoint = new DataPoint();
                    eventPoint.XValue = dte.Timestamp.ToOADate();
                    eventPoint.YValues = lastDataPoint.YValues;
                    eventSeries.Points.Add(eventPoint);
                    CalloutAnnotation sessionStateAnnotation = new CalloutAnnotation();
                    sessionStateAnnotation.Text = String.Format(dte.SessionState.Value.ToString());
                    sessionStateAnnotation.AnchorDataPoint = lastDataPoint;
                    chart1.Annotations.Add(sessionStateAnnotation);
                }
            }*/

            toolStripButton1.Enabled = listBox1.SelectedIndex > 0;
            toolStripButton2.Enabled = listBox1.SelectedIndex < listBox1.Items.Count - 1;
            toolStripTextBox1.Text = ((ListBoxDateWrapper)listBox1.SelectedItem).Value.ToShortDateString();
        }

        private void gerätespeicherAuslesenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm mf = AzusaContext.GetInstance().MainForm;
            SerialPort sp = SelectComPortForm.ShowSelectionFor(mf);
            if (sp == null)
                return;
            gerätespeicherInAzusaDexXMLToolStripMenuItem.Enabled = false;
            gerätespeicherAuslesenToolStripMenuItem.Enabled = false;
            Thread thread = new Thread(DeviceToDatabaseThread);
            thread.Start(sp);
        }

        private void DeviceToDatabaseThread(object rawSerialPort)
        {
            MainForm mainForm = AzusaContext.GetInstance().MainForm;
            SerialPort serialPort = (SerialPort)rawSerialPort;
            if (serialPort == null)
                return;
            serialPort.Open();
            AzusaDexTimeline result;
            try
            {
                result = LoadTimelineFromDevice(serialPort);
            }
            catch (Exception e)
            {
                mainForm.SetStatusBar("Auslesen fehlgeschlagen: " + e);
                serialPort.Close();
                return;
            }
            serialPort.Close();
            DoImport(result);

            mainForm.Invoke((MethodInvoker)delegate
            {
                gerätespeicherInAzusaDexXMLToolStripMenuItem.Enabled = true;
                gerätespeicherAuslesenToolStripMenuItem.Enabled = false;
                UpdateDates();
            });
        }

        private void cSVExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            int tableSize = listBox1.Items.Count - 1;
            int startItem = Math.Max(0, tableSize - 100);

            AzusaDexTimeline timeline = new AzusaDexTimeline();
            for (int i = startItem; i < tableSize; i++)
            {
                ListBoxDateWrapper dateWrapper = (ListBoxDateWrapper)listBox1.Items[i];
                IEnumerable<DexTimelineEntry> entries = DexcomHistoryService.GetDexTimelineEntries(dateWrapper.Value);
                foreach(DexTimelineEntry entry in entries)
                {
                    timeline.Data.Add(entry);
                }
            }
            timeline.Order();
            FileInfo fi = new FileInfo(saveFileDialog1.FileName);
            if (fi.Exists)
                fi.Delete();
            timeline.SaveCsv(fi, true, true, false);
            AzusaContext.GetInstance().MainForm.SetStatusBar("CSV-Export fertig!");
        }

        private void UserControl1_Resize(object sender, EventArgs e)
        {
            toolStripLabel1.Text = "";
            toolStripLabel1.AutoSize = false;
            toolStripLabel1.Width = toolStrip1.Width / 2 - ((toolStripButton1.Width + toolStripButton2.Width + toolStripTextBox1.Width) / 2);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex--;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            listBox1.SelectedIndex++;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        ManualDataEntires mde;
        private void manuelleDatenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mde == null)
            {
                mde = new ManualDataEntires();
            }
            mde.ShowDialog(this);
        }

        private void hBA1CSchätzenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IDatabaseDriver database = AzusaContext.GetInstance().DatabaseDriver;
            DexTimelineEntry latestGlucose = database.Dexcom_GetLatestGlucoseEntry();
            DateTime scope = latestGlucose.Timestamp.AddMonths(-3);
            IEnumerable<DexTimelineEntry> entries = database.Dexcom_GetGlucoseEntriesAfter(scope);

            uint totalGlucose = 0;
            uint numGlucoseValues = 0;

            foreach (DexTimelineEntry entry in entries)
            {
                if (entry.Glucose.HasValue)
                {
                    totalGlucose += entry.Glucose.Value;
                    numGlucoseValues++;
                }
            }

            double avgGlucose = (double)totalGlucose / (double)numGlucoseValues;
            double hba1c = 0.031;
            hba1c *= avgGlucose;
            hba1c += 2.393;

            string text = String.Format("Geschätzer HBA1c: {0}%", hba1c);
            Debug.WriteLine(text);
            MessageBox.Show(text);
        }
    }
}
