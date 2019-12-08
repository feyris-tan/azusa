using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzusaERP.DatabaseIO
{
    static class DbDataReaderExtensions
    {
        public static byte[] GetByteArray(this DbDataReader reader,int ordinal)
        {
            if (reader.IsDBNull(ordinal))
                return null;

            Stream stream = reader.GetStream(ordinal);
            byte[] result = new byte[stream.Length];
            int size = stream.Read(result, 0, result.Length);
            if (size != stream.Length)
                throw new Exception("read stream fully failed");
            stream.Dispose();
            return result;
        }
    }
}
