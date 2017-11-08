using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace cry
{
    public static class CoinSnapshot
    {
        /// <summary>
        /// Cryptocurrency Markets according to Volume traded within last 24 hours
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<KeyValuePair<string, double>>> GetCryptoCurrencyMarkets(string fsym, string tsym)
        {
            var url = "https://www.cryptocompare.com/api/data/coinsnapshot/?fsym=" + fsym + "&tsym=" + tsym;
            var client = new HttpClient();
            var result = await client.GetStringAsync(url);
            var obj = JObject.Parse(result);

            var d = obj["Data"]["Exchanges"];
            var market = d
                .ToDictionary(i => (string)i["MARKET"], i => Math.Round(double.Parse((string)i["VOLUME24HOUR"]), 2))
                .OrderByDescending(m => m.Value)
                .Take(10);

            return market;
        }
    }
}
