using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Licensing;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.DexcomHistory.Entity;
using moe.yo3explorer.azusa.Dumping.Entity;
using moe.yo3explorer.azusa.Gelbooru.Entity;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.MyFigureCollection.Entity;
using moe.yo3explorer.azusa.Notebook.Entity;
using moe.yo3explorer.azusa.PsxDatacenter.Entity;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;
using moe.yo3explorer.azusa.VgmDb.Entity;
using moe.yo3explorer.azusa.VnDb.Entity;
using moe.yo3explorer.azusa.VocaDB.Entity;
using moe.yo3explorer.azusa.WarWalking.Entity;

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
            string licenseFileName = context.ReadIniKey("rest", "licenseFile", null);
            if (licenseFileName == null)
            {
                errorState = RestDriverErrorState.LicenseUnspecified;
                return;
            }
            FileInfo fi = new FileInfo(licenseFileName);
            if (!fi.Exists)
            {
                errorState = RestDriverErrorState.LicenseFileDoesNotExist;
                return;
            }
            byte[] buffer = File.ReadAllBytes(fi.FullName);
            byte[] compressed = HashLib.HashFactory.Crypto.SHA3.CreateBlueMidnightWish224().ComputeBytes(buffer).GetBytes();
            license = BitConverter.ToString(compressed);

            webClient = new WebClient();
            webClient.BaseAddress = url;
            webClient.Encoding = Encoding.UTF8;
            webClient.Headers.Add("User-Agent", "AzusaERP 1.0");
            webClient.Headers.Add("Azusa-Machine-Name", Environment.MachineName);
            webClient.Headers.Add("Azusa-Username", Environment.UserName);
            webClient.Headers.Add("Azusa-User-Domain-Name", Environment.UserDomainName);
            webClient.Headers.Add("Azusa-License", license);
            webClient.Headers.Add("Authorization", "Azusa-License");
        }
        

        private AzusaContext context;
        private RestDriverErrorState errorState;
        private string license;
        private WebClient webClient;

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_InsertVersion(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public int? SedgeTree_GetLatestVersion()
        {
            throw new NotImplementedException();
        }

        public byte[] SedgeTree_GetDataByVersion(int version)
        {
            throw new NotImplementedException();
        }

        public bool SedgeTree_TestForPhoto(string toString)
        {
            throw new NotImplementedException();
        }

        public bool WarWalking_IsTourKnown(long hash)
        {
            throw new NotImplementedException();
        }

        public int WarWalking_InsertTourAndReturnId(long hash, int recordStart, string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tour> WarWalking_GetAllTours()
        {
            throw new NotImplementedException();
        }

        public bool WarWalking_IsAccessPointKnown(string bssid)
        {
            throw new NotImplementedException();
        }

        public Discovery WarWalking_GetByBssid(string bssid)
        {
            throw new NotImplementedException();
        }

        public void WarWalking_UpdateDiscovery(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public void WarWalking_AddAccessPoint(Discovery discovery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Discovery> WarWalking_GetDiscoveriesByTour(Tour tour)
        {
            throw new NotImplementedException();
        }

        public byte[] SedgeTree_GetPhotoByPerson(Person person)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_UpdatePhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_ErasePhoto(string personId)
        {
            throw new NotImplementedException();
        }

        public void SedgeTree_InsertPhoto(byte[] data, string personId)
        {
            throw new NotImplementedException();
        }

        public bool ConnectionIsValid()
        {
            throw new NotImplementedException();
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

        public void Statistics_Insert(DateTime today, int totalProducts, int totalMedia, int missingCover, int missingGraph,
            int undumped, int missingScreenshots)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shop> GetAllShops()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Shelf> GetAllShelves()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf)
        {
            throw new NotImplementedException();
        }

        public int CreateProductAndReturnId(Shelf shelf, string name)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<MediaType> GetMediaTypes()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public IEnumerable<DateTime> Dexcom_GetDates()
        {
            throw new NotImplementedException();
        }

        public bool Dexcom_InsertTimestamp(DexTimelineEntry entry)
        {
            throw new NotImplementedException();
        }

        public bool Dexcom_TestForTimestamp(DateTime theDate, DateTime theTime)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DexTimelineEntry> Dexcom_GetTimelineEntries(DateTime day)
        {
            throw new NotImplementedException();
        }

        public int MailArchive_GetLatestMessageId()
        {
            throw new NotImplementedException();
        }

        public void MailArchive_StoreMessage(Mail mail)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public Mail MailArchive_GetSpecificMessage(int uid)
        {
            throw new NotImplementedException();
        }

        public bool MailArchive_TestForFolder(long folderId)
        {
            throw new NotImplementedException();
        }

        public void MailArchive_InsertFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public int MailArchive_CountItemsInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public long MailArchive_GetHighestMessageUTimeInFolder(Folder folder)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ManualDataEntity> Dexcom_GetAllManualGlucoseValues()
        {
            throw new NotImplementedException();
        }

        public bool Dexcom_ManualGlucoseValueTestForTimestamp(DateTime dt)
        {
            throw new NotImplementedException();
        }

        public void Dexcom_ManualGlucoseValueStore(DateTime timestamp, short value, string unit)
        {
            throw new NotImplementedException();
        }

        public void Dexcom_ManualGlucoseValueUpdate(int id, byte be, byte novorapid, byte levemir, string note)
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public bool TransactionSupported { get; }
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

        public DbCommand Sync_GetWriteCommand(string tableName, List<DatabaseColumn> columns)
        {
            throw new NotImplementedException();
        }

        public void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage)
        {
            throw new NotImplementedException();
        }

        public LicenseState CheckLicenseStatus(byte[] uid)
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

        public IEnumerable<Note> Notebook_GetAllNotes()
        {
            throw new NotImplementedException();
        }

        public Note Notebook_CreateNote(string name, bool isCategory, int? parent)
        {
            throw new NotImplementedException();
        }

        public string Notebook_GetRichText(int noteId)
        {
            throw new NotImplementedException();
        }

        public void Notebook_UpdateNote(int currentNoteId, string text)
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

        public DexTimelineEntry Dexcom_GetLatestGlucoseEntry()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DexTimelineEntry> Dexcom_GetGlucoseEntriesAfter(DateTime scope)
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

        public bool IsAllowedSyncSource()
        {
            throw new NotImplementedException();
        }

        public bool IsAllowedSyncTarget()
        {
            throw new NotImplementedException();
        }

        public object GetConnectionObject()
        {
            throw new NotImplementedException();
        }

        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public void InsertDiscArchivatorDisc(long discid, string path, string name)
        {
            throw new NotImplementedException();
        }

        public DiscStatus GetDiscArchivatorDisc(long discid)
        {
            throw new NotImplementedException();
        }

        public void SetDiscArchivatorProperty(long discid, DiscStatusProperty property, bool value)
        {
            throw new NotImplementedException();
        }

        public void SetDiscArchivatorAzusaLink(long discid, int mediumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DiscStatus> GetDiscArchivatorEntries()
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
    }
}
