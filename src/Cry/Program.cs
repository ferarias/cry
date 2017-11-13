using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace cry
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }


        private static async Task MainAsync()
        {
            // define a pair
            const string fsym = "ETH";
            const string tsym = "USD";
            const int maxMarkets = 10;

            // Get best markets to operate with
            var markets = await CoinSnapshot.GetCryptoCurrencyMarkets(fsym, tsym);

            var closePriceTimeSeriesByExchange = new List<KeyValuePair<string, IEnumerable<KeyValuePair<DateTimeOffset, double>>>>();
            foreach (var market in markets)
            {
                Console.Write($"{market}...");
                var ohlc = await HistoDay.FetchCryptoOhlcByExchange(fsym, tsym, market.Key, 2000);
                var f = from c in ohlc
                        where c.Value.TimeStamp.AddMonths(6) > DateTimeOffset.Now
                        select new KeyValuePair<DateTimeOffset, double>(c.Key, c.Value.CloseValue);
                closePriceTimeSeriesByExchange.Add(new KeyValuePair<string, IEnumerable<KeyValuePair<DateTimeOffset, double>>>(market.Key, f));
                Console.WriteLine($"downloaded ({closePriceTimeSeriesByExchange.Count} of {maxMarkets})");
                if (closePriceTimeSeriesByExchange.Count == maxMarkets) break;
            }

            var list = new List<(string MarketA, string MarketB, double Average, double StdDev)>();

            for (var i = 0; i < closePriceTimeSeriesByExchange.Count; i++)
            {
                for (var j = i + 1; j < closePriceTimeSeriesByExchange.Count; j++)
                {
                    var marketA = closePriceTimeSeriesByExchange[i];
                    var marketB = closePriceTimeSeriesByExchange[j];
                    Console.WriteLine($"{marketA.Key} {marketB.Key}");

                    var timeSeriesA = marketA.Value;
                    var timeSeriesB = marketB.Value;

                    // spread is the difference of the price between the two markets
                    var spread = (from a in timeSeriesA
                        join b in timeSeriesB
                            on a.Key equals b.Key
                        select b.Value - a.Value).ToList();

                    // new we calculate the average and standard deviation
                    var count = spread.Count;
                    var avg = spread.Average();
                    //Perform the Sum of (value-avg)^2
                    var sum = spread.Sum(d => Math.Pow(d - avg, 2));
                    var sd = Math.Sqrt((sum) / count - 1);

                    list.Add((marketA.Key, marketB.Key, avg, sd));
                }
            }

            foreach (var tuple in list)
            {
                Console.WriteLine("{0:-12d} {1:-12d} {2:-12d} {3:-12d}", tuple.MarketA, tuple.MarketB, tuple.Average, tuple.StdDev);
            }

        }

    }
}
