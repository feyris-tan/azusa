using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
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

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    public interface IDatabaseDriver : IDisposable
    {
        void SedgeTree_InsertVersion(byte[] buffer);
        Nullable<int> SedgeTree_GetLatestVersion();
        byte[] SedgeTree_GetDataByVersion(int version);
        bool SedgeTree_TestForPhoto(string toString);

        bool WarWalking_IsTourKnown(long hash);
        int WarWalking_InsertTourAndReturnId(long hash, int recordStart, string name);
        IEnumerable<Tour> WarWalking_GetAllTours();
        bool WarWalking_IsAccessPointKnown(string bssid);
        Discovery WarWalking_GetByBssid(string bssid);
        void WarWalking_UpdateDiscovery(Discovery discovery);
        void WarWalking_AddAccessPoint(Discovery discovery);
        IEnumerable<Discovery> WarWalking_GetDiscoveriesByTour(Tour tour);
        byte[] SedgeTree_GetPhotoByPerson(Person person);
        void SedgeTree_UpdatePhoto(byte[] data, string personId);
        void SedgeTree_ErasePhoto(string personId);
        void SedgeTree_InsertPhoto(byte[] data, string personId);
        bool ConnectionIsValid();
        List<string> GetAllPublicTableNames();
        List<string> GetAllTableNames();
        List<string> GetAllSchemas();
        bool Statistics_TestForDate(DateTime today);
        int Statistics_GetTotalProducts();
        int Statistics_GetTotalMedia();
        int Statistics_GetTotalMissingCovers();
        int Statistics_GetTotalMissingGraphData();
        int Statistics_GetTotalUndumpedMedia();
        int Statistics_GetTotalMissingScreenshots();
        void Statistics_Insert(DateTime today, int totalProducts, int totalMedia, int missingCover, int missingGraph, int undumped, int missingScreenshots);
        IEnumerable<Shop> GetAllShops();
        IEnumerable<Shelf> GetAllShelves();
        IEnumerable<ProductInShelf> GetProductsInShelf(Shelf shelf);
        int CreateProductAndReturnId(Shelf shelf, string name);
        Product GetProductById(int id);
        void UpdateProduct(Product product);
        void SetCover(Product product);
        void SetScreenshot(Product product);
        IEnumerable<Platform> GetAllPlatforms();
        IEnumerable<MediaType> GetMediaTypes();
        IEnumerable<MediaInProduct> GetMediaByProduct(Product prod);
        Media GetMediaById(int o);
        void UpdateMedia(Media media);
        int CreateMediaAndReturnId(int productId, string name);
        IEnumerable<Country> GetAllCountries();
        IEnumerable<DateTime> Dexcom_GetDates();
        bool Dexcom_InsertTimestamp(DexTimelineEntry entry);
        bool Dexcom_TestForTimestamp(DateTime theDate, DateTime theTime);
        IEnumerable<DexTimelineEntry> Dexcom_GetTimelineEntries(DateTime day);
        int MailArchive_GetLatestMessageId();
        void MailArchive_StoreMessage(Mail mail);
        bool MailArchive_TestForMessage(int uid);
        Mail MailArchive_GetSpecificMessage(int uid);
        bool MailArchive_TestForFolder(long folderId);
        void MailArchive_InsertFolder(Folder folder);
        int MailArchive_CountItemsInFolder(Folder folder);
        long MailArchive_GetHighestMessageUTimeInFolder(Folder folder);
        IEnumerable<ManualDataEntity> Dexcom_GetAllManualGlucoseValues();
        bool Dexcom_ManualGlucoseValueTestForTimestamp(DateTime dt);
        void Dexcom_ManualGlucoseValueStore(DateTime timestamp, short value, string unit);
        void Dexcom_ManualGlucoseValueUpdate(int id, byte be, byte novorapid, byte levemir, string note);
        void BeginTransaction();
        bool TransactionSupported { get; }
        void EndTransaction(bool sucessful);
        List<DatabaseColumn> Sync_DefineTable(string tableName);
        bool Sync_DoesTableExist(string tableName);
        void Sync_CreateTable(string tableName, List<DatabaseColumn> columns);
        DateTime? Sync_GetLastSyncDateForTable(string tableName);
        DbDataReader Sync_GetSyncReader(string tableName, DateTime? latestSynced);
        DbCommand Sync_GetWriteCommand(string tableName, List<DatabaseColumn> columns);
        void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage);
        LicenseState CheckLicenseStatus(byte[] uid);
        DateTime? Sync_GetLatestUpdateForTable(string tableName);
        DbDataReader Sync_GetUpdateSyncReader(string tableName, DateTime? latestUpdate);
        void Sync_CopyUpdatesFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage, Queue<object> leftovers);

        IEnumerable<Note> Notebook_GetAllNotes();
        Note Notebook_CreateNote(string name, bool isCategory, int? parent);
        string Notebook_GetRichText(int noteId);
        void Notebook_UpdateNote(int currentNoteId, string text);

        IEnumerable<int> Vgmdb_FindAlbumsByTrackMask(string text);
        IEnumerable<int> Vgmdb_FindAlbumsByArbituraryProducts(string text);
        IEnumerable<int> Vgmdb_FindAlbumsByAlbumTitle(string text);
        AlbumListEntry Vgmdb_FindAlbumForList(int id);
        Bitmap Vgmdb_GetAlbumCover(int entryId);
        IEnumerable<string> Vgmdb_FindProductNamesByAlbumId(int entryId);
        IEnumerable<string> Vgmdb_FindArtistNamesByAlbumId(int entryId);
        IEnumerable<int> Vgmdb_FindArtistIdsByName(string escaped);
        IEnumerable<int> Vgmdb_FindAlbumIdsByArtistId(int possibleArtist);
        IEnumerable<Image> FindCoversByAlbumId(int entryId);
        IEnumerable<int> Vgmdb_FindAlbumsBySkuPart(string startswith);
        IEnumerable<Tuple<string, int, int, string, int>> Vgmdb_FindTrackDataByAlbum(int entryId);
        IEnumerable<string> Vgmdb_FindLabelNamesByAlbumId(int entryId);
        IEnumerable<string> Vgmdb_FindRelatedAlbums(int albumId);
        string Vgmdb_GetReleaseEvent(int albumId);
        IEnumerable<string> Vgmdb_FindReprints(int albumId);
        IEnumerable<Uri> Vgmdb_GetWebsites(int albumId);

        IEnumerable<PsxDatacenterPreview> PsxDc_Search(string textBox);
        PsxDatacenterGame PsxDc_GetSpecificGame(int previewId);
        IEnumerable<byte[]> PsxDc_GetScreenshots(int previewId);

        IEnumerable<SqlIndex> GetSqlIndexes();
        void CreateIndex(SqlIndex index);

        void ForgetFilesystemContents(int currentMediaId);
        void AddFilesystemInfo(FilesystemMetadataEntity dirEntity);
        IEnumerable<FilesystemMetadataEntity> GetFilesystemMetadata(int currentMediaId, bool dirs);

        IEnumerable<VndbSearchResult> Vndb_Search(string searchquery);
        IEnumerable<VndbVnResult> Vndb_GetVnsByRelease(int searchResultRid);
        VndbRelease Vndb_GetReleaseById(int releaseResultRid);
        VndbVn Vndb_GetVnById(int vnResultVnid);

        DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query);

        IEnumerable<Figure> MyFigureCollection_Search(string query);
        Image MyFigureCollection_GetPhoto(int wrappedFigureId);

        IEnumerable<VocadbSearchResult> VocaDb_Search(string text);
        Image Vocadb_GetAlbumCover(int id);
        List<string> VocaDb_FindAlbumNamesBySongNames(string text);
        IEnumerable<VocadbTrackEntry> VocaDb_GetTracksByAlbum(int wrappedId);

        IEnumerable<GelbooruTag> Gelbooru_GetAllTags();
        IEnumerable<int> Gelbooru_GetPostsByTag(int tagId);
        DexTimelineEntry Dexcom_GetLatestGlucoseEntry();
        IEnumerable<DexTimelineEntry> Dexcom_GetGlucoseEntriesAfter(DateTime scope);
        void CreateSchema(string schemaName);
        void MoveAndRenameTable(string oldSchemaName, string oldTableName, string schemaName, string newTableName);

        bool IsAllowedSyncSource();
        bool IsAllowedSyncTarget();
        object GetConnectionObject();
        string GetConnectionString();

        void InsertDiscArchivatorDisc(long discid, string path, string name);
        DiscStatus GetDiscArchivatorDisc(long discid);
        void SetDiscArchivatorProperty(long discid, DiscStatusProperty property, bool value);
        void SetDiscArchivatorAzusaLink(long discid, int mediumId);
        IEnumerable<DiscStatus> GetDiscArchivatorEntries();
    }
}
