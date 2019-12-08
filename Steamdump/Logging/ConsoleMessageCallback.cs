using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ionic.Zip;

namespace Steamdump.Logging
{
    internal class ConsoleMessageCallback : ISteamdumpMessageCallback
    {
        private ulong buff;
        private int entryNo;

        public void SendMessage(string message, params object[] args)
        {
            Console.WriteLine(message, args);
        }

        public void SetProgress(ZipProgressEventArgs e)
        {
            switch (e.EventType)
            {
                case ZipProgressEventType.Saving_AfterWriteEntry:
                case ZipProgressEventType.Extracting_AfterExtractEntry:
                    entryNo++;
                    break;
                case ZipProgressEventType.Saving_Started:
                case ZipProgressEventType.Saving_BeforeWriteEntry:
                case ZipProgressEventType.Saving_AfterSaveTempArchive:
                case ZipProgressEventType.Saving_BeforeRenameTempArchive:
                case ZipProgressEventType.Saving_AfterRenameTempArchive:
                case ZipProgressEventType.Saving_Completed:
                case ZipProgressEventType.Extracting_BeforeExtractAll:
                case ZipProgressEventType.Extracting_BeforeExtractEntry:
                case ZipProgressEventType.Extracting_AfterExtractAll:
                    break;
                default:
                    if (buff++ % 100 != 0)
                        return;
                    long progress = e.BytesTransferred * 100 / e.TotalBytesToTransfer;
                    
                    Console.WriteLine("Saving {0} ({1}%) ({2}/{3})", e.CurrentEntry.FileName, progress,entryNo,e.EntriesTotal);
                    break;
            }
        }
    }
}
