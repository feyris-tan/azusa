using System;
using System.Data.Common;
using System.Drawing;
using System.IO;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
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

        public static Bitmap GetPicture(this DbDataReader reader, int ordinal)
        {
            byte[] buffer = GetByteArray(reader, ordinal);
            if (buffer == null)
                return null;
            if (buffer.Length == 0)
                return null;
            MemoryStream ms = new MemoryStream(buffer);
            Image result = Image.FromStream(ms);
            ms.Dispose();
            return (Bitmap)result;
        }
    }
}
