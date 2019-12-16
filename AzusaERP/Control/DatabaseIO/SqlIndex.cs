using System;
using System.Xml.Serialization;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    public class SqlIndex
    {
        [XmlAttribute] public bool Unique { get; set; }
        [XmlAttribute] public string TableName { get; set; }
        [XmlAttribute] public string IndexName { get; set; }
        [XmlAttribute] public DateTime Timestamp { get; set; }

        public string[] Columns { get; set; }
        [XmlAttribute] public string SchemaName { get; set; }

        protected bool Equals(SqlIndex other)
        {
            return string.Equals(IndexName, other.IndexName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SqlIndex) obj);
        }

        public override int GetHashCode()
        {
            return (IndexName != null ? IndexName.GetHashCode() : 0);
        }

        public static bool operator ==(SqlIndex left, SqlIndex right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SqlIndex left, SqlIndex right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return IndexName;
        }
    }
}
