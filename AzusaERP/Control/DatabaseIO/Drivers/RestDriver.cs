using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using libeuroexchange.Model;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Setup;
using moe.yo3explorer.azusa.DatabaseTasks;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
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
            string rawJson = webClient.DownloadString("/shops");
            List<Shop> list = JsonConvert.DeserializeObject<List<Shop>>(rawJson);
            return list;
        }

        public IEnumerable<Shelf> GetAllShelves()
        {
            string rawJson = webClient.DownloadString("/shelves");
            List<Shelf> list = JsonConvert.DeserializeObject<List<Shelf>>(rawJson);
            return list;
        }

        public IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf)
        {
            string rawJson = webClient.DownloadString(String.Format("/products/inshelf/{0}",shelf.Id));
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
                productInShelf.ScreenshotSize = jToken.Value<long>("ScreenshotSize");
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
            string rawJson = webClient.DownloadString(String.Format("/products/{0}", id));
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
            string rawJson = webClient.DownloadString("/platforms");
            List<Platform> list = JsonConvert.DeserializeObject<List<Platform>>(rawJson);
            return list;
        }

        public IEnumerable<MediaType> GetMediaTypes()
        {
            string rawJson = webClient.DownloadString("/mediaTypes");
            List<MediaType> list = JsonConvert.DeserializeObject<List<MediaType>>(rawJson);
            return list;
        }

        public IEnumerable<MediaInProduct> GetMediaByProduct(Product prod)
        {
            string rawJson = webClient.DownloadString("/media/inProduct/names/" + prod.Id);
            List<MediaInProductJson> tmp = JsonConvert.DeserializeObject<List<MediaInProductJson>>(rawJson);
            return tmp.ConvertAll(x => x.Transform()).ToList();
        }

        public Media GetMediaById(int o)
        {
            string rawJson = webClient.DownloadString("/media/" + o);
            Media tmp = JsonConvert.DeserializeObject<Media>(rawJson);
            return tmp;
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
            string rawJson = webClient.DownloadString("/countries");
            List<Country> list = JsonConvert.DeserializeObject<List<Country>>(rawJson);
            return list;
        }
        
        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool CanActivateLicense => false;
        public bool CanUpdateExchangeRates => false;

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
            string rawJson = webClient.DownloadString(String.Format("/fsinfo/container/{0}?dirs={1}",currentMediaId,dirs));
            List<FilesystemMetadataEntity> tmp = JsonConvert.DeserializeObject<List<FilesystemMetadataEntity>>(rawJson);
            return tmp;
        }
        
        public DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query)
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
        
        public AzusifiedCube GetLatestEuroExchangeRates()
        {
            string rawJson = webClient.DownloadString("/exchangerates/latest");
            JsonDocument jsonDocument = JsonDocument.Parse(rawJson);
            AzusifiedCube list = new AzusifiedCube();

            string messyDate = jsonDocument.RootElement.GetProperty("cubedate").GetString();
            if (messyDate.EndsWith("Z"))
            {
                messyDate = messyDate.Substring(0, messyDate.Length - 1);
                list.CubeDate = DateTime.Parse(messyDate);
                list.CubeDate += new TimeSpan(1, 0, 0, 0);

                long daUnixtime = jsonDocument.RootElement.GetProperty("dateadded").GetInt64();
                daUnixtime += 7200;
                list.DateAdded = UnixTimeConverter.FromUnixTime(daUnixtime);
            }
            else
                throw new Exception("Don't know how to parse this timezone: " + messyDate);

            list.USD = jsonDocument.RootElement.GetProperty("usd").GetDouble();
            list.JPY = jsonDocument.RootElement.GetProperty("jpy").GetDouble();
            list.GBP = jsonDocument.RootElement.GetProperty("gbp").GetDouble();

            return list;
        }

        public void InsertEuroExchangeRate(AzusifiedCube cube)
        {
            throw new NotImplementedException();
        }

        public DateTime GetLatestCryptoExchangeRateUpdateDate()
        {
            throw new NotImplementedException();
        }

        public void InsertCryptoExchangeRate(CryptoExchangeRates exchangeRates)
        {
            throw new NotImplementedException();
        }
    }
}
