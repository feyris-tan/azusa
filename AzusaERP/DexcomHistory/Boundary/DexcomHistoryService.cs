using System;
using System.Collections.Generic;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.dex;

namespace moe.yo3explorer.azusa.DexcomHistory.Boundary
{
    static class DexcomHistoryService
    {
        public static IEnumerable<DateTime> GetDates()
        {
            return AzusaContext.GetInstance().DatabaseDriver.Dexcom_GetDates();
        }

        public static void Import(AzusaDexTimeline timeline)
        {
            AzusaContext context = AzusaContext.GetInstance();
            IDatabaseDriver databaseDriver = context.DatabaseDriver;
            int addedStamps = 0;

            foreach(DexTimelineEntry entry in timeline.Data)
            {
                context.MainForm.SetStatusBar(String.Format("Verarbeite Zeitstempel {0}", entry.Timestamp));
                if (!TestForTimestamp(entry.Timestamp))
                {
                    bool result = databaseDriver.Dexcom_InsertTimestamp(entry);
                    addedStamps++;
                }
            }
            
            context.MainForm.SetStatusBar(String.Format("Fertig. {0} neue Zeitstempel hinzugefügt.", addedStamps));
        }
        
        public static bool TestForTimestamp(DateTime dt)
        {
            return AzusaContext.GetInstance().DatabaseDriver.Dexcom_TestForTimestamp(dt, dt);
        }

        public static IEnumerable<DexTimelineEntry> GetDexTimelineEntries(DateTime day)
        {
            return AzusaContext.GetInstance().DatabaseDriver.Dexcom_GetTimelineEntries(day);
        }
    }
}
