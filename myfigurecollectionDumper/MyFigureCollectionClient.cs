using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using log4net;
using log4net.Repository.Hierarchy;

namespace myfigurecollectionDumper
{
    class MyFigureCollectionClient : IDisposable
    {
        public MyFigureCollectionClient()
        {
            webClient = new WebClient();
            rng = new Random();
            xmlSerializer = new XmlSerializer(typeof(myfigurecollection));
            logger = LogManager.GetLogger(GetType());
            logger.InfoFormat("Client constructed.");
            
        }

        private const int THROTTLE = 10000;
        private const string QUERY_PAGE = "https://myfigurecollection.net/api_v2.php?type=xml&access=read&object=items&request=search&page={0}";
        private const string QUERY_BY_ID = "https://myfigurecollection.net/api_v2.php?type=xml&access=read&object=items&request=search&id={0}";
        private WebClient webClient;
        private int throttleOps = 0;
        private Random rng;
        private XmlSerializer xmlSerializer;
        private ILog logger;

        private void Throttle()
        {
            if (throttleOps > 0)
            {
                double multiplier = rng.NextDouble();
                multiplier += 0.5;
                int amount = (int) ((double) THROTTLE * multiplier);
                Thread.Sleep(amount);
            }
            throttleOps++;
        }

        public myfigurecollection GetSearchRoot()
        {
            return GetSearchPage(1);
        }

        public byte[] DownloadPhoto(string url)
        {
            Throttle();
            try
            {
                logger.Info("Downloading " + url);
                return webClient.DownloadData(url);
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Download of {0} failed.", url);
                return null;
            }
        }

        public myfigurecollection QueryById(long figId)
        {
            Throttle();
            string url = String.Format(QUERY_BY_ID, figId);
            logger.Info("Downloading " + url);
            string result = webClient.DownloadString(url);
            return ParseFromString(result);
        }

        public myfigurecollection GetSearchPage(short page)
        {
            Throttle();
            string url = String.Format(QUERY_PAGE, page);
            logger.Info("Downloading " + url);
            string result = webClient.DownloadString(url);
            return ParseFromString(result);
        }

        public myfigurecollection ParseFromString(string input)
        {
            StringReader sr = new StringReader(input);
            XmlReader xr = XmlReader.Create(sr);
            myfigurecollection result = (myfigurecollection)xmlSerializer.Deserialize(xr);
            xr.Dispose();
            sr.Close();
            return result;
        }

        public void Dispose()
        {
            webClient?.Dispose();
        }
    }
}
