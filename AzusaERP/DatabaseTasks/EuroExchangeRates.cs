using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using libeuroexchange;
using libeuroexchange.Model;

namespace moe.yo3explorer.azusa.DatabaseTasks
{
    class EuroExchangeRates : IPostConnectionTask
    {
        public void ExecutePostConnectionTask()
        {
            AzusaContext context = AzusaContext.GetInstance();
            context.Splash.SetLabel("Frage €-Umrechungskurse ab...");
            if (!context.DatabaseDriver.CanUpdateExchangeRates)
                return;

            AzusifiedCube cube = context.DatabaseDriver.GetLatestEuroExchangeRates();
            if (cube == null)
            {
                cube = new AzusifiedCube();
                cube.DateAdded = DateTime.MinValue;
            }

            cube.DateAdded = cube.DateAdded.Date;
            if (DateTime.Today > cube.DateAdded)
            {
                EcbClient ecbClient = EcbClient.GetInstance();
                Cube ecbCube = ecbClient.DownloadCube();
                cube = ecbClient.AzusifyCube(ecbCube);
                context.DatabaseDriver.InsertEuroExchangeRate(cube);
            }
        }
    }
}
