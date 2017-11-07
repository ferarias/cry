using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Deedle;
using Newtonsoft.Json.Linq;

namespace cry
{
    public static class CryptoOHLC
    {

        /// <summary>
        /// a function fetches a crypto OHLC price-series for fsym/tsym and stores
        /// it in a pandas DataFrame; uses specific Exchange as provided
        /// src: https://www.cryptocompare.com/api/
        /// </summary>
        /// <returns>The crypto ohlcby exchange.</returns>
        /// <param name="fsym">Fsym.</param>
        /// <param name="tsym">Tsym.</param>
        /// <param name="exchange">Exchange.</param>
        public static async Task<Series<int, float>> FetchCryptoOHLCByExchange(string fsym, string tsym, int exchange)
        {
            var cols = new string[] { "date", "timestamp", "open", "high", "low", "close" };
            var lst = new string[] { "time", "open", "high", "low", "close" };
            var timestamp_today = DateTimeOffset.Now.ToUnixTimeSeconds();
            var curr_timestamp = timestamp_today;

            Frame<int, string> data = Frame.CreateEmpty<int, string>();
            Frame<int, string> df0 = Frame.CreateEmpty<int, string>();
            for (int j = 0; j < 2; j++)
            {
                var url = "https://min-api.cryptocompare.com/data/histoday?fsym=" + fsym
                                    + "&tsym=" + tsym
                                    + "&toTs=" + curr_timestamp
                                    + "&limit=2000"
                                    + "&e=" + exchange;
                var client = new HttpClient();
                var result = await client.GetStringAsync(url);
                JObject dic = JObject.Parse(result);

                var rows = Enumerable.Range(1, 2001).Select(i =>
                {
                    return new OPOPO
                    {
                        Index = i,
                        TimeStamp = (long)(dic["Data"][i]["time"]),
                        OpenValue = (long)dic["Data"][i]["open"],
                        HighValue = (long)dic["Data"][i]["high"],
                        LowValue = (long)dic["Data"][i]["low"],
                        CloseValue = (long)dic["Data"][i]["close"]
                    };
                }).Where(o => o.OpenValue + o.HighValue + o.LowValue + o.CloseValue > 0)
                                     .Select(v =>
                                     {
                                         // Build each row using series builder & return 
                                         // KeyValue representing row key with row data
                                         var sb = new SeriesBuilder<string>();
                                         sb.Add("time", DateTimeOffset.FromUnixTimeMilliseconds(v.TimeStamp).ToString("Y-m-d"));
                                         sb.Add("open", v.OpenValue);
                                         sb.Add("high", v.HighValue);
                                         sb.Add("low", v.LowValue);
                                         sb.Add("close", v.CloseValue);
                                         return KeyValue.Create(v.Index, sb.Series);
                                     });

                // Turn sequence of row information into data frame
                var df = Frame.FromRows(rows);

                curr_timestamp = (int)df["date", 0];

                if (j == 0)

                    df0 = df.Clone();

                else
                    data = df.Join(df0, JoinKind.Outer);
                return data.GetRowsAs<float>();
            }

        }

        internal struct OPOPO
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