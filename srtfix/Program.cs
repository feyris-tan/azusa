using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace srtfix
{
    class Program
    {
        static void Main(string[] args)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars();
            var invalidPathChars = Path.GetInvalidPathChars();
            var s = new string(invalidFileNameChars);
            var s1 = new string(invalidPathChars);
            bool m = s.Equals(s1);

            if (args.Length == 0)
            {
                Console.WriteLine("Usage: srt2html MyFavoriteMovie.GoogleTranslated.srt");
                return;
            }

            FileInfo srt = new FileInfo(args[0]);
            string changedExtension = Path.ChangeExtension(srt.FullName, "fixed.srt");
            FileInfo html = new FileInfo(changedExtension);
            if (html.Exists)
                html.Delete();

            StreamReader inLevel1 = srt.OpenText();
            string firstline = inLevel1.ReadLine();
            if (!firstline.Equals("1"))
            {
                Console.WriteLine("not an srt file!");
                return;
            }

            FileStream outLevel1 = html.OpenWrite();
            StreamWriter outLevel2 = new StreamWriter(outLevel1, Encoding.UTF8);
            outLevel2.WriteLine(firstline);

            while (!inLevel1.EndOfStream)
            {
                string line = inLevel1.ReadLine();
                line = line.Replace(" ​​-> ", " --> ");
                line = line.Replace(" -> ", " --> ");
                line = line.Replace(": ", ":");

                outLevel2.WriteLine(line);
            }

            outLevel2.Flush();
            outLevel1.Flush();
            outLevel1.Close();
        }
    }
}
