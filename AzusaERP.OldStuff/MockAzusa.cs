using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO.Migrations;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.DexcomHistory.Entity;
using moe.yo3explorer.azusa.Notebook.Entity;
using moe.yo3explorer.azusa.OfflineReaders.Gelbooru.Entity;
using moe.yo3explorer.azusa.OfflineReaders.MyFigureCollection.Entity;
using moe.yo3explorer.azusa.OfflineReaders.PsxDatacenter.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VgmDb.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VnDb.Entity;
using moe.yo3explorer.azusa.OfflineReaders.VocaDB.Entity;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;
using moe.yo3explorer.azusa.WarWalking.Entity;

namespace AzusaERP.OldStuff
{
    interface IAzusaModule
    {
        System.Windows.Forms.Control GetSelf();
    }

    class AzusaContext
    {
        public static AzusaContext GetInstance()
        {
            if (context == null)
                context = new AzusaContext();
            return context;
        }

        private static AzusaContext context;
        public AzusaSplash Splash { get; set; }
        public MainForm MainForm { get; set; }
        public IDatabaseDriver DatabaseDriver { get; set; }
        public Ini Ini { get; set; }
        public Random RandomNumberGenerator { get; set; }
        public Control CurrentOnScreenModule { get; set; }

        public int ReadIniKey(string gps, string forbidupload, int i)
        {
            throw new NotImplementedException();
        }
    }

    abstract class AzusaSplash : Form
    {
        public abstract string SetLabel(string message);
    }

    abstract class MainForm : Form
    {
        public abstract string SetStatusBar(string message);
    }

    interface IDatabaseDriver
    {
        DexTimelineEntry Dexcom_GetLatestGlucoseEntry();
        IEnumerable<DexTimelineEntry> Dexcom_GetGlucoseEntriesAfter(DateTime scope);
        IEnumerable<DateTime> Dexcom_GetDates();
        bool Dexcom_InsertTimestamp(DexTimelineEntry entry);
        bool Dexcom_TestForTimestamp(DateTime dt, DateTime dateTime);
        IEnumerable<DexTimelineEntry> Dexcom_GetTimelineEntries(DateTime day);
        List<Note> Notebook_GetAllNotes();
        string Notebook_GetRichText(int getCurrentNoteId);
        Note Notebook_CreateNote(string name, bool isCategory, int? nodeDatabaseId);
        void Notebook_UpdateNote(int getCurrentNoteId, string text);
        bool TransactionSupported { get; set; }
        void BeginTransaction();
        void EndTransaction(bool b);
        bool MailArchive_TestForFolder(long folderId);
        void MailArchive_InsertFolder(Folder folder);
        int MailArchive_CountItemsInFolder(Folder folder);
        long MailArchive_GetHighestMessageUTimeInFolder(Folder folder);
        bool SedgeTree_TestForPhoto(string toString);
        int? SedgeTree_GetLatestVersion();
        byte[] SedgeTree_GetDataByVersion(int version);
        void SedgeTree_InsertVersion(byte[] toArray);
        IEnumerable<ManualDataEntity> Dexcom_GetAllManualGlucoseValues();
        bool WarWalking_IsTourKnown(long hash);
        void WarWalking_AddAccessPoint(Discovery discovery);
        void SedgeTree_ErasePhoto(string toString);
        bool MailArchive_TestForMessage(int uid);
        bool WarWalking_IsAccessPointKnown(string bssid);
        Discovery WarWalking_GetByBssid(string bssid);
        void WarWalking_UpdateDiscovery(Discovery discovery);
        IEnumerable<Discovery> WarWalking_GetDiscoveriesByTour(Tour tour);
        int WarWalking_InsertTourAndReturnId(long hash, int recordStart, string name);
        IEnumerable<Tour> WarWalking_GetAllTours();
        void SedgeTree_UpdatePhoto(byte[] toArray, string toString);
        void SedgeTree_InsertPhoto(byte[] toArray, string toString);
        byte[] SedgeTree_GetPhotoByPerson(Person person);
        void MailArchive_StoreMessage(Mail mail);
        Mail MailArchive_GetSpecificMessage(int uid);
        int MailArchive_GetLatestMessageId();
        void Dexcom_ManualGlucoseValueUpdate(int id, byte be, byte novorapid, byte levemir, string note);
        void Dexcom_ManualGlucoseValueStore(DateTime dt, short value, string unit);
        bool Dexcom_ManualGlucoseValueTestForTimestamp(DateTime dt);
        IEnumerable<GelbooruTag> Gelbooru_GetAllTags();
        IEnumerable<int> Gelbooru_GetPostsByTag(int tagId);
        IEnumerable<Figure> MyFigureCollection_Search(string text);
        Image MyFigureCollection_GetPhoto(int wrappedFigureId);
        PsxDatacenterGame PsxDc_GetSpecificGame(int previewId);
        IEnumerable<byte[]> PsxDc_GetScreenshots(int previewId);
        PsxDatacenterPreview[] PsxDc_Search(string text);
        IEnumerable<int> Vgmdb_FindArtistIdsByName(string escaped);
        IEnumerable<int> Vgmdb_FindAlbumIdsByArtistId(int possibleArtist);
        IEnumerable<int> Vgmdb_FindAlbumsBySkuPart(string startswith);
        IEnumerable<int> Vgmdb_FindAlbumsByAlbumTitle(string escaped);
        IEnumerable<int> Vgmdb_FindAlbumsByArbituraryProducts(string escaped);
        IEnumerable<int> Vgmdb_FindAlbumsByTrackMask(string escaped);
        AlbumListEntry Vgmdb_FindAlbumForList(int input);
        Image Vgmdb_GetAlbumCover(int entryId);
        IEnumerable<string> Vgmdb_FindProductNamesByAlbumId(int entryId);
        IEnumerable<string> Vgmdb_FindArtistNamesByAlbumId(int entryId);
        IEnumerable<string> Vgmdb_FindRelatedAlbums(int entryId);
        string Vgmdb_GetReleaseEvent(int entryId);
        IEnumerable<string> Vgmdb_FindReprints(int entryId);
        IEnumerable<Uri> Vgmdb_GetWebsites(int entryId);
        List<Image> FindCoversByAlbumId(int entryId);
        IEnumerable<Tuple<string, int, int, string, int>> Vgmdb_FindTrackDataByAlbum(int entryId);
        IEnumerable<string> Vgmdb_FindLabelNamesByAlbumId(int entryId);
        VndbRelease Vndb_GetReleaseById(int releaseResultRid);
        VndbVn Vndb_GetVnById(int vnResultVnid);
        IEnumerable<VndbVnResult> Vndb_GetVnsByRelease(int searchResultRid);
        IEnumerable<VndbSearchResult> Vndb_Search(string searchquery);
        IEnumerable<MediaType> GetMediaTypes();
        List<string> VocaDb_FindAlbumNamesBySongNames(string text);
        Image Vocadb_GetAlbumCover(int wrappedId);
        IEnumerable<VocadbTrackEntry> VocaDb_GetTracksByAlbum(int wrappedId);
        IEnumerable<VocadbSearchResult> VocaDb_Search(string word);
        List<string> GetAllPublicTableNames();
        void CreateSchema(string schemaName);
        void MoveAndRenameTable(string @public, string oldTableName, string schemaName, string newTableName);
    }

    interface IPostConnectionTask
    {

    }

    class TextInputForm
    {
        public static string PromptPassword(string format, AzusaSplash contextSplash)
        {
            throw new NotImplementedException();
        }

        public static string Prompt(string nameDerNeuenKategorie, Form findForm)
        {
            throw new NotImplementedException();
        }
    }

    static class UnixTimeConverter
    {
        public static int ToUnixTime(this DateTime source)
        {
            return (int) (source.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime FromUnixTime(long utime) => new DateTime(1970, 1, 1).AddSeconds(utime);
    }

    static class StringExtensions
    {
        public static byte[] HexToBytes(this string hex)
        {
            hex = hex.Replace("-", "");
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
    }

    public class UnixtimeToDatetimeJsonConverter
    {
    }

    interface IGalleriaModel
    {
        Galleria Galleria { get; set; }
    }

    class Galleria : Control
    {
        public class DefaultGalleriaModel : IGalleriaModel
        {
            public Galleria Galleria { get; set; }
            public int Count { get; set; }

            public void AddRange(List<Image> images)
            {
                throw new NotImplementedException();
            }
        }

        public int CurrentImageNo { get; set; }
        public IGalleriaModel GalleriaModel { get; set; }

        public void UpdateControls()
        {
            throw new NotImplementedException();
        }
    }

    class EmptyGalleriaModel : IGalleriaModel
    {
        public Galleria Galleria { get; set; }
    }

    class MediaType
    {
        public string VnDbKey { get; set; }
        public byte[] Icon { get; set; }
    }

    class Attachment
    {
        public byte[] _Buffer;
        public bool _Complete;
        public bool _IsInDatabase;
        public int _MediaId;
        public int _TypeId;
    }

    class PostgresDriver
    {
        public IEnumerable<AttachmentMigrationCandidate> GetAttachmentMigrationCandidates()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void InsertAttachment(Attachment migrateElement)
        {
            throw new NotImplementedException();
        }

        public void EndTransaction(bool b)
        {
            throw new NotImplementedException();
        }
    }
}


namespace moe.yo3explorer.azusa
{
    internal class Properties
    {
        public class Resources
        {
            public static Image NavForward { get; set; }
            public static Image NavBack { get; set; }
            public static Image Find_VS { get; }
            public static Image internet_web_browser { get; set; }
            public static Image media_optical { get; set; }
        }
    }
}

