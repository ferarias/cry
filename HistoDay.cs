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
        public static async Task<Series<DateTimeOffset, float>> FetchCryptoOhlcByExchange(string fsym, string tsym, string exchange)
        {
            var cols = new[] {"date", "timestamp", "open", "high", "low", "close"};
            var lst = new[] {"time", "open", "high", "low", "close"};
            var timestampToday = DateTimeOffset.Now.ToUnixTimeSeconds();
            var currTimestamp = timestampToday;

            var data = Frame.CreateEmpty<DateTimeOffset, string>();
            var df0 = Frame.CreateEmpty<DateTimeOffset, string>();
            for (var j = 0; j < 2; j++)
            {
                var url = "https://min-api.cryptocompare.com/data/histoday?fsym=" + fsym
                          + "&tsym=" + tsym
                          + "&toTs=" + currTimestamp
                          + "&limit=2000"
                          + "&e=" + exchange;
                var client = new HttpClient();
                var result = await client.GetStringAsync(url);
                var dic = JObject.Parse(result);

                var rows = Enumerable.Range(0, 2000).Select(i => new HistoDayData
                    {
                        TimeStamp = DateTimeOffset.FromUnixTimeSeconds((long) dic["Data"][i]["time"]),
                        OpenValue = (float) dic["Data"][i]["open"],
                        HighValue = (float) dic["Data"][i]["high"],
                        LowValue = (float) dic["Data"][i]["low"],
                        CloseValue = (float) dic["Data"][i]["close"]
                    })
                    .Where(o => o.OpenValue + o.HighValue + o.LowValue + o.CloseValue > 0)
                    .Select(v =>
                    {
                        // Build each row using series builder & return 
                        // KeyValue representing row key with row data
                        var sb = new SeriesBuilder<string>
                        {
                            {"time", v.TimeStamp.ToString("yyyy-MMM-dd")},
                            {"open", v.OpenValue},
                            {"high", v.HighValue},
                            {"low", v.LowValue},
                            {"close", v.CloseValue}
                        };
                        return KeyValue.Create(v.TimeStamp, sb.Series);
                    });

                // Turn sequence of row information into data frame
                var df = Frame.FromRows(rows);
                df.Print();

                var frameDate = df.IndexRows<string>("time").SortRowsByKey();
                frameDate.Print();

                var byOffs = frameDate.SelectRowKeys(kvp => DateTimeOffset.Parse(kvp.Key, null, DateTimeStyles.AssumeUniversal));

                byOffs.Print();
                // var d = frameDate.GetColumn<DateTimeOffset>("time");

                //if (j == 0)

                //    df0 = df.Clone();
                //else
                //    data = df.Join(df0, JoinKind.Outer);
            }
            return data.GetRowsAs<float>();
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