using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using libazuwarc;
using Supremes;
using Supremes.Nodes;

namespace psxdatacenterDumper
{
    class Program
    {
        private const bool ENABLE_SCREENSHOT_GRAB = true;
        private const bool ENABLE_COVER_GRAB = true;

        private static string[] LIST_URLS =
        {
            "https://psxdatacenter.com/psx2/jlist2.html",
            "https://psxdatacenter.com/psx2/plist2.html",
            "https://psxdatacenter.com/psx2/ulist2.html",
            "https://psxdatacenter.com/ulist.html",
            "https://psxdatacenter.com/plist.html",
            "https://psxdatacenter.com/jlist.html",
            "https://psxdatacenter.com/psp/plist.html",
            "https://psxdatacenter.com/psp/jlist.html",
            "https://psxdatacenter.com/psp/ulist.html",
        };

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("usage: psxdatacenterDumper /path/to/psxdatacenter.cdx");
                return;
            }

            if (!File.Exists("azusa.ini"))
            {
                Console.WriteLine("azusa.ini not found");
                return;
            }
            
            Program instance = new Program();
            instance.cdx = new CdxContext();
            instance.cdx.AddCdx(new FileInfo(args[0]));
            instance.sink = new PostgresSink();
            instance.Run();
            instance.sink.Dispose();
        }

        private CdxContext cdx;
        private IPdcSink sink;

        private void Run()
        {
            foreach (string listUrl in LIST_URLS)
            {
                CdxWebResponse response = cdx.GetResponse(listUrl);
                Document document = Dcsoup.Parse(response.GetResponseStream(), null, listUrl);
                ParseList(document);
            }

            sink.Dispose();
        }

        private void ParseList(Document document)
        {
            string platformName = DetectPlatform(document);
            string regionName = DetectRegion(document);

            Element body = document.Body;
            foreach (Element table in body.Select(".sectiontable"))
            {
                foreach (Element tr in table.Select("tr"))
                {
                    Elements tdElements = tr.Select("td");
                    if (tdElements.Count == 0)
                        continue;

                    if (sink.GameKnown(tdElements[1].Text))
                        continue;

                    bool scrapable = tdElements[0].Children.Count > 0;
                    Game child = new Game();
                    if (scrapable)
                    {
                        Element a = tdElements[0].Select("a")[0];
                        string link = a.Attributes["href"];
                        CdxEntry request = cdx.Entries.FirstOrDefault(x => x.Url.EndsWith(link));
                        CdxWebResponse resp = cdx.GetResponse(request);
                        Document respDoc = Dcsoup.Parse(resp.GetResponseStream(), null, link);
                        child = ScrapeGameData(respDoc);
                        if (child == null)
                        {
                            child = new Game();
                            child.SKU = tdElements[1].Text;
                            child.Title = tdElements[2].Text;
                            child.Language = tdElements[3].Text;
                        }

                        if (child.DateReleased == DateTime.MaxValue)
                            continue;
                    }
                    else
                    {
                        child.SKU = tdElements[1].Text;
                        child.Title = tdElements[2].Text;
                        child.Language = tdElements[3].Text;
                    }

                    child.Platform = platformName;
                    child.Region = regionName;
                    sink.HandleGame(child);
                    Console.WriteLine(child.Title);
                }
            }
        }

        private string DetectPlatform(Document document)
        {
            if (document.Location.Contains("psx2"))
                return "PS2";
            if (document.Location.Contains("/psp/"))
                return "PSP";
            if (document.Location.Equals("https://psxdatacenter.com/ulist.html"))
                return "PS1";
            if (document.Location.Equals("https://psxdatacenter.com/plist.html"))
                return "PS1";
            if (document.Location.Equals("https://psxdatacenter.com/jlist.html"))
                return "PS1";
            throw new NotImplementedException();
        }

        private string DetectRegion(Document document)
        {
            if (document.Location.Contains("ulist"))
                return "NTSC-U";
            else if (document.Location.Contains("jlist"))
                return "NTSC-J";
            else if (document.Location.Contains("plist"))
                return "PAL";
            throw new NotImplementedException();
        }

        private Game ScrapeGameData(Document document)
        {
            /**
             * Selector:
             * "p" finds all <p></p>
             * "#id" finds by id
             * ".class" finds all elements by class
             */
            Game result = new Game();

            Elements h1 = document.Select("h1");
            if (h1.Count > 0)
                if (h1[0].Text.EndsWith("Not Found"))
                    return null;

            Element table2 = document.GetElementById("table2");
            if (table2 == null)
                return null;

            if (ENABLE_COVER_GRAB)
            {
                Element tbody2 = table2.Children[0];
                Element tr2 = tbody2.Children[1];
                Element img2 = tr2.Select("img")[0];
                string coverUrl = img2.Attributes["src"];
                if (coverUrl.StartsWith(".."))
                    coverUrl = coverUrl.Substring(2);
                if (coverUrl.StartsWith("/../../"))
                    coverUrl = coverUrl.Substring(6);
                if (coverUrl.Contains("//"))
                    coverUrl = coverUrl.Replace("//", "/");
                coverUrl = cdx.Entries.First(x => x.Url.EndsWith(coverUrl)).Url;
                result.Cover = cdx.GetResponse(coverUrl).GetResponseStream().ToByteArray();
            }

            Element table4 = document.GetElementById("table4");
            foreach (Element tr4 in table4.Select("tr"))
            {
                string k = tr4.Children[0].Text;
                string v = tr4.Children[1].Text;
                switch (k.ToUpperInvariant())
                {
                    case "OFFICIAL TITLE":
                        result.Title = v;
                        break;
                    case "COMMON TITLE":
                        result.CommonTitle = v;
                        break;
                    case "SERIAL NUMBER(S)":
                        result.SKU = v;
                        break;
                    case "REGION":
                        result.Region = v;
                        break;
                    case "GENRE / STYLE":
                        result.Genre = v;
                        break;
                    case "DEVELOPER":
                        result.Developer = v;
                        break;
                    case "PUBLISHER":
                        result.Publisher = v;
                        break;
                    case "DATE RELEASED":
                        if (v[1] == ' ')
                            v = "0" + v;
                        if (v.StartsWith("31 November"))
                            v = v.Replace("31 November", "01 December");
                        if (v.Length == 4)
                            break;
                        if (v.Contains("Noviembre"))
                            v = v.Replace("Noviembre", "November");
                        if (v.Contains("/"))
                        {
                            v = v.Split('/')[0];
                            v = v.Trim();
                        }
                        if (v.Length == 2)
                            break;
                        if (v[1] == '-')
                        {
                            v = "0" + v;
                            v = v.Replace('-', ' ');
                        }

                        if (v.ToLowerInvariant().Contains("never released"))
                            break;
                        if (v.StartsWith("31 June"))
                            v = v.Replace("31 June", "01 July");
                        if (v.StartsWith("&nbsp"))
                            v = v.Substring(5);
                        if (v[1] == ' ')
                            v = "0" + v;
                        if (v.Contains("Mayo"))
                            v = v.Replace("Mayo", "May");
                        if (v.Contains("Abril"))
                            v = v.Replace("Abril", "April");
                        if (v.Contains("37 "))
                            v = v.Replace("37", "27");
                        if (v.Contains("Ocotber"))
                            v = v.Replace("Ocotber", "October");
                        if (v.Contains("Augutst"))
                            v = v.Replace("Augutst", "August");

                        DateTime datum = DateTime.ParseExact(v, "dd MMMM yyyy", CultureInfo.InvariantCulture);
                        result.DateReleased = datum;
                        break;
                    default:
                        throw new Exception(String.Format("Don't know what {0} means.", k));
                }
            }

            if (sink.GameKnown(result.SKU))
            {
                result.DateReleased = DateTime.MaxValue;
                return result;
            }

            Element table7 = document.GetElementById("table7");
            Elements ul7 = table7.Select("li");
            Element li7 = ul7.Select("li")[0];
            result.Barcode = li7.Text;
            if (result.Barcode.EndsWith("-"))
                result.Barcode = result.Barcode.Substring(0, result.Barcode.Length - 1);
            result.Barcode = result.Barcode.Trim();
            if (result.Barcode.Contains(" or "))
                result.Barcode = result.Barcode.Split(' ')[0];

            Element table13 = document.GetElementById("table13");
            Elements td13 = table13.Select("td");
            foreach (Element td in td13)
            {
                result.Language += td.Text;
            }

            Element table16 = document.GetElementById("table16");
            Element td16 = table16.Select("td").First;
            result.Description = td16.Html;

            if (ENABLE_SCREENSHOT_GRAB)
            {
                Element table22 = document.GetElementById("table22");
                Elements screenshots = table22.Select("img");
                foreach (Element screenshotImg in screenshots)
                {
                    string screenshotUrl = screenshotImg.Attributes["src"];
                    string name = screenshotUrl;
                    if (screenshotUrl.StartsWith(".."))
                        screenshotUrl = screenshotUrl.Substring(2);
                    if (screenshotUrl.StartsWith("/../../"))
                        screenshotUrl = screenshotUrl.Substring(6);
                    screenshotUrl = cdx.Entries.First(x => x.Url.EndsWith(screenshotUrl)).Url;
                    result.Screenshots.Add(new Screenshot(cdx.GetResponse(screenshotUrl).GetResponseStream().ToByteArray(),name));
                }
            }

            result.AdditionalData = true;
            return result;
        }
    }
}
