using System;
using System.Globalization;

namespace moe.yo3explorer.azusa.DexcomHistory.Entity
{
    public class ManualDataEntity
    {
        public long pid;
        public DateTime dateAdded, ts;
        public short messwert;
        public string einheit;
        public Nullable<byte> be, novorapid, levemir;
        public bool hide;
        public int minuteCorrection;
        public string notice;

        public object[] GetDgvRow()
        {
            return new object[] {
                pid,
                dateAdded,
                String.Format("{0}, {1}",ts.DayOfWeek.ToString(),ts.ToString("dd.MM.yyyy HH:mm",CultureInfo.InvariantCulture)),
                messwert,
                einheit,
                be,
                novorapid,
                levemir,
                hide,
            minuteCorrection,
            notice};
        }
    }
}
