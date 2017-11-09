using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Deedle;
using Newtonsoft.Json.Linq;

namespace cry
{
    public static class HistoDay
    {
        /// <summary>
        ///     a function fetches a crypto OHLC price-series for fsym/tsym and stores
        ///     it in a pandas DataFrame; uses specific Exchange as provided
        ///     src: https://www.cryptocompare.com/api/
        /// </summary>
        /// <returns>The crypto ohlcby exchange.</returns>
        /// <param name="fsym">Fsym.</param>
        /// <param name="tsym">Tsym.</param>
        /// <param name="exchange">Exchange.</param>
        public static async Task<Frame<DateTimeOffset, string>> FetchCryptoOhlcByExchange(string fsym, string tsym, string exchange)
        {

            var url = "https://min-api.cryptocompare.com/data/histoday?fsym=" + fsym
                      + "&tsym=" + tsym
                      + "&limit=2000"
                      + "&e=" + exchange;
            var client = new HttpClient();
            var result = await client.GetStringAsync(url);
            var dic = JObject.Parse(result);

            var rows = Enumerable.Range(0, 2000).Select(i => new HistoDayData
            {
                TimeStamp = DateTimeOffset.FromUnixTimeSeconds((long)dic["Data"][i]["time"]),
                OpenValue = (float)dic["Data"][i]["open"],
                HighValue = (float)dic["Data"][i]["high"],
                LowValue = (float)dic["Data"][i]["low"],
                CloseValue = (float)dic["Data"][i]["close"]
            })
                .Where(o => o.OpenValue + o.HighValue + o.LowValue + o.CloseValue > 0)
                .Select(v =>
                {
                    // Build each row using series builder & return 
                    // KeyValue representing row key with row data
                    var sb = new SeriesBuilder<string>
                    {
                            {"time", v.TimeStamp},
                            {"open", v.OpenValue},
                            {"high", v.HighValue},
                            {"low", v.LowValue},
                            {"close", v.CloseValue}
                    };
                    return KeyValue.Create(v.TimeStamp, sb.Series);
                });

            // Turn sequence of row information into data frame
            var frameDate = Frame.FromRows(rows).IndexRows<DateTimeOffset>("time");
            //frameDate.Print();

            return frameDate;
        }

        internal struct HistoDayData
        {
            public DateTimeOffset TimeStamp;
            public float OpenValue;
            public float HighValue;
            public float LowValue;
            public float CloseValue;
        }
    }
}