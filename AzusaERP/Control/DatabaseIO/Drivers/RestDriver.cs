using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Setup;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.OfflineReaders.Gelbooru.Entity;
using moe.yo3explorer.azusa.OfflineReaders.MyFigureCollection.Entity;
using moe.yo3explorer.azusa.OfflineReaders.PsxDatacenter.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VgmDb.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VocaDB.Entity;
using moe.yo3explorer.azusa.Utilities.Dumping.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace moe.yo3explorer.azusa.Control.DatabaseIO.Drivers
{
    enum RestDriverErrorState
    {
        NoError,
        UrlUnspecified,
        LicenseUnspecified,
        LicenseFileDoesNotExist
    }

    class RestDriver : IDatabaseDriver
    {
        public RestDriver()
        {
            context = AzusaContext.GetInstance();
            errorState = RestDriverErrorState.NoError;
            string url = context.ReadIniKey("rest", "url", null);
            if (url == null)
            {
                errorState = RestDriverErrorState.UrlUnspecified;
                return;
            }
            
            if (string.IsNullOrEmpty(context.LicenseKey))
            {
                errorState = RestDriverErrorState.LicenseUnspecified;
            }
            license = context.LicenseKey;

            webClient = new WebClient();
            webClient.BaseAddress = url;
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("User-Agent", "AzusaERP 1.0");
            webClient.Headers.Add("Azusa-Machine-Name", Environment.MachineName);
            webClient.Headers.Add("Azusa-Username", Environment.UserName);
            webClient.Headers.Add("Azusa-User-Domain-Name", Environment.UserDomainName);
            webClient.Headers.Add("Azusa-License", license);
            webClient.Headers.Add("Authorization", "Azusa-License");
            webClient.Headers.Add("Azusa-License-Buffer-Size", context.LicenseLength.ToString());
        }
        

        private AzusaContext context;
        private RestDriverErrorState errorState;
        private string license;
        private WebClient webClient;

        public RestDriverErrorState RestDriverErrorState => errorState;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
        
        public bool ConnectionIsValid()
        {
            if (RestDriverErrorState != RestDriverErrorState.NoError)
                return false;

            try
            {
                string rawJson = webClient.DownloadString("/startup/validate");
                JsonDocument jsonDocument = JsonDocument.Parse(rawJson);
                JsonElement isValid = jsonDocument.RootElement.GetProperty("isValid");
                return isValid.GetBoolean();
            }
            catch (WebException web)
            {
                Console.WriteLine(web.Status);
                if (web.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse hwr = (HttpWebResponse) web.Response;
                    if (hwr.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Console.WriteLine("Die Lizenznummer \"{0}\" wurde abgelehnt.", this.license);
                    }
                    else
                    {
                        Console.WriteLine(hwr.StatusDescription);
                    }
                }
                return false;
            }
        }

        public List<string> GetAllPublicTableNames()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllTableNames()
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllSchemas()
        {
            throw new NotImplementedException();
        }

        public bool Statistics_TestForDate(DateTime today)
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalProducts()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMedia()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingCovers()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingGraphData()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalUndumpedMedia()
        {
            throw new NotImplementedException();
        }

        public int Statistics_GetTotalMissingScreenshots()
        {
            throw new NotImplementedException();
        }

        public void Statistics_Insert(DateTime today, int totalProducts, int totalMedia, int missingCover,
            int missingGraph,
            int undumped, int missingScreenshots)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shop> GetAllShops()
        {
            string rawJson = webClient.DownloadString("/azusa/shops");
            List<Shop> list = JsonConvert.DeserializeObject<List<Shop>>(rawJson);
            return list;
        }

        public IEnumerable<Shelf> GetAllShelves()
        {
            string rawJson = webClient.DownloadString("/azusa/shelves");
            List<Shelf> list = JsonConvert.DeserializeObject<List<Shelf>>(rawJson);
            return list;
        }

        public IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf)
        {
            string rawJson = webClient.DownloadString(String.Format("/azusa/products/inshelf/{0}",shelf.Id));
            JArray deserializeObject = (JArray)JsonConvert.DeserializeObject(rawJson);
            foreach (JToken jToken in deserializeObject)
            {
                ProductInShelf productInShelf = new ProductInShelf();
                productInShelf.CoverSize = jToken.Value<long>("CoverSize");
                productInShelf.BoughtOn = UnixTimeConverter.FromUnixTime(jToken.Value<long>("BoughtOn"));
                productInShelf.ContainsUndumped = jToken.Value<bool>("ContainsUndumped");
                productInShelf.IconId = jToken.Value<int>("IconId");
                productInShelf.Id = jToken.Value<int>("Id");
                productInShelf.MissingGraphData = jToken.Value<int>("MissingGraphData");
                productInShelf.NSFW = jToken.Value<bool>("NSFW");
                productInShelf.Name = jToken.Value<string>("Name");
                productInShelf.NumberOfDiscs = jToken.Value<int>("NumberOfDiscs");
                productInShelf.Price = jToken.Value<double>("Price");
                productInShelf.relatedShelf = shelf;
                yield return productInShelf;
            }
            yield break;
        }

        public int CreateProductAndReturnId(Shelf shelf, string name)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            string rawJson = webClient.DownloadString(String.Format("/azusa/products/{0}", id));
            Product product = JsonConvert.DeserializeObject<Product>(rawJson);
            return product;
        }

        public void UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }

        public void SetCover(Product product)
        {
            throw new NotImplementedException();
        }

        public void SetScreenshot(Product product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            string rawJson = webClient.DownloadString("/azusa/platforms");
            List<Platform> list = JsonConvert.DeserializeObject<List<Platform>>(rawJson);
            return list;
        }

        public IEnumerable<MediaType> GetMediaTypes()
        {
            string rawJson = webClient.DownloadString("/azusa/mediaTypes");
            List<MediaType> list = JsonConvert.DeserializeObject<List<MediaType>>(rawJson);
            return list;
        }

        public IEnumerable<MediaInProduct> GetMediaByProduct(Product prod)
        {
            throw new NotImplementedException();
        }

        public Media GetMediaById(int o)
        {
            throw new NotImplementedException();
        }

        public void UpdateMedia(Media media)
        {
            throw new NotImplementedException();
        }

        public int CreateMediaAndReturnId(int productId, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Country> GetAllCountries()
        {
            string rawJson = webClient.DownloadString("/azusa/countries");
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(rawJson);
            return list;
        }
        
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool CanActivateLicense { get; }

        public void EndTransaction(bool sucessful)
        {
            throw new NotImplementedException();
        }

        public List<DatabaseColumn> Sync_DefineTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public bool Sync_DoesTableExist(string tableName)
        {
            throw new NotImplementedException();
        }

        public void Sync_CreateTable(string tableName, List<DatabaseColumn> columns)
        {
            throw new NotImplementedException();
        }

        public DateTime? Sync_GetLastSyncDateForTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbDataReader Sync_GetSyncReader(string tableName, DateTime? latestSynced)
        {
            throw new NotImplementedException();
        }
        
        public void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage)
        {
            throw new NotImplementedException();
        }
        
        public DateTime? Sync_GetLatestUpdateForTable(string tableName)
        {
            throw new NotImplementedException();
        }

        public DbDataReader Sync_GetUpdateSyncReader(string tableName, DateTime? latestUpdate)
        {
            throw new NotImplementedException();
        }

        public void Sync_CopyUpdatesFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage,
            Queue<object> leftovers)
        {
            throw new NotImplementedException();
        }
    
        public IEnumerable<int> Vgmdb_FindAlbumsByTrackMask(string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vgmdb_FindAlbumsByArbituraryProducts(string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vgmdb_FindAlbumsByAlbumTitle(string text)
        {
            throw new NotImplementedException();
        }

        public AlbumListEntry Vgmdb_FindAlbumForList(int id)
        {
            throw new NotImplementedException();
        }

        public Bitmap Vgmdb_GetAlbumCover(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Vgmdb_FindProductNamesByAlbumId(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Vgmdb_FindArtistNamesByAlbumId(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vgmdb_FindArtistIdsByName(string escaped)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vgmdb_FindAlbumIdsByArtistId(int possibleArtist)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Image> FindCoversByAlbumId(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Vgmdb_FindAlbumsBySkuPart(string startswith)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tuple<string, int, int, string, int>> Vgmdb_FindTrackDataByAlbum(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Vgmdb_FindLabelNamesByAlbumId(int entryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Vgmdb_FindRelatedAlbums(int albumId)
        {
            throw new NotImplementedException();
        }

        public string Vgmdb_GetReleaseEvent(int albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> Vgmdb_FindReprints(int albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Uri> Vgmdb_GetWebsites(int albumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PsxDatacenterPreview> PsxDc_Search(string textBox)
        {
            throw new NotImplementedException();
        }

        public PsxDatacenterGame PsxDc_GetSpecificGame(int previewId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<byte[]> PsxDc_GetScreenshots(int previewId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SqlIndex> GetSqlIndexes()
        {
            throw new NotImplementedException();
        }

        public void CreateIndex(SqlIndex index)
        {
            throw new NotImplementedException();
        }

        public void ForgetFilesystemContents(int currentMediaId)
        {
            throw new NotImplementedException();
        }

        public void AddFilesystemInfo(FilesystemMetadataEntity dirEntity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FilesystemMetadataEntity> GetFilesystemMetadata(int currentMediaId, bool dirs)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VndbSearchResult> Vndb_Search(string searchquery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VndbVnResult> Vndb_GetVnsByRelease(int searchResultRid)
        {
            throw new NotImplementedException();
        }

        public VndbRelease Vndb_GetReleaseById(int releaseResultRid)
        {
            throw new NotImplementedException();
        }

        public VndbVn Vndb_GetVnById(int vnResultVnid)
        {
            throw new NotImplementedException();
        }

        public DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Figure> MyFigureCollection_Search(string query)
        {
            throw new NotImplementedException();
        }

        public Image MyFigureCollection_GetPhoto(int wrappedFigureId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VocadbSearchResult> VocaDb_Search(string text)
        {
            throw new NotImplementedException();
        }

        public Image Vocadb_GetAlbumCover(int id)
        {
            throw new NotImplementedException();
        }

        public List<string> VocaDb_FindAlbumNamesBySongNames(string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<VocadbTrackEntry> VocaDb_GetTracksByAlbum(int wrappedId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<GelbooruTag> Gelbooru_GetAllTags()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Gelbooru_GetPostsByTag(int tagId)
        {
            throw new NotImplementedException();
        }

        public void CreateSchema(string schemaName)
        {
            throw new NotImplementedException();
        }

        public void MoveAndRenameTable(string oldSchemaName, string oldTableName, string schemaName, string newTableName)
        {
            throw new NotImplementedException();
        }

        public Media[] findBrokenBandcampImports()
        {
            throw new NotImplementedException();
        }

        public Media[] FindAutofixableMetafiles()
        {
            throw new NotImplementedException();
        }

        public void Sync_AlterTable(string tableName, DatabaseColumn missingColumn)
        {
            throw new NotImplementedException();
        }

        public void RemoveMedia(Media currentMedia)
        {
            throw new NotImplementedException();
        }

        public StartupFailReason CheckLicenseStatus(string contextLicenseKey)
        {
            throw new NotImplementedException();
        }

        public void ActivateLicense(string contextLicenseKey)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<AttachmentType> GetAllMediaAttachmentTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Attachment> GetAllMediaAttachments(Media currentMedia)
        {
            throw new NotImplementedException();
        }

        public void UpdateAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public void InsertAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }

        public void DeleteAttachment(Attachment attachment)
        {
            throw new NotImplementedException();
        }
    }
}
