using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace srt2html
{
    class Program
    {
        private static string GuessLangaugeFromFilename(string name)
        {
            if (name.Contains("French"))
                return "fr";

            return "en";
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: srt2html MyFavoriteMovie.French.srt");
                return;
            }

            FileInfo srt = new FileInfo(args[0]);
            string changedExtension = Path.ChangeExtension(srt.FullName, ".html");
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

            outLevel2.WriteLine("<!DOCTYPE html>");
            outLevel2.WriteLine("<html lang=\"{0}\">",GuessLangaugeFromFilename(srt.Name));
            outLevel2.WriteLine("\t<head>");
            outLevel2.WriteLine("\t\t<title>{0}</title>", srt.Name);
            outLevel2.WriteLine("\t\t<meta charset=\"UTF-8\">");
            outLevel2.WriteLine("\t\t<meta name=\"author\" content=\"{0}\">",Environment.UserName);
            outLevel2.WriteLine("\t\t<meta name=\"robots\" content=\"noindex\"/>");
            outLevel2.WriteLine("\t\t<meta name=\"generator\" content=\"srt2html\">");
            outLevel2.WriteLine("\t</head>");
            outLevel2.WriteLine("\t<body style=\"background-color: #000; color:#fff; \">");
            outLevel2.WriteLine("\t\t{0}<br/>", firstline);
            while (!inLevel1.EndOfStream)
            {
                string inLine = inLevel1.ReadLine();
                inLine = HttpUtility.HtmlEncode(inLine);
                outLevel2.WriteLine("\t\t{0}<br/>", inLine);
            }
            outLevel2.WriteLine("\t</body>");
            outLevel2.WriteLine("</html>");
            outLevel2.Flush();
            outLevel2.Close();
            outLevel1.Close();
            inLevel1.Close();
        }
    }
}
