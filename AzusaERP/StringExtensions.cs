using System;
using System.Linq;

namespace moe.yo3explorer.azusa
{
    public static class StringExtensions
    {
        public static string unix2dos(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            return input.Replace("\n", "\r\n");
        }

        public static byte[] HexToBytes(this string hex)
        {
            hex = hex.Replace("-", "");
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string EscapePath(this string input)
        {
            input = input.Replace('!', '！');
            input = input.Replace('?', '？');
            input = input.Replace('\"', '＂');
            input = input.Replace('*', '＊');
            return input;
        }

        public static bool ContainsDigits(this string input)
        {
            foreach (char c in input)
            {
                if (Char.IsDigit(c))
                    return true;
            }

            return false;
        }

        public static string MakeFullyQualifiedTableName(this string tableName)
        {
            string[] args = tableName.Split('.');
            if (args[1].ContainsDigits())
            {
                args[1] = String.Format("\"{0}\"", args[1]);
                tableName = String.Join(".", args);
            }

            return tableName;
        }
    }
}
