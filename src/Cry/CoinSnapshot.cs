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
        private const string BaseUrl = "https://www.cryptocompare.com/api/data/coinsnapshot";

        /// <summary>
        /// Get cryptocurrency exchanges according to volume traded within last 24 hours
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<KeyValuePair<string, double>>> GetCryptoCurrencyMarkets(
            string fsym,
            string tsym)
        {
            var url = $"{BaseUrl}/?fsym={fsym}&tsym={tsym}";
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(url);
                var parsedResult = JObject.Parse(result);

                return (from m in parsedResult["Data"]["Exchanges"]
                        select new KeyValuePair<string, double>(
                            (string) m["MARKET"],
                            Math.Round(double.Parse((string) m["VOLUME24HOUR"]), 2))
                    )
                    .OrderByDescending(m => m.Value);
            }
        }
    }
}