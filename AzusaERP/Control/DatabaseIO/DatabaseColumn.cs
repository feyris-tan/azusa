using System.Data;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    public class DatabaseColumn
    {
        public string ParameterName { get; set; }
        public string ColumnName { get; set; }
        public DbType DbType { get; set; }
        public int Ordingal { get; set; }
    }
}
