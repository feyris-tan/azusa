using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            xmlSerializerTags = new XmlSerializer(typeof(tags));
            logger = LogManager.GetLogger(GetType());
        }

        private const string COMMENTS_ROOT = "https://gelbooru.com/index.php?page=dapi&s=comment&q=index&post_id={0}";
        private const string ROOT = "https://gelbooru.com/index.php?page=dapi&s=post&q=index&tags={0}";
        private const string TAG_ROOT = "https://gelbooru.com/index.php?page=dapi&s=tag&q=index&name={0}";
        private const int THROTTLE = 10000;
        private ulong throttleOps;
        private Random rng;
        private WebClient webClient;
        private XmlSerializer xmlSerializerPosts;
        private XmlSerializer xmlSerializerComments;
        private XmlSerializer xmlSerializerTags;
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
            logger.Info("Querying: " + tag);
            string rawXml = webClient.DownloadString(String.Format(ROOT, tag));
            StringReader rawXmlReader = new StringReader(rawXml);
            posts result = (posts)xmlSerializerPosts.Deserialize(rawXmlReader);
            return result;
        }

        public comments GetComments(int id)
        {
            Throttle();
            logger.Info("Fetching comments: " + id);
            string rawXml = webClient.DownloadString(String.Format(COMMENTS_ROOT, id));
            rawXml = rawXml.Replace("creator=\"Nicole_<3\"", "creator=\"Nicole_&lt;3\"");
            StringReader rawXmlReader = new StringReader(rawXml);
            comments comments = (comments) xmlSerializerComments.Deserialize(rawXmlReader);
            return comments;
        }

        public tagsTag GetTag(string s)
        {
            Throttle();
            logger.Info("Downloading tag info: " + s);
            string sUnescaped = s;
            s = Uri.EscapeDataString(s);
            string rawXml = webClient.DownloadString(String.Format(TAG_ROOT, s));
            int rawXmlLen = rawXml.Length;
            if (rawXmlLen == 57)
            {
                logger.Warn("Could not fetch that one, inserting dummy object!");
                tagsTag tagsTag = new tagsTag();
                tagsTag.name = sUnescaped;
                tagsTag.type = (-1).ToString();
                tagsTag.id = (DateTime.UtcNow.Ticks / -1).ToString();
                tagsTag.ambiguous = "false";
                tagsTag.count = (-1).ToString();
                return tagsTag;
            }
            StringReader rawXmlReader = new StringReader(rawXml);
            tags tags = (tags) xmlSerializerTags.Deserialize(rawXmlReader);
            if (tags.tag.Length == 0)
                throw new FileNotFoundException(s);
            return tags.tag[0];
        }
    }
}
