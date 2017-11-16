using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Cry
{
    public static class CoinSnapshot
    {
        private const string BaseUrl = "https://www.cryptocompare.com/api/data/coinsnapshot";

        /// <summary>
        /// Cryptocurrency Markets according to Volume traded within last 24 hours
        /// </summary>
        /// <returns></returns>
        public static async Task<IDictionary<string, double>> GetVolume24HByMarket(
            string fsym,
            string tsym, 
            CultureInfo ci)
        {
            var url = $"{BaseUrl}/?fsym={fsym}&tsym={tsym}";
            int retries = 3;

            while (retries > 0)
            {
                retries--;
                using (var client = new HttpClient())
                {
                    var result = await client.GetStringAsync(url);
                    var parsedResult = JObject.Parse(result);
                    if ((string) parsedResult["Result"] == "Error") continue;

                    return parsedResult["Data"]["Exchanges"]
                        .ToDictionary(
                            m => (string) m["MARKET"],
                            m => double.Parse((string) m["VOLUME24HOUR"], ci.NumberFormat));
                }
            }
            return new Dictionary<string, double>();
        }
    }
}