﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using libeuroexchange.Model;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;
using moe.yo3explorer.azusa.Control.Setup;
using moe.yo3explorer.azusa.DatabaseTasks;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.Control.DatabaseIO
{
    public interface IDatabaseDriver : IDisposable
    {
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
        void BeginTransaction();
        bool CanActivateLicense { get; }
        bool CanUpdateExchangeRates { get; }
        void EndTransaction(bool sucessful);
        List<DatabaseColumn> Sync_DefineTable(string tableName);
        bool Sync_DoesTableExist(string tableName);
        void Sync_CreateTable(string tableName, List<DatabaseColumn> columns);
        DateTime? Sync_GetLastSyncDateForTable(string tableName);
        DbDataReader Sync_GetSyncReader(string tableName, DateTime? latestSynced);
        void Sync_CopyFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage);
        DateTime? Sync_GetLatestUpdateForTable(string tableName);
        DbDataReader Sync_GetUpdateSyncReader(string tableName, DateTime? latestUpdate);
        void Sync_CopyUpdatesFrom(string tableName, List<DatabaseColumn> columns, DbDataReader syncReader, SyncLogMessageCallback onMessage, Queue<object> leftovers);
        
        IEnumerable<SqlIndex> GetSqlIndexes();
        void CreateIndex(SqlIndex index);

        void ForgetFilesystemContents(int currentMediaId);
        void AddFilesystemInfo(FilesystemMetadataEntity dirEntity);
        IEnumerable<FilesystemMetadataEntity> GetFilesystemMetadata(int currentMediaId, bool dirs);
        
        DbDataReader Sync_ArbitrarySelect(string tableName, DatabaseColumn column, object query);
        
        void CreateSchema(string schemaName);
        void MoveAndRenameTable(string oldSchemaName, string oldTableName, string schemaName, string newTableName);

        Media[] findBrokenBandcampImports();
        Media[] FindAutofixableMetafiles();
        void Sync_AlterTable(string tableName, DatabaseColumn missingColumn);
        void RemoveMedia(Media currentMedia);
        StartupFailReason CheckLicenseStatus(string contextLicenseKey);
        void ActivateLicense(string contextLicenseKey);

        IEnumerable<AttachmentType> GetAllMediaAttachmentTypes();
        IEnumerable<Attachment> GetAllMediaAttachments(Media currentMedia);
        void UpdateAttachment(Attachment attachment);
        void InsertAttachment(Attachment attachment);
        void DeleteAttachment(Attachment attachment);

        AzusifiedCube GetLatestEuroExchangeRates();
        void InsertEuroExchangeRate(AzusifiedCube cube);

        DateTime GetLatestCryptoExchangeRateUpdateDate();
        void InsertCryptoExchangeRate(CryptoExchangeRates exchangeRates);
    }
}
