using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace cry
{
    public static class CoinSnapshot
    {
        /// <summary>
        /// Cryptocurrency Markets according to Volume traded within last 24 hours
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<KeyValuePair<string, double>>> GetCryptoCurrencyMarkets(
            string fsym, string tsym)
        {
            var url = "https://www.cryptocompare.com/api/data/coinsnapshot/?fsym=" + fsym + "&tsym=" + tsym;

            int retries = 3;

            while (retries > 0)
            {
                retries--;
                var client = new HttpClient();
                var result = await client.GetStringAsync(url);
                var obj = JObject.Parse(result);
                if ((string)obj["Result"] == "Error") continue;

                var ci = new CultureInfo("en-US");
                var exchanges = obj["Data"]["Exchanges"];
                var market = exchanges.ToDictionary(
                    i => (string)i["MARKET"],
                    i => Math.Round(double.Parse((string)i["VOLUME24HOUR"], ci.NumberFormat), 2));

                var sortedMarkets = market
                    .OrderByDescending(m => m.Value);
                return sortedMarkets;
            }
            return new Dictionary<string, double>();
        }
    }
}
