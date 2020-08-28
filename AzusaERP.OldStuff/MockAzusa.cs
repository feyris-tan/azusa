using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.MailArchive.Entity;
using moe.yo3explorer.azusa.dex;
using moe.yo3explorer.azusa.DexcomHistory.Entity;
using moe.yo3explorer.azusa.Notebook.Entity;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;
using moe.yo3explorer.azusa.WarWalking.Boundary;
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
            return (int)(source.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
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
}


namespace moe.yo3explorer.azusa
{
    internal class Properties
    {
        public class Resources
        {
            public static Image NavForward { get; set; }
            public static Image NavBack { get; set; }
        }
    }
}

