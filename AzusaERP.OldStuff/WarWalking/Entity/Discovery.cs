using System;
using moe.yo3explorer.azusa.WarWalking.Control;

namespace moe.yo3explorer.azusa.WarWalking.Entity
{
    public class Discovery
    {
        private string bssid, ssid;
        private int rssi;
        private float lon, lat;
        private Wlan.Dot11BssType dot11bssType;
        private Wlan.Dot11AuthAlgorithm defaultAuthAlgorithm;
        private Wlan.Dot11CipherAlgorithm defaultCipherAlgorithm;
        private DateTime discoveryUtime;
        private int flags;
        private bool morePhyTypes, networkConnectable, securityEnable;
        private int wlanSignalQuality, beaconPeriod, capabilityInformation;
        private int chCenterFrequency;
        private Wlan.Dot11PhyType dot11bssPhyType;
        private long hostTimestamp;
        private int ieOffset, ieSize;
        private bool inRegDomain;
        private int phyId;
        private long timestamp;
        private int mbps;
        private DateTime dateAdded;
        private int relatedTour;
        private DateTime strogestSignalUtime;
        private int strongestTour;

        public string Bssid
        {
            get { return bssid; }
            set { bssid = value; }
        }

        public string Ssid { get { return ssid;} set {ssid = value; }}
        public int Rssi { get { return rssi;} set {rssi = value; }}
        public float Lon { get { return lon;} set {lon = value; }}
        public float Lat { get { return lat;} set {lat = value; }}
        public Wlan.Dot11BssType Dot11bssType { get { return dot11bssType;} set {dot11bssType = value; }}
        public Wlan.Dot11AuthAlgorithm DefaultAuthAlgorithm { get { return defaultAuthAlgorithm;} set {defaultAuthAlgorithm = value; }}
        public Wlan.Dot11CipherAlgorithm DefaultCipherAlgorithm { get { return defaultCipherAlgorithm;} set {defaultCipherAlgorithm = value; }}
        public DateTime DiscoveryUtime { get { return discoveryUtime;} set {discoveryUtime = value; }}
        public int Flags { get { return flags;} set {flags = value; }}
        public bool MorePhyTypes { get { return morePhyTypes;} set {morePhyTypes = value; }}
        public bool NetworkConnectable { get { return networkConnectable;} set {networkConnectable = value; }}
        public bool SecurityEnable { get { return securityEnable;} set {securityEnable = value; }}
        public int WlanSignalQuality { get { return wlanSignalQuality;} set {wlanSignalQuality = value; }}
        public int BeaconPeriod { get { return beaconPeriod;} set {beaconPeriod = value; }}
        public int CapabilityInformation { get { return capabilityInformation;} set {capabilityInformation = value; }}
        public int ChCenterFrequency { get { return chCenterFrequency;} set {chCenterFrequency = value; }}
        public Wlan.Dot11PhyType Dot11bssPhyType { get { return dot11bssPhyType;} set {dot11bssPhyType = value; }}
        public long HostTimestamp { get { return hostTimestamp;} set {hostTimestamp = value; }}
        public int IeOffset { get { return ieOffset;} set {ieOffset = value; }}
        public int IeSize { get { return ieSize;} set {ieSize = value; }}
        public bool InRegDomain { get { return inRegDomain;} set {inRegDomain = value; }}
        public int PhyId { get { return phyId;} set {phyId = value; }}
        public long Timestamp { get { return timestamp;} set {timestamp = value; }}
        public int Mbps { get { return mbps;} set {mbps = value; }}
        public DateTime DateAdded { get { return dateAdded;} set {dateAdded = value; }}
        public int RelatedTour { get { return relatedTour;} set {relatedTour = value; }}
        public DateTime StrogestSignalUtime { get { return strogestSignalUtime;} set {strogestSignalUtime = value; }}
        public int StrongestTour { get { return strongestTour; } set { strongestTour = value; } }
    }
}
