using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.WarWalking.Control;

namespace moe.yo3explorer.azusa.WarWalking.Boundary
{
    public partial class WarWalkingControl : UserControl, IAzusaModule
    {
        public WarWalkingControl()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
            ClearLabels();
        }

        private void ClearLabels()
        {
            latLabel.Text = "";
            lonLabel.Text = "";
            numSatellitesLabel.Text = "";
            fixLabel.Text = "";
            altitudeLabel.Text = "";
        }

        public string IniKey => "warwalking";

        public string Title {
            get { return "WarWalking"; }}

        public int Priority
        {
            get { return 3; }
        }

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        

        private AzusaContext context;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (context.CurrentOnScreenModule != this)
                return;

            if (!tourRecording && scanThreadEnded)
            {
                Task task = nmeaStreamDevice.CloseAsync();
                nmeaStreamDevice.Dispose();

                lock (lockVictim)
                {
                    tourStreamWriter.Flush();
                    tourStreamWriter.Close();
                    tourStreamWriter.Dispose();
                }

                nmeaStream.Close();
                nmeaStream.Dispose();

                aufzeichnungBeendenToolStripMenuItem.Enabled = false;
                touraufzeichnungStartenToolStripMenuItem.Enabled = true;
                scanThreadEnded = false;
            }

            if (!tourRecording)
            {
                ClearLabels();
                return;
            }

            numSatellitesLabel.Text = numSattelites.ToString();
            lonLabel.Text = lon.ToString();
            latLabel.Text = lat.ToString();
            fixLabel.Text = fixMode.ToString();
            altitudeLabel.Text = altitude.ToString();

        }

        private void touraufzeichnungStartenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wifiFoundDelegate = WiFiFound;
            scanThreadEnded = false;
            lockVictim = new object();
            messageStats = new Dictionary<string, int>();
            validGpsSignal = false;
            DirectoryInfo tourStorageDir = new DirectoryInfo("tours");
            if (!tourStorageDir.Exists)
            {
                tourStorageDir.Create();
                tourStorageDir.Refresh();
            }

            wlanClient = new WlanClient();
            usableInterface = null;
            foreach (WlanClient.WlanInterface device in wlanClient.Interfaces)
            {
                if (device.InterfaceState == Wlan.WlanInterfaceState.Disconnected)
                {
                    usableInterface = device;
                    break;
                }
                Console.WriteLine("Can't use {0}, it is in state {1}.", device.InterfaceDescription, device.InterfaceState);
            }
            if (usableInterface == null)
            {
                MessageBox.Show("Es ist kein WLAN-Gerät verfügbar!");
                return;
            }

            switch (context.Ini["gps"]["connType"])
            {
                case "tcp":
                    string ip = context.Ini["gps"]["tcp_ip"];
                    ushort port = Convert.ToUInt16(context.Ini["gps"]["tcp_port"]);
                    TcpClient tcpClient = new TcpClient();
                    tcpClient.Connect(ip, port);
                    nmeaStream = tcpClient.GetStream();
                    break;
                case "serial":
                    string comPort = context.Ini["gps"]["serial_port"];
                    int baud = Convert.ToInt32(context.Ini["gps"]["serial_baud"]);
                    SerialPort sp = new SerialPort(comPort, baud);
                    sp.Open();
                    nmeaStream = sp.BaseStream;
                    break;
                default:
                    MessageBox.Show(String.Format("\"{0}\" ist kein gültiger GPS-Transport.", context.Ini["gps"]["connType"]));
                    break;
            }

            nmeaStreamDevice = new StreamDevice(nmeaStream);
            nmeaStreamDevice.MessageReceived += NmeaStreamDevice_MessageReceived;
            if (!nmeaStreamDevice.IsOpen)
            {
                Task task = nmeaStreamDevice.OpenAsync();
                task.Wait();
            }
            tourRecording = true;
            context.MainForm.SetStatusBar("Nmea gestartet!");

            FileInfo tourFileName = new FileInfo(Path.Combine(tourStorageDir.FullName, DateTime.Now.Ticks.ToString() + ".csv"));
            tourStreamWriter = new StreamWriter(tourFileName.OpenWrite(), Encoding.UTF8);
            tourStreamWriter.WriteLine("Azusa Warwalking Tour");
            tourStreamWriter.WriteLine(Path.GetFileNameWithoutExtension(tourFileName.Name));

            usableInterface.WlanConnectionNotification += UsableInterface_WlanConnectionNotification;
            usableInterface.WlanNotification += UsableInterface_WlanNotification;
            usableInterface.WlanReasonNotification += UsableInterface_WlanReasonNotification;

            wlanScanThread = new Thread(WlanScanThread);
            wlanScanThread.Priority = ThreadPriority.Lowest;
            wlanScanThread.Name = "WiFi Sniffer";
            wlanScanThread.Start();

            touraufzeichnungStartenToolStripMenuItem.Enabled = false;
            aufzeichnungBeendenToolStripMenuItem.Enabled = true;
        }

        private void UsableInterface_WlanReasonNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanReasonCode reasonCode)
        {
        }

        private void UsableInterface_WlanNotification(Wlan.WlanNotificationData notifyData)
        {
            if (notifyData.notificationSource == Wlan.WlanNotificationSource.ACM)
            {
                if (notifyData.notificationCode == 7)
                {
                    scanComplete = true;
                    return;
                }
            }

            lock (lockVictim)
            {
                tourStreamWriter.WriteLine("NOTIFY;{0};{1};{2};{3}",
                    notifyData.notificationSource, notifyData.notificationCode, notifyData.interfaceGuid, notifyData.dataSize);
            }
        }

        private void UsableInterface_WlanConnectionNotification(Wlan.WlanNotificationData notifyData, Wlan.WlanConnectionNotificationData connNotifyData)
        {
        }

        private void NmeaStreamDevice_MessageReceived(object sender, NmeaMessageReceivedEventArgs e)
        {
            if (!messageStats.ContainsKey(e.Message.MessageType))
            {
                messageStats.Add(e.Message.MessageType, 0);
            }
            messageStats[e.Message.MessageType]++;

            switch (e.Message.MessageType)
            {
                case "GPGSV":
                    Gpgsv gpgsv = (Gpgsv)e.Message;
                    numSattelites = gpgsv.SVsInView;
                    break;
                case "GPGLL":   //enthält GPS Daten
                    Gpgll gpgll = (Gpgll)e.Message;
                    if (gpgll.DataActive)
                    {
                        lon = gpgll.Longitude;
                        lat = gpgll.Latitude;
                        validGpsSignal = true;
                        gpsLlTimestamp = DateTime.Now;
                    }
                    break;
                case "GPGSA":
                    Gpgsa gpgsa = (Gpgsa)e.Message;
                    fixMode = gpgsa.FixMode;
                    break;
                case "GPRMC":   //enthält GPS Daten
                    Gprmc gprmc = (Gprmc)e.Message;                    
                    break;
                case "GPVTG":
                    break;
                case "GPZDA":
                    break;
                case "GPGGA":   //enthält GPS Daten
                    Gpgga gpgga = (Gpgga)e.Message;
                    altitude = gpgga.Altitude;
                    break;
                default:
                    lock (lockVictim)
                    {
                        tourStreamWriter.WriteLine(String.Format("UNIMP_GPS;", e.Message.ToString()));
                    }
                    break;
            }
        }

        bool tourRecording;
        bool validGpsSignal;
        DateTime gpsLlTimestamp;
        double lon, lat;
        Thread wlanScanThread;
        StreamWriter tourStreamWriter;
        WlanClient.WlanInterface usableInterface;
        WlanClient wlanClient;
        Stream nmeaStream;
        StreamDevice nmeaStreamDevice;
        int numSattelites;
        Gpgsa.Mode fixMode;
        double altitude;
        Dictionary<string, int> messageStats;
        object lockVictim;
        bool scanComplete;
        bool scanThreadEnded;
        WifiFoundDelegate wifiFoundDelegate;

        private void aufzeichnungBeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tourRecording = false;
            aufzeichnungBeendenToolStripMenuItem.Enabled = false;
        }

        void WlanScanThread()
        {
            while (tourRecording)
            {
                if (!validGpsSignal)
                {
                    context.MainForm.SetStatusBar("Warte auf gültiges GPS Signal...");
                    while (!validGpsSignal)
                        Thread.Sleep(100);
                }
                double signalAge = (DateTime.Now - gpsLlTimestamp).TotalSeconds;
                while (signalAge > 10)
                {
                    context.MainForm.SetStatusBar(String.Format("Letztes GPS Signal ist {0} Sekunden her. Warte auf Erneuerung...",(int)signalAge));
                    Thread.Sleep(1000);
                    signalAge = (DateTime.Now - gpsLlTimestamp).TotalSeconds;
                }

                scanComplete = false;
                usableInterface.Scan();
                while (!scanComplete)
                    Thread.Sleep(100);

                Invoke((MethodInvoker)delegate { listView1.Items.Clear(); });
                int numAps = 0;
                Wlan.WlanAvailableNetwork[] networks = usableInterface.GetAvailableNetworkList(Wlan.WlanGetAvailableNetworkFlags.IncludeAllAdhocProfiles | Wlan.WlanGetAvailableNetworkFlags.IncludeAllManualHiddenProfiles);
                foreach (Wlan.WlanAvailableNetwork network in networks)
                {
                    Wlan.Dot11Ssid ssid = network.dot11Ssid;
                    Wlan.WlanBssEntry[] bssEntries = usableInterface.GetNetworkBssList(ssid, network.dot11BssType, network.securityEnabled);

                    foreach (Wlan.WlanBssEntry bss in bssEntries)
                    {
                        numAps++;
                        string bssid = BitConverter.ToString(bss.dot11Bssid);
                        string ssidEncoded = BitConverter.ToString(network.dot11Ssid.SSID);
                        this.Invoke(wifiFoundDelegate, bssid, Encoding.UTF8.GetString(network.dot11Ssid.SSID, 0, (int)network.dot11Ssid.SSIDLength), bss.rssi, network.dot11BssType, network.dot11DefaultAuthAlgorithm);

                        lock (lockVictim)
                        {
                            tourStreamWriter.WriteLine("NETWORK;{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};" +
                                "{12};{13};{14};{15};{16};{17};{18};{19};{20};{21};{22};{23};{24};{25};{26};{27}",
                                DateTime.Now.Ticks, DateTime.Now.ToString(), lon, lat, bssid, (int)network.dot11BssType,
                                (int)network.dot11DefaultAuthAlgorithm, (int)network.dot11DefaultCipherAlgorithm,
                                network.dot11Ssid.SSIDLength, ssidEncoded, (int)network.flags, network.morePhyTypes,
                                network.networkConnectable, network.securityEnabled, network.wlanSignalQuality,
                                bss.beaconPeriod, bss.capabilityInformation, bss.chCenterFrequency, 
                                (int)bss.dot11BssPhyType, (int)bss.dot11BssType, bss.hostTimestamp, bss.ieOffset, 
                                bss.ieSize, bss.inRegDomain, bss.phyId, bss.rssi, bss.timestamp, (int)bss.wlanRateSet.GetRateInMbps(0)
                                );
                        }
                    }
                }
                lock (lockVictim)
                {
                    tourStreamWriter.Flush();
                }
                context.MainForm.SetStatusBar(String.Format("{0} Services entdeckt.", numAps));
            }
            scanThreadEnded = true;
            context.MainForm.SetStatusBar("Scan angehalten!");
        }

        delegate void WifiFoundDelegate(string bssid,string ssid,int rssi,Wlan.Dot11BssType bssType,Wlan.Dot11AuthAlgorithm authAlg);

        

        void WiFiFound(string bssid, string ssid, int rssi, Wlan.Dot11BssType bssType, Wlan.Dot11AuthAlgorithm authAlg)
        {
            ListViewItem lvi = new ListViewItem(bssid);
            lvi.SubItems.Add(ssid);
            lvi.SubItems.Add(rssi.ToString());
            lvi.SubItems.Add(authAlg.ToString());
            listView1.Items.Add(lvi);
        }

    }
}
