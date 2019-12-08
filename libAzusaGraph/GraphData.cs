using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace dsMediaLibraryClient.GraphDataLib
{
    public class GraphData
    {
        private static bool wasLoaded;
        
        public GraphData(TextReader br)
        {
            if (!wasLoaded)
            {
                Console.WriteLine("libAzusaGraph was loaded.");
                wasLoaded = true;
            }
            read(br);
        }

        private GraphData()
        {
        }
        
        private int ibgdVersion;
        private DateTime date;
        private int sample_rate;
        private GraphDataDevice device;
        private GraphDataMedia media;
        private GraphDataData data;
        private GraphDataVerify verify;
        private bool hrpc;
        private List<GraphDataSample> samples;
        
        void read(TextReader br)
        {
            device = new GraphDataDevice();
            media = new GraphDataMedia();
            data = new GraphDataData();
            verify = new GraphDataVerify();
            
            string magic = br.ReadLine();
                if(!magic.Equals("IBGD"))
                {
                    throw new InvalidMagicException();
                }
                string temp;
                string[] args;
                while (true) {
                    temp = br.ReadLine();
                    if (temp.Equals("[START_CONFIGURATION]"))
                    {
                        break;
                    }
                }
                while(true)
                {
                    temp = br.ReadLine();
                    if (string.IsNullOrEmpty(temp))
                    {
                        continue;
                    }
                    if (temp.Equals("[END_CONFIGURATION]"))
                    {
                        break;
                    }
                    args = temp.Split('=');
                    if (args[0].Equals("IBGD_VERSION"))
                    {
                        ibgdVersion = Int32.Parse(args[1]);
                    }
                    else if (args[0].Equals("DATE"))
                    {
                        date = DateTime.ParseExact(args[1], "dd.MM.yyyy HH:m:s", CultureInfo.InvariantCulture);
                    }
    
                    else if (args[0].Equals("SAMPLE_RATE"))
                    {
                        sample_rate = int.Parse(args[1]);
                    }
                    else if (args[0].Equals("DEVICE"))
                    {
                    }
                    else if (args[0].Equals("DEVICE_ADDRESS"))
                    {
                        device.Address = args[1];
                    }
                    else if (args[0].Equals("DEVICE_MAKEMODEL"))
                    {
                        device.MakeModel = args[1];
                    }
                    else if (args[0].Equals("DEVICE_FIRMWAREVERSION"))
                    {
                        device.FirmwareVersion = args[1];
                    }
                    else if (args[0].Equals("DEVICE_DRIVELETTER"))
                    {
                        device.DriveLetter = args[1];
                    }
                    else if (args[0].Equals("DEVICE_BUSTYPE"))
                    {
                        device.BusType = args[1];
                    }
    
                    else if (args[0].Equals("MEDIA_TYPE"))
                    {
                        media.Type = args[1];
                    }
                    else if (args[0].Equals("MEDIA_BOOKTYPE"))
                    {
                        media.BookType = args[1];
                    }
                    else if (args[0].Equals("MEDIA_ID"))
                    {
                        media.Id = args[1];
                    }
                    else if (args[0].Equals("MEDIA_TRACKPATH"))
                    {
                        media.TrackPath = args[1];
                    }
                    else if (args[0].Equals("MEDIA_SPEEDS"))
                    {
                        if (!args[1].Equals("N/A"))
                        {
                            media.ParseSpeeds(args[1]);
                        }
                    }
                    else if (args[0].Equals("MEDIA_CAPACITY"))
                    {
                        media.Capacity = long.Parse(args[1]);
                    }
                    else if (args[0].Equals("MEDIA_LAYER_BREAK"))
                    {
                        media.LayerBreak = long.Parse(args[1]);
                    }
                    else if (args[0].Equals("DATA_IMAGEFILE"))
                    {
                        data.ImageFile1 = args[1];
                    }
                    else if (args[0].Equals("DATA_SECTORS"))
                    {
                        data.Sectors1 = long.Parse(args[1]);
                    }
                    else if (args[0].Equals("DATA_TYPE"))
                    {
                        data.Type1 = GraphDataDataType.parse(args[1]);
                    }
                    else if (args[0].Equals("DATA_VOLUMEIDENTIFIER"))
                    {
                        data.VolumeIdentifier1 = args[1];
                    }
    
                    else if (args[0].Equals("VERIFY_SPEED_START"))
                    {
                        verify.SpeedStart = double.Parse(args[1], CultureInfo.InvariantCulture);
                    }
                    else if (args[0].Equals("VERIFY_SPEED_END"))
                    {
                        verify.SpeedEnd = double.Parse(args[1], CultureInfo.InvariantCulture);
                    }
                    else if (args[0].Equals("VERIFY_SPEED_AVERAGE"))
                    {
                        verify.SpeedAverage = double.Parse(args[1], CultureInfo.InvariantCulture);
                    }
                    else if (args[0].Equals("VERIFY_SPEED_MAX"))
                    {
                        verify.SpeedMax = double.Parse(args[1], CultureInfo.InvariantCulture);
                    }
                    else if (args[0].Equals("VERIFY_TIME_TAKEN"))
                    {
                        verify.TimeTaken = long.Parse(args[1], CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        throw new Exception("don't know what to do with line: " + temp);
                    }
                }
    
                while(!temp.Equals("[START_VERIFY_GRAPH_VALUES]"))
                {
                    temp = br.ReadLine();
                    if (temp.StartsWith("HRPC"))
                    {
                        hrpc = bool.Parse(temp.Split('=')[1]);
                    }
                }
                while (true)
                {
                    temp = br.ReadLine();
                    if (string.IsNullOrEmpty(temp))
                        continue;
                    if (temp.Equals("[END_VERIFY_GRAPH_VALUES]"))
                    {
                        break;
                    }
                    args = temp.Split(',');
                    GraphDataSample newSample = new GraphDataSample();
                    newSample.ReadSpeed = float.Parse(args[0], CultureInfo.InvariantCulture);
                    newSample.SectorNo = long.Parse(args[1]);
                    newSample.SampleDistance = int.Parse(args[2]);
                    newSample.CpuLoad = float.Parse(args[3], CultureInfo.InvariantCulture);
                    if (samples == null)
                    {
                        samples = new List<GraphDataSample>();
                    }
                    samples.Add(newSample);
                }
    
                br.Close();
    
        }

        public GraphDataDevice Device { get { return device; } }

        public GraphDataMedia Media { get { return media; }}

        public GraphDataData Data { get { return data; }}

        public GraphDataVerify Verify { get { return verify; }}

        public int NumberOfSamples { get { return samples.Count; }}

        public DateTime Date { get { return date; }}

        public int SampleRate { get { return sample_rate; }}


        public GraphDataSample GetSample(int no)
        {
            return samples[no];
        }
    
        public int GetYSize()
        {
            int result = 0;
            foreach(GraphDataSample sample in samples)
            {
                if (sample.CpuLoad > result)
                {
                    result = (int) sample.CpuLoad;
                }
                if (sample.ReadSpeed > result)
                {
                    result = (int) sample.ReadSpeed;
                }
                if (sample.SampleDistance > result)
                {
                    result = sample.SampleDistance;
                }
            }
            return result;
    }

        public ReadOnlyCollection<GraphDataSample> ToList()
        {
            return new ReadOnlyCollection<GraphDataSample>(samples);
        }

        public GraphData Deinterpolate()
        {
            GraphData result = new GraphData();
            result.ibgdVersion = ibgdVersion;
            result.date = date;
            result.sample_rate = sample_rate;
            result.device = device;
            result.media = media;
            result.data = data;
            result.verify = verify;
            result.hrpc = hrpc;
            result.samples = new List<GraphDataSample>();
            
            var enumerator = samples.GetEnumerator();
            
            GraphDataSample a, b, c;
            do
            {
                if (!enumerator.MoveNext()) break;
                a = enumerator.Current;
                if (!enumerator.MoveNext()) break;
                b = enumerator.Current;

                c = new GraphDataSample();
                c.CpuLoad = (a.CpuLoad + b.CpuLoad) / 2;
                c.ReadSpeed = (a.ReadSpeed + b.ReadSpeed) / 2;
                c.SampleDistance = (a.SampleDistance + b.SampleDistance) / 2;
                c.SectorNo = (a.SectorNo + b.SectorNo) / 2;
                result.samples.Add(c);

            } while (wasLoaded);

            return result;
        }
        
        public void UpdateSyntheticLines()
        {
            double totalCpu = 0.0;
            double totalSpeed = 0.0;

            for (int i = 0; i < samples.Count; i++)
            {
                totalCpu += samples[i].CpuLoad;
                totalSpeed += samples[i].ReadSpeed;
                samples[i].AverageCpuLoad = totalCpu / (i + 1);
                samples[i].AverageReadSpeed = totalSpeed / (i + 1);
            }
        }
    }
}