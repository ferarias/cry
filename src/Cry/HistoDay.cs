using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Deedle;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

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
        public static async Task<Frame<DateTimeOffset, string>> GetExchangeData(string fsym, string tsym, string exchange)
        {
            try
            {
                var url = "https://min-api.cryptocompare.com/data/histoday?fsym=" + fsym
                          + "&tsym=" + tsym
                          + "&limit=2000"
                          + "&e=" + exchange;
                var client = new HttpClient();
                var result = await client.GetStringAsync(url);
                var dic = JObject.Parse(result);
                if ((string)dic["Response"] == "Error") throw new Exception((string)dic["Message"]);

                var ci = new CultureInfo("en-US");
                var rows = Enumerable.Range(0, 2000).Select(i => new HistoDayData
                {
                    TimeStamp = DateTimeOffset.FromUnixTimeSeconds((long)dic["Data"][i]["time"]).Date,
                    OpenValue = double.Parse((string)dic["Data"][i]["open"], ci.NumberFormat),
                    HighValue = double.Parse((string)dic["Data"][i]["high"], ci.NumberFormat),
                    LowValue = double.Parse((string)dic["Data"][i]["low"], ci.NumberFormat),
                    CloseValue = double.Parse((string)dic["Data"][i]["close"], ci.NumberFormat)
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
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't get data for exchange " + exchange);
                return Frame.CreateEmpty<DateTimeOffset, string>();
            }

            
        }


        internal struct HistoDayData
        {
            public DateTimeOffset TimeStamp;
            public double OpenValue;
            public double HighValue;
            public double LowValue;
            public double CloseValue;
        }
    }
}