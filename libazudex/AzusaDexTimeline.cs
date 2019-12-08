using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using moe.yo3explorer.azusa.dex.Schema;
using moe.yo3explorer.azusa.dex.Schema.Enums;

namespace moe.yo3explorer.azusa.dex
{
    public class AzusaDexTimeline
    {
        public AzusaDexTimeline()
        {
            wrapped = new List<DexTimelineEntry>();
            Timestamp = DateTime.Now;
            UUID = Guid.NewGuid();
        }

        private static XmlSerializer xs;
        private List<DexTimelineEntry> wrapped;

        public void Order()
        {
            wrapped.Sort((x, y) => x.Timestamp.CompareTo(y.Timestamp));
        }
        
        private DexTimelineEntry GetTimelineEntry(DateTime dateTime)
        {
            DexTimelineEntry candidate = wrapped.FirstOrDefault(x => x.MatchTimestamp(dateTime));
            if (candidate != null)
            {
                return candidate;
            }

            candidate = new DexTimelineEntry(dateTime);
            wrapped.Add(candidate);
            return candidate;
        }

        public DexTimelineEntry this[DateTime value]
        {
            get { return GetTimelineEntry(value); }
        }

        public void SaveCsv(FileInfo fi, bool filter = true, bool rewrite_date_headers = false,bool use_oa_date = false)
        {
            FileStream fs = fi.OpenWrite();
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            SaveCsv(sw, filter, rewrite_date_headers,use_oa_date);
            fs.Flush();
            fs.Close();
        }
        
        public void SaveCsv(TextWriter tw, bool filter,bool rewrite_date_headers, bool use_oa_date)
        {
            Order();
            DateTime oldDate = Data[0].Timestamp.Date;
            string dateHeader = "TIMESTAMP;GLUCOSE;METER_GLUCOSE;CARBS;INSULIN;";
            tw.WriteLine(dateHeader);
            foreach (DexTimelineEntry dte in Data)
            {
                /*
                    [x] private DateTime _timestamp;
                    [ ] private UInt32? _sensorFiltered;
                    [ ] private UInt32? _sensorUnfiltered;
                    [ ] private UInt32? _rssi;
                    [x] private UInt32? _glucose;
                    [ ] private SessionState? _sessionState;
                    [x] private UInt32? _meterGlucose;
                    [ ] private EventType? _eventType;
                    [x] private Double? _carbs;
                    [x] private Double? _insulin;
                    [ ] private HealthSubType? _healthEvent;
                    [ ] private ExerciseSubType? _exerciseEvent;
                    [ ] private SpecialGlucoseValue? _specialGlucoseValue;
                 */
                if ((filter & KeepWithFilter(dte) || (!filter)))
                {
                    DateTime newDate = dte.Timestamp.Date;
                    if (!newDate.Equals(oldDate) && rewrite_date_headers)
                    {
                        tw.WriteLine("");
                        tw.WriteLine(dateHeader);
                    }

                    oldDate = newDate;
                    tw.WriteLine("{0};{1};{2};{3};{4}",
                        use_oa_date ? dte.Timestamp.ToOADate().ToString() : dte.Timestamp.ToShortDateString() + " " + dte.Timestamp.ToLongTimeString(),
                        dte.Glucose.HasValue ? dte.Glucose.ToString() : "",
                        dte.MeterGlucose.HasValue ? dte.MeterGlucose.ToString() : "",
                        dte.Carbs.HasValue ? dte.Carbs.ToString() : "",
                        dte.Insulin.HasValue ? dte.Insulin.ToString() : "");
                }
            }
            tw.Flush();
        }
        
        public void SaveTo(FileInfo fi)
        {
            FileStream fs = fi.OpenWrite();
            SaveTo(fs);
            fs.Close();
        }
        
        public void SaveTo(Stream str)
        {
            if (xs == null)
                xs = new XmlSerializer(typeof(AzusaDexTimeline));

            Order();
            xs.Serialize(str, this);
        }

        public static AzusaDexTimeline LoadFrom(Stream str)
        {
            if (xs == null)
                xs = new XmlSerializer(typeof(AzusaDexTimeline));

            AzusaDexTimeline result = (AzusaDexTimeline)xs.Deserialize(str);
            return result;
        }

        public static AzusaDexTimeline LoadFrom(FileInfo fi)
        {
            FileStream fs = fi.OpenRead();
            AzusaDexTimeline result = LoadFrom(fs);
            fs.Close();
            return result;
        }

        public static AzusaDexTimeline LoadFrom(DexcomDevice dd)
        {
            AzusaDexTimeline result = new AzusaDexTimeline();
            result.Timestamp = dd.ReadRTC();
            PartitionInfo pi = dd.ReadDatabaseParitionInfo();
            for (int i = 0; i < pi.Partition.Length; i++)
            {
                DatabasePageRange dpr = dd.ReadDatabasePageRange(pi.Partition[i]);
                if (dpr.Start == -1)
                    continue;
                for (int j = dpr.Start; j < dpr.End; j++)
                {
                    DatabasePage page = dd.ReadDatabasePage(pi.Partition[i], j);
                    for (int k = 0; k < page.Record.Length; k++)
                    {
                        switch (page.RecordType)
                        {
                            case RecordType.SensorData:
                                SensorData sd = page.Record[k] as SensorData;
                                result[sd.DisplayTime].Rssi = sd.Rssi;
                                result[sd.DisplayTime].SensorFiltered = sd.Filtered;
                                result[sd.DisplayTime].SensorUnfiltered = sd.Unfiltered;
                                break;
                            case RecordType.EgvData:
                                EGVRecord egv = page.Record[k] as EGVRecord;
                                if (egv.SpecialGlucoseValue == null)
                                {
                                    result[egv.DisplayTime].Glucose = egv.Glucose;
                                }
                                else
                                {
                                    result[egv.DisplayTime].SpecialGlucoseValue = egv.SpecialGlucoseValue;
                                }
                                result[egv.DisplayTime].TrendArrow = egv.TrendArrow;
                                break;
                            case RecordType.InsertionTime:
                                InsertionTime it = page.Record[k] as InsertionTime;
                                result[it.DisplayTime].SessionState = it.SessionState;
                                break;
                            case RecordType.MeterData:
                                MeterData md = page.Record[k] as MeterData;
                                result[md.DisplayTime].MeterGlucose = md.MeterGlucose;
                                break;
                            case RecordType.UserEventData:
                                UserEventData ued = page.Record[k] as UserEventData;
                                result[ued.DisplayTime2].EventType = ued.EventType;
                                switch (ued.EventType)
                                {
                                    case EventType.Carbs:
                                        result[ued.DisplayTime2].Carbs = ued.EventValue;
                                        break;
                                    case EventType.Insulin:
                                        result[ued.DisplayTime2].Insulin = ued.EventValue;
                                        break;
                                    default:
                                        result[ued.DisplayTime2].HealthEvent = ued.HealthEvent;
                                        result[ued.DisplayTime2].ExerciseEvent = ued.ExerciseEvent;
                                        break;
                                }
                                break;
                            case RecordType.ManufacturingData:
                                ManufacturingParameters mp = page.Record[k] as ManufacturingParameters;
                                result[mp.DisplayTime].ManufacturingParameters = mp;
                                break;
                            case RecordType.FirmwareParameterData:
                                FirmwareParameterData fpd = page.Record[k] as FirmwareParameterData;
                                result[fpd.DisplayTime].FirmwareParameters = fpd.FPR;
                                break;
                            case RecordType.PcSoftwareParameter:
                                PcSoftwareParameter pcp = page.Record[k] as PcSoftwareParameter;
                                result[pcp.DisplayTime].PcSoftwareParameter = pcp;
                                break;
                            case RecordType.CalSet:
                            case RecordType.ReceiverLogData:
                            case RecordType.UserSettingData:
                            case RecordType.Deviation:
                                break;
                            default:
                                throw new NotImplementedException(page.RecordType.ToString());
                        }
                    }
                }
            }
            
            result.Order();
            return result;
        }

        public List<DexTimelineEntry> Data
        {
            get { return wrapped; }
            set { wrapped = value; }
        }

        public bool KeepWithFilter(DexTimelineEntry dte)
        {
            if (!wrapped.Contains(dte))
                return false;

            if (dte.SpecialGlucoseValueSpecified)
                return false;

            int index = wrapped.IndexOf(dte);

            if (index < 3)
                return false;

            if (index > (wrapped.Count - 3))
                return false;

            for (int i = index - 3; i < index + 3; i++)
            {
                if (wrapped[i].CarbsSpecified)
                    return true;
                if (wrapped[i].InsulinSpecified)
                    return true;
                if (wrapped[i].MeterGlucoseSpecified)
                    return true;
            }

            return false;
        }
        
        [XmlAttribute]
        public DateTime Timestamp { get; set; }
        
        [XmlAttribute]
        public Guid UUID { get; set; }
    }
    
    public class DexTimelineEntry
    {
        public DexTimelineEntry() {}
        public DexTimelineEntry(DateTime dateTime)
        {
            Timestamp = dateTime;
        }
        
        private DateTime _timestamp;
        private UInt32? _sensorFiltered;
        private UInt32? _sensorUnfiltered;
        private UInt32? _rssi;
        private UInt32? _glucose;
        private TrendArrow? _trendArrow;
        private SessionState? _sessionState;
        private UInt32? _meterGlucose;
        private EventType? _eventType;
        private Double? _carbs;
        private Double? _insulin;
        private HealthSubType? _healthEvent;
        private ExerciseSubType? _exerciseEvent;
        private SpecialGlucoseValue? _specialGlucoseValue;

        [XmlAttribute]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute,0); }
        }

        public uint? SensorFiltered
        {
            get { return _sensorFiltered; }
            set { _sensorFiltered = value; }
        }

        [XmlIgnore]
        public bool SensorFilteredSpecified { get { return SensorFiltered.HasValue; }}

        public uint? SensorUnfiltered
        {
            get { return _sensorUnfiltered; }
            set {
                    _sensorUnfiltered = value;
                }
            }
        
        [XmlIgnore]
        public bool SensorUnfilteredSpecified { get { return SensorUnfiltered.HasValue; }}

        public uint? Rssi
        {
            get { return _rssi; }
            set { _rssi = value;}
        }
        
        [XmlIgnore]
        public bool RssiSpecified { get { return Rssi.HasValue; }}

        public uint? Glucose
        {
            get { return _glucose; }
            set { _glucose = value; }
        }
        
        [XmlIgnore]
        public bool GlucoseSpecified { get { return Glucose.HasValue; }}

        public TrendArrow? TrendArrow
        {
            get { return _trendArrow; }
            set { _trendArrow = value; }
        }
        
        [XmlIgnore]
        public bool TrendArrowSpecified { get { return TrendArrow.HasValue; }}

        public SessionState? SessionState
        {
            get { return _sessionState; }
            set { _sessionState = value; }
        }
        
        [XmlIgnore]
        public bool SessionStateSpecified { get { return SessionState.HasValue; }}

        public uint? MeterGlucose
        {
            get { return _meterGlucose; }
            set { _meterGlucose = value; }
        }
        
        [XmlIgnore]
        public bool MeterGlucoseSpecified { get { return MeterGlucose.HasValue; }}

        public EventType? EventType
        {
            get { return _eventType; }
            set { _eventType = value; }
        }
        
        [XmlIgnore]
        public bool EventTypeSpecified { get { return EventType.HasValue; }}

        public Double? Carbs
        {
            get { return _carbs; }
            set { _carbs = value; }
        }
        
        [XmlIgnore]
        public bool CarbsSpecified { get { return Carbs.HasValue; }}

        public double? Insulin
        {
            get { return _insulin; }
            set { _insulin = value; }
        }
        
        [XmlIgnore]
        public bool InsulinSpecified { get { return Insulin.HasValue; }}

        public HealthSubType? HealthEvent
        {
            get { return _healthEvent; }
            set { _healthEvent = value; }
        }
        
        [XmlIgnore]
        public bool HealthEventSpecified { get { return HealthEvent.HasValue; }}

        public ExerciseSubType? ExerciseEvent
        {
            get { return _exerciseEvent; }
            set { _exerciseEvent = value; }
        }
        
        [XmlIgnore]
        public bool ExerciseEventSpecified { get { return ExerciseEvent.HasValue; }}

        public SpecialGlucoseValue? SpecialGlucoseValue
        {
            get { return _specialGlucoseValue; }
            set
            { _specialGlucoseValue = value; }
        }
        
        [XmlIgnore]
        public bool SpecialGlucoseValueSpecified { get { return SpecialGlucoseValue.HasValue; }}

        public ManufacturingParameters ManufacturingParameters { get; set; }

        [XmlIgnore]
        public bool ManufacturingParametersSpecified { get { return ManufacturingParameters != null; } }

        public FPR FirmwareParameters { get; set; }

        [XmlIgnore]
        public bool FirmwareParametersSpecified { get { return FirmwareParameters != null; } }

        public PcSoftwareParameter PcSoftwareParameter { get; set; }

        [XmlIgnore]
        public bool PcSoftwareParameterSpecified { get { return PcSoftwareParameter != null; } }

        #region Equals
        protected bool Equals(DexTimelineEntry other)
        {
            return _timestamp.Equals(other._timestamp);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DexTimelineEntry) obj);
        }

        public override int GetHashCode()
        {
            return _timestamp.GetHashCode();
        }

        public bool MatchTimestamp(DateTime dateTime)
        {
            DateTime rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute,0);
            return rounded.Equals(_timestamp);
        }
        #endregion Equals
        
    }
}