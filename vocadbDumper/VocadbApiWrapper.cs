using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
using log4net;

namespace vocadbDumper
{
    class VocadbApiWrapper
    {
        public VocadbApiWrapper(string userAgent)
        {
            configuration = new Configuration();
            apiClient = new ApiClient();
            apiClient.AddDefaultHeader("User-Agent", userAgent);
            albumApi = new AlbumApiApi(apiClient);
            artistApi = new ArtistApiApi(apiClient);
            webClient = new WebClient();
            logger = LogManager.GetLogger(GetType());
            logger.Info("Client ready.");
        }

        private Configuration configuration;
        private ApiClient apiClient;
        private AlbumApiApi albumApi;
        private ILog logger;
        private ArtistApiApi artistApi;
        private WebClient webClient;
        
        public List<AlbumForApiContract> GetNewAlbums()
        {
            BeforeRequest();
            logger.Info("Getting new Albums...");
            return albumApi.AlbumApiGetNewAlbums("Default", "None");
        }

        public List<AlbumForApiContract> GetTopAlbums()
        {
            BeforeRequest();
            logger.Info("Getting top albums...");
            return albumApi.AlbumApiGetTopAlbums(new List<int?>(), "Default", "None");
        }

        public List<AlbumForApiContract> BrowseAlbumList(int offset)
        {
            BeforeRequest();
            logger.InfoFormat("Browsing Album List at offset {0}", offset);
            PartialFindResultAlbumForApiContract findResultAlbumForApiContract = albumApi.AlbumApiGetList(null, null, null, null, null, null, null, null, null, null, null, null,
                null, null, offset, 50, null, null, null, null, null, null, null);
            return findResultAlbumForApiContract.Items;
        }

        public List<SongInAlbumForApiContract> GetAlbumTracks(int albumId)
        {
            BeforeRequest();
            logger.InfoFormat("Getting Track list for album: " + albumId);
            List<SongInAlbumForApiContract> songInAlbumForApiContracts = albumApi.AlbumApiGetTracks(albumId, "Artists", "Default");
            return songInAlbumForApiContracts;
        }

        public ArtistForApiContract GetArtistAlbums(int artistId)
        {
            BeforeRequest();
            ArtistForApiContract artistApiGetOne = artistApi.ArtistApiGetOne(artistId, "MainPicture", "All", "Default");
            return artistApiGetOne;
        }

        public byte[] DownloadImage(string url)
        {
            Thread.Sleep(100);
            logger.Info("Downloading: " + url);
            return webClient.DownloadData(url);
        }

        public event BeforeRequestDelegate BeforeRequest;
    }

    public delegate void BeforeRequestDelegate();
}
