using System.Collections.Generic;
using System.Text;

namespace libazusax360
{
    public class FSFilenameComparer : IComparer<string>
    {
        // Methods
        public int Compare(string x, string y)
        {
            int length;
            if (x.Length > y.Length)
            {
                length = x.Length;
            }
            else
            {
                length = y.Length;
            }
            byte[] array = new byte[length];
            byte[] buffer2 = new byte[length];
            Encoding.ASCII.GetBytes(x.ToUpper()).CopyTo(array, 0);
            Encoding.ASCII.GetBytes(y.ToUpper()).CopyTo(buffer2, 0);
            for (int i = 0; i < length; i++)
            {
                if (array[i] < buffer2[i])
                {
                    return -1;
                }
                if (array[i] > buffer2[i])
                {
                    return 1;
                }
            }
            return 0;
        }
    }
}