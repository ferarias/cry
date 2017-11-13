using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace cry
{
    public static class HistoDay
    {
        private const string BaseUrl = "https://min-api.cryptocompare.com/data/histoday";

        /// <summary>
        ///     a function fetches a crypto OHLC price-series for fsym/tsym and stores
        ///     it in a pandas DataFrame; uses specific Exchange as provided
        ///     src: https://www.cryptocompare.com/api/
        /// </summary>
        /// <returns>The crypto ohlcby exchange.</returns>
        /// <param name="fsym">Fsym.</param>
        /// <param name="tsym">Tsym.</param>
        /// <param name="exchange">Exchange.</param>
        /// <param name="limit">Limit n of rows</param>
        public static async Task<IEnumerable<KeyValuePair<DateTimeOffset, HistoDayData>>> GetExchangeData(
        string fsym, string tsym, string exchange, int limit)
        {
            var url = $"{BaseUrl}?fsym={fsym}&tsym={tsym}&e={exchange}&limit={limit}";
            var client = new HttpClient();
            var ci = new CultureInfo("en-US");
            var result = await client.GetStringAsync(url);
            var parsedResult = JObject.Parse(result);
            if ((string)parsedResult["Response"] == "Error") throw new Exception((string)parsedResult["Message"]);
            return from i in Enumerable.Range(0, limit)
                   let h = new HistoDayData
                   {
                       TimeStamp = DateTimeOffset.FromUnixTimeSeconds((long)parsedResult["Data"][i]["time"]).Date,
                       OpenValue = float.Parse((string)parsedResult["Data"][i]["open"], ci.NumberFormat),
                       HighValue = float.Parse((string)parsedResult["Data"][i]["high"], ci.NumberFormat),
                       LowValue = float.Parse((string)parsedResult["Data"][i]["low"], ci.NumberFormat),
                       CloseValue = float.Parse((string)parsedResult["Data"][i]["close"], ci.NumberFormat)
                   }
                   where h.OpenValue + h.HighValue + h.LowValue + h.CloseValue > 0
                   select new KeyValuePair<DateTimeOffset, HistoDayData>(new DateTimeOffset(h.TimeStamp.Date), h);

        }

        public struct HistoDayData
        {
            public DateTimeOffset TimeStamp;
            public float OpenValue;
            public float HighValue;
            public float LowValue;
            public float CloseValue;
        }
    }
}