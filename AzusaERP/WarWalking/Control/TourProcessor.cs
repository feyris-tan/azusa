using System;
using System.IO;
using System.Text;
using moe.yo3explorer.azusa.WarWalking.Boundary;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace moe.yo3explorer.azusa.WarWalking.Control
{
    class TourProcessor : IPostConnectionTask
    {
        public void ExecutePostConnectionTask()
        {
            context = AzusaContext.GetInstance();

            int forbidUpload = context.ReadIniKey("gps", "forbidUpload", 1);
            if (forbidUpload == 1)
                return;

            DirectoryInfo tourDir = new DirectoryInfo("tours");
            if (!tourDir.Exists)
            {
                tourDir.Create();
                tourDir.Refresh();
            }
            foreach (FileInfo fi in tourDir.GetFiles("*.csv"))
            {
                if (IsStringNumeric(Path.GetFileNameWithoutExtension(fi.Name)))
                    continue;

                FileStream fs = fi.OpenRead();
                ByteBuffer checkBuffer = new ByteBuffer(512);
                int readResult = fs.Read(checkBuffer.Data, 0, 512);
                fs.Close();
                if (readResult != 512)
                    continue;

                if (TourService.IsTourKnown(checkBuffer.Hash))
                {
                    fi.Delete();
                    ExecutePostConnectionTask();
                    return;
                }

                if (context.DatabaseDriver.TransactionSupported)
                {
                    context.DatabaseDriver.BeginTransaction();
                    StreamReader sr = fi.OpenText();
                    string magic = sr.ReadLine();
                    if (magic != "Azusa Warwalking Tour")
                    {
                        sr.Dispose();
                        continue;
                    }

                    DateTime recordingStarting = new DateTime(Convert.ToInt64(sr.ReadLine()));
                    int utimeRecordingStarted = recordingStarting.ToUnixTime();
                    int tourId = TourService.CreateTour(checkBuffer.Hash, utimeRecordingStarted,
                        Path.GetFileNameWithoutExtension(fi.Name));
                    context.Splash.SetLabel(String.Format("Lade WarWalking Tour hoch: {0}", fi.Name));
                    ProcessTour(sr, tourId);
                    context.DatabaseDriver.EndTransaction(true);
                }
            }
        }

        public void ProcessTour(StreamReader sr, int tourId)
        {
            
            string line;
            string[] args;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                if (!line.StartsWith("NETWORK;"))
                    continue;

                args = line.Split(';');
                Discovery parsed = ParseDiscovery(args);
                parsed.RelatedTour = tourId;
                parsed.StrongestTour = tourId;
                if (DiscoveryService.IsAccessPointKnown(parsed.Bssid))
                {
                    Discovery inDb = DiscoveryService.GetByBssid(parsed.Bssid);
                    if (parsed.Rssi > inDb.Rssi)
                    {
                        DiscoveryService.UpdateDiscovery(parsed);
                    }
                }
                else
                {
                    context.Splash.SetLabel(String.Format("Neuer WarWalking Access-Point: {0}", parsed.Ssid));
                    DiscoveryService.AddAccessPoint(parsed);
                }
            }
        }

        public Discovery ParseDiscovery(string[] args)
        {
            Discovery result = new Discovery();
            result.DiscoveryUtime = new DateTime(Convert.ToInt64(args[1]));
            result.StrogestSignalUtime = new DateTime(result.DiscoveryUtime.Ticks);
            result.Lon = Convert.ToSingle(args[3]);
            result.Lat = Convert.ToSingle(args[4]);
            result.Bssid = args[5];
            result.Dot11bssType = (Wlan.Dot11BssType)Convert.ToInt32(args[6]);
            result.DefaultAuthAlgorithm = (Wlan.Dot11AuthAlgorithm)Convert.ToInt32(args[7]);
            result.DefaultCipherAlgorithm = (Wlan.Dot11CipherAlgorithm)Convert.ToInt32(args[8]);
            result.Ssid = Encoding.UTF8.GetString(args[10].HexToBytes(), 0, Convert.ToInt32(args[9]));
            result.Flags = Convert.ToInt32(args[11]);
            result.MorePhyTypes = Convert.ToBoolean(args[12]);
            result.NetworkConnectable = Convert.ToBoolean(args[13]);
            result.SecurityEnable = Convert.ToBoolean(args[14]);
            result.WlanSignalQuality = Convert.ToInt32(args[15]);
            result.BeaconPeriod = Convert.ToInt32(args[16]);
            result.CapabilityInformation = Convert.ToInt32(args[17]);
            result.ChCenterFrequency = Convert.ToInt32(args[18]);
            result.Dot11bssPhyType = (Wlan.Dot11PhyType)Convert.ToInt32(args[20]);
            result.HostTimestamp = Convert.ToInt64(args[21]);
            result.IeOffset = Convert.ToInt32(args[22]);
            result.IeSize = Convert.ToInt32(args[23]);
            result.InRegDomain = Convert.ToBoolean(args[24]);
            result.PhyId = Convert.ToInt32(args[25]);
            result.Rssi = Convert.ToInt32(args[26]);
            result.Timestamp = Convert.ToInt64(args[27]);
            result.Mbps = Convert.ToInt32(args[28]);
            result.StrongestTour = result.RelatedTour;
            return result;
        }

        AzusaContext context;

        bool IsStringNumeric(string fname)
        {
            foreach(char c in fname)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }
    }
}
