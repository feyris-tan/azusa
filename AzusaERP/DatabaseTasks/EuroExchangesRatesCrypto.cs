using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoCompare;

namespace moe.yo3explorer.azusa.DatabaseTasks
{
    class EuroExchangesRatesCrypto : IPostConnectionTask
    {
        public void ExecutePostConnectionTask()
        {
            AzusaContext context = AzusaContext.GetInstance();
            if (!context.DatabaseDriver.CanUpdateExchangeRates)
                return;

            DateTime lastUpdate = context.DatabaseDriver.GetLatestCryptoExchangeRateUpdateDate();
            
            if (lastUpdate.Date >= DateTime.Today)
                return;

            string apiKey = context.ReadIniKey("cryptocompare", "apikey", null);
            if (string.IsNullOrEmpty(apiKey))
                return;

            context.Splash.SetLabel("Frage Krypto-Umrechungskurse ab...");

            CryptoCompareClient cryptoCompareClient = CryptoCompareClient.Instance;
            cryptoCompareClient.SetApiKey(apiKey);
            Task<PriceSingleResponse> task = cryptoCompareClient.Prices.SingleSymbolPriceAsync("EUR",new []{"BTC","LTC","DOGE","XCH","ETH"});
            task.Wait();
            PriceSingleResponse euro = task.Result;
            if (euro.Count == 0)
                return;

            CryptoExchangeRates result = new CryptoExchangeRates();
            result.btc = Convert.ToDouble(euro["BTC"]);
            result.ltc = Convert.ToDouble(euro["LTC"]);
            result.doge = Convert.ToDouble(euro["DOGE"]);
            result.xch = Convert.ToDouble(euro["XCH"]);
            result.eth = Convert.ToDouble(euro["ETH"]);
            context.DatabaseDriver.InsertCryptoExchangeRate(result);
        }
    }

    public struct CryptoExchangeRates
    {
        public double btc, ltc, doge, xch, eth;
    }
}
