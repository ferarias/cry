using System;
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
        public static async Task<Series<int, float>> FetchCryptoOhlcByExchange(string fsym, string tsym, string exchange)
        {
            var cols = new[] {"date", "timestamp", "open", "high", "low", "close"};
            var lst = new[] {"time", "open", "high", "low", "close"};
            var timestampToday = DateTimeOffset.Now.ToUnixTimeSeconds();
            var currTimestamp = timestampToday;

            var data = Frame.CreateEmpty<int, string>();
            var df0 = Frame.CreateEmpty<int, string>();
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
                        Index = i,
                        TimeStamp = (long) dic["Data"][i]["time"],
                        OpenValue = (long) dic["Data"][i]["open"],
                        HighValue = (long) dic["Data"][i]["high"],
                        LowValue = (long) dic["Data"][i]["low"],
                        CloseValue = (long) dic["Data"][i]["close"]
                    })
                    .Where(o => o.OpenValue + o.HighValue + o.LowValue + o.CloseValue > 0)
                    .Select(v =>
                    {
                        // Build each row using series builder & return 
                        // KeyValue representing row key with row data
                        var sb = new SeriesBuilder<string>
                        {
                            {"time", DateTimeOffset
                                .FromUnixTimeSeconds(v.TimeStamp)
                                .ToString("yyyy-M-d")},
                            {"open", v.OpenValue},
                            {"high", v.HighValue},
                            {"low", v.LowValue},
                            {"close", v.CloseValue}
                        };
                        return KeyValue.Create(v.Index, sb.Series);
                    });

                // Turn sequence of row information into data frame
                var df = Frame.FromRows(rows);

                currTimestamp = (int) df["time", 0];

                if (j == 0)

                    df0 = df.Clone();
                else
                    data = df.Join(df0, JoinKind.Outer);
            }
            return data.GetRowsAs<float>();
        }

        internal struct HistoDayData
        {
            public int Index;
            public long TimeStamp;
            public long OpenValue;
            public long HighValue;
            public long LowValue;
            public long CloseValue;
        }
    }
}