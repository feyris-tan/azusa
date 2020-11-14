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
            xmlSerializerPosts = new XmlSerializer(typeof(posts));
            xmlSerializerComments = new XmlSerializer(typeof(comments));
            logger = LogManager.GetLogger(GetType());
        }

        private const string COMMENTS_ROOT = "https://gelbooru.com/index.php?page=dapi&s=comment&q=index&post_id={0}";
        private const string ROOT = "https://gelbooru.com/index.php?page=dapi&s=post&q=index&tags={0}";
        private const int THROTTLE = 10000;
        private ulong throttleOps;
        private Random rng;
        private WebClient webClient;
        private XmlSerializer xmlSerializerPosts;
        private XmlSerializer xmlSerializerComments;
        private ILog logger;

        private void Throttle()
        {
            if (throttleOps > 0)
            {
                double multiplier = rng.NextDouble();
                multiplier += 0.5;
                int amount = (int)((double)THROTTLE * multiplier);
                amount += (int)throttleOps;
                Thread.Sleep(amount);
            }
            throttleOps++;
        }
        
        public posts Search(string tag)
        {
            Throttle();
            logger.Info("Downloading Tag data: " + tag);
            string rawXml = webClient.DownloadString(String.Format(ROOT, tag));
            StringReader rawXmlReader = new StringReader(rawXml);
            posts result = (posts)xmlSerializerPosts.Deserialize(rawXmlReader);
            return result;
        }

        public comments GetComments(int id)
        {
            Throttle();
            logger.Info("Downloading comment data: " + id);
            string rawXml = webClient.DownloadString(String.Format(COMMENTS_ROOT, id));
            rawXml = rawXml.Replace("creator=\"Nicole_<3\"", "creator=\"Nicole_&lt;3\"");
            StringReader rawXmlReader = new StringReader(rawXml);
            comments comments = (comments) xmlSerializerComments.Deserialize(rawXmlReader);
            return comments;
        }
    }
}
