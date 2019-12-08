using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace libazuwarc
{
    static class IEnumerableExtensions
    {
        public static int IndexOf<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            IEnumerator<T> enumerator = self.GetEnumerator();
            int result = 0;
            while (enumerator.MoveNext())
            { 
                if (predicate.Invoke(enumerator.Current))
                    return result;

                result++;
            }

            return -1;
        }
    }
}
