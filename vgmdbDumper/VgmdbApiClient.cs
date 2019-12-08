using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using vgmdbDumper.Model;
using Newtonsoft.Json;
using log4net;
using System.Threading;

namespace vgmdbDumper
{
    class VgmdbApiClient
    {
        public static VgmdbApiClient GetInstance()
        {
            if (instance == null)
            {
                instance = new VgmdbApiClient();
            }
            return instance;
        }

        private VgmdbApiClient()
        {
            webClient = new WebClient();
            logger = LogManager.GetLogger(GetType());
        }

        private static VgmdbApiClient instance;
        private WebClient webClient;
        private ILog logger;

        public AlbumList GetAlbumList(char c)
        {
            string urlEncoded = WebUtility.UrlEncode(new string(c, 1));
            string url = String.Format("https://vgmdb.info/albumlist/{0}?format=json", urlEncoded);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<AlbumList>(decoded);
        }

        public ArtistList GetArtistList(char c)
        {
            string urlEncoded = WebUtility.UrlEncode(new string(c, 1));
            string url = String.Format("http://vgmdb.info/artistlist/{0}?format=json", urlEncoded);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<ArtistList>(decoded);
        }

        public ProductList GetProductList(char c)
        {
            string urlEncoded = WebUtility.UrlEncode(new string(c, 1));
            string url = String.Format("https://vgmdb.info/productlist/{0}?format=json", urlEncoded);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<ProductList>(decoded);
        }

        public LabelList GetLabelList()
        {
            string decoded = Download("https://vgmdb.info/orglist/I?format=json");
            return JsonConvert.DeserializeObject<LabelList>(decoded);
        }

        public EventList GetEventList()
        {
            string decoded = Download("https://vgmdb.info/eventlist/2001?format=json");
            return JsonConvert.DeserializeObject<EventList>(decoded);
        }

        private string Download(string url)
        {
            try
            {
                logger.Info("Downloading " + url);
                byte[] downloaded = webClient.DownloadData(url);
                string decoded = Encoding.UTF8.GetString(downloaded);
                return decoded;
            }
            catch (WebException we)
            {
                if (we.Status == WebExceptionStatus.Timeout)
                {
                    logger.Warn("Download timeout. Retrying.");
                    Thread.Sleep(1000);
                    return Download(url);
                }
                throw we;
            }
        }

        public byte[] DownloadGraphic(string url)
        {
            logger.Info("Downloading " + url);
            try
            {
                byte[] downloaded = webClient.DownloadData(url);
                return downloaded;
            }
            catch (Exception e)
            {
                logger.Warn("Download failed: " + e.Message);
                return new byte[] { };
            }
        }

        public Product GetProduct(int id)
        {
            string url = String.Format("https://vgmdb.info/product/{0}?format=json",id);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Product>(decoded);
        }

        public Release GetRelease(int id)
        {
            string url = String.Format("https://vgmdb.info/release/{0}?format=json",id);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Release>(decoded);
        }

        internal Album GetAlbum(int value)
        {
            string url = String.Format("https://vgmdb.info/album/{0}?format=json", value);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Album>(decoded);
        }

        internal Event GetEventList(int id)
        {
            string url = String.Format("https://vgmdb.info/event/{0}?format=json", id);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Event>(decoded);
        }

        public Label GetLabel(int id)
        {
            string url = String.Format("https://vgmdb.info/org/{0}?format=json",id);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Label>(decoded);
        }

        public Artist GetArtist(int id)
        {
            string url = String.Format("https://vgmdb.info/artist/{0}?format=json", id);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<Artist>(decoded);
        }

        public UpdateList GetUpdateList(string section)
        {
            string url = String.Format("https://vgmdb.info/recent/{0}?format=json", section);
            string decoded = Download(url);
            return JsonConvert.DeserializeObject<UpdateList>(decoded);
        }
    }
}
