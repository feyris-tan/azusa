using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace libazuwarc
{
    public class CdxContext
    {
        public CdxContext()
        {
            entries = new List<CdxEntry>();
            warcFiles = new Dictionary<string, FileInfo>();
            cdxFiles = new List<FileInfo>();
        }

        private List<CdxEntry> entries;
        private Dictionary<string, FileInfo> warcFiles;
        private List<FileInfo> cdxFiles;

        private FileInfo currentWarcFileInfo;
        private Stream currentWarcFileStream;
        private string currentWarcFileName;

        public void AddCdx(FileInfo fi)
        {
            if (cdxFiles.Contains(fi))
                return;

            cdxFiles.Add(fi);
            
            StreamReader sr = fi.OpenText();
            string headerLine = sr.ReadLine();
            char seperator = headerLine[0];
            if (headerLine[1] != 'C' || headerLine[2] != 'D' || headerLine[3] != 'X')
            {
                throw new Exception("invalid magic");
            }

            headerLine = headerLine.Substring(5);
            string[] headerArgs = headerLine.Split(seperator);
            int aIndex = headerArgs.IndexOf(x => x.Equals("a"));
            int bIndex = headerArgs.IndexOf(x => x.Equals("b"));
            int kIndex = headerArgs.IndexOf(x => x.Equals("k"));
            int SIndex = headerArgs.IndexOf(x => x.Equals("S"));
            int VIndex = headerArgs.IndexOf(x => x.Equals("V"));
            int gIndex = headerArgs.IndexOf(x => x.Equals("g"));
            int uIndex = headerArgs.IndexOf(x => x.Equals("u"));

            string line;
            string[] lineArgs;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();
                lineArgs = line.Split(seperator);
                CdxEntry child = new CdxEntry();
                child.Url = lineArgs[aIndex];
                child.GrabDate = ConvertFromUnixTimestamp(Convert.ToInt64(lineArgs[bIndex]));
                child.Fingerprint = lineArgs[kIndex];
                child.Size = Convert.ToInt64(lineArgs[SIndex]);
                child.Offset = Convert.ToInt64(lineArgs[VIndex]);
                child.Warc = ResovleWarcFile(lineArgs[gIndex]);
                child.Uuid = ParseGuid(lineArgs[uIndex]);
                entries.Add(child);
            }
        }

        private static DateTime ConvertFromUnixTimestamp(long timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private FileInfo ResovleWarcFile(string fname)
        {
            if (warcFiles.ContainsKey(fname))
                return warcFiles[fname];

            foreach (FileInfo fi in cdxFiles)
            {
                DirectoryInfo cdxDir = fi.Directory;
                FileInfo probeA = new FileInfo(Path.Combine(cdxDir.FullName, fname));
                if (probeA.Exists)
                {
                    warcFiles.Add(fname, probeA);
                    return probeA;
                }
            }

            FileInfo probeB = new FileInfo(fname);
            if (probeB.Exists)
            {
                warcFiles.Add(fname, probeB);
                return probeB;
            }

            throw new FileNotFoundException(fname);
        }

        private const string EVIL_PREFIX = "<urn:uuid:";
        private Guid ParseGuid(string s)
        {
            if (s.StartsWith(EVIL_PREFIX))
                s = s.Substring(EVIL_PREFIX.Length);

            if (s.EndsWith(">"))
                s = s.Substring(0, s.Length - 1);

            return Guid.Parse(s);
        }

        public ReadOnlyCollection<CdxEntry> Entries
        {
            get { return entries.AsReadOnly(); }
        }

        public CdxWebResponse GetResponse(CdxEntry ce)
        {
            if (currentWarcFileInfo != ce.Warc)
            {
                if (currentWarcFileStream != null) currentWarcFileStream.Dispose();

                currentWarcFileInfo = ce.Warc;
                currentWarcFileStream = ce.Warc.OpenRead();
                currentWarcFileName = ce.Warc.Name;
            }

            return CdxWebResponse.Build(ce, currentWarcFileStream);
        }

        public CdxWebResponse GetResponse(string requestUrl)
        {
            CdxEntry entry = entries.Find(x => x.Url.Equals(requestUrl));
            if (entry == null)
                return null;
            return GetResponse(entry);
        }
    }
}
