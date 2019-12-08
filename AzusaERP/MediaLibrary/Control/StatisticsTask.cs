using System;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    class StatisticsTask : IPostConnectionTask
    {
        public void ExecutePostConnectionTask()
        {
            int totalProducts, totalMedia, missingCover, missingGraph, undumped, missingScreenshots;

            AzusaContext context = AzusaContext.GetInstance();
            DateTime today = DateTime.Today;

            context.Splash.SetLabel("Frage Tagesstatistik ab...");
            bool todayAlreadyDone = context.DatabaseDriver.Statistics_TestForDate(today);
            if (todayAlreadyDone)
                return;

            context.Splash.SetLabel("Frage Anzahl Produkte ab...");
            totalProducts = context.DatabaseDriver.Statistics_GetTotalProducts();

            context.Splash.SetLabel("Frage Anzahl Medien ab...");
            totalMedia = context.DatabaseDriver.Statistics_GetTotalMedia();

            context.Splash.SetLabel("Frage fehlende Coverbilder ab...");
            missingCover = context.DatabaseDriver.Statistics_GetTotalMissingCovers();
            
            context.Splash.SetLabel("Frage fehlende Graphdaten ab...");
            missingGraph = context.DatabaseDriver.Statistics_GetTotalMissingGraphData();
            
            context.Splash.SetLabel("Frage nicht gedumpte Medien ab...");
            undumped = context.DatabaseDriver.Statistics_GetTotalUndumpedMedia();

            context.Splash.SetLabel("Frage fehlende Screenshots ab...");
            missingScreenshots = context.DatabaseDriver.Statistics_GetTotalMissingScreenshots();

            context.Splash.SetLabel("Speichere Tagesstatistik...");
            context.DatabaseDriver.Statistics_Insert(today, totalProducts, totalMedia, missingCover, missingGraph, undumped, missingScreenshots);
        }
    }
}
