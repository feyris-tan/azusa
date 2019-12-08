using System.Collections.Generic;

namespace moe.yo3explorer.azusa
{
    static class HastSetExtensions
    {
        public static int AddAll<T>(this HashSet<T> self, IEnumerable<T> other)
        {
            int result = 0;
            foreach (T value in other)
            {
                if (self.Add(value))
                    result++;
            }

            return result;
        }
    }
}
