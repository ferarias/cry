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
        private const string BaseUrl = "https://www.cryptocompare.com/api/data/coinsnapshot";

        /// <summary>
        /// Cryptocurrency Markets according to Volume traded within last 24 hours
        /// </summary>
        /// <returns></returns>
        public static async Task<IEnumerable<KeyValuePair<string, double>>> GetCryptoCurrencyMarkets(
            string fsym,
            string tsym)
        {
			var ci = new CultureInfo("en-US");
            var url = $"{BaseUrl}/?fsym={fsym}&tsym={tsym}";
            int retries = 3;

            while (retries > 0)
            {
                retries--;
                using (var client = new HttpClient())
				{
	                var result = await client.GetStringAsync(url);
	                var parsedResult = JObject.Parse(result);
	                if ((string)parsedResult["Result"] == "Error") continue;

	                
                return (from m in parsedResult["Data"]["Exchanges"]
                        select new KeyValuePair<string, double>(
                            (string) m["MARKET"],
                            Math.Round(double.Parse((string) m["VOLUME24HOUR"], ci.NumberFormat), 2))
                    )
                    .OrderByDescending(m => m.Value);

				}
            }
            return new Dictionary<string, double>();
        }
    }
}
