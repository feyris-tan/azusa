using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace fileTimeline
{
    class TimelineDate : ListViewItem, IEquatable<TimelineDate>
    {
        public TimelineDate(DateTime dt)
        {
            this.Date = dt;
            this.files = new List<FileInfo>();
            this.ForeColor = Color.Red;
        }

        public readonly DateTime Date;
        public List<FileInfo> files;

        public bool Equals(TimelineDate other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Date.Equals(other.Date);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimelineDate) obj);
        }

        public override int GetHashCode()
        {
            return Date.GetHashCode();
        }

        public static bool operator ==(TimelineDate left, TimelineDate right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TimelineDate left, TimelineDate right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"{nameof(Date)}: {Date.ToShortDateString()}";
        }
    }
}