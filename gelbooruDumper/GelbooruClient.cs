using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;

namespace gelbooruDumper
{
    class GelbooruClient
    {
        public GelbooruClient()
        {
            rng = new Random();
            webClient = new WebClient();
            xmlSerializer = new XmlSerializer(typeof(posts));
            logger = LogManager.GetLogger(GetType());
        }

        private const string ROOT = "https://gelbooru.com/index.php?page=dapi&s=post&q=index&tags={0}";
        private const int THROTTLE = 10000;
        private ulong throttleOps;
        private Random rng;
        private WebClient webClient;
        private XmlSerializer xmlSerializer;
        private ILog logger;

        private void Throttle()
        {
            if (throttleOps > 0)
            {
                double multiplier = rng.NextDouble();
                multiplier += 0.5;
                int amount = (int)((double)THROTTLE * multiplier);
                Thread.Sleep(amount);
            }
            throttleOps++;
        }

        public byte[] DownloadImage(string url)
        {
            Throttle();
            logger.Info("Downloading " + url);
            return webClient.DownloadData(url);
        }

        public posts Search(string tag)
        {
            Throttle();
            logger.Info("Downloading Tag data: " + tag);
            string rawXml = webClient.DownloadString(String.Format(ROOT, tag));
            StringReader rawXmlReader = new StringReader(rawXml);
            posts result = (posts)xmlSerializer.Deserialize(rawXmlReader);
            return result;
        }
    }
}
