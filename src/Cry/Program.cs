using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

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
            foreach (var market in markets.Take(5))
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
           
            PlotClosePrices(closePriceTimeSeriesByExchange);

            var list = new List<(string MarketA, string MarketB, double Average, double StdDev)>();

            for (var i = 0; i < closePriceTimeSeriesByExchange.Count; i++)
            {
                for (var j = i + 1; j < closePriceTimeSeriesByExchange.Count; j++)
                {
                    var marketA = closePriceTimeSeriesByExchange[i];
                    var marketB = closePriceTimeSeriesByExchange[j];
                    var timeSeriesA = marketA.Value;
                    var timeSeriesB = marketB.Value;

                    // spread is the difference of the price between the two markets
                    var spread = (from a in timeSeriesA
                                  join b in timeSeriesB
                                      on a.Key equals b.Key
                                  select Math.Abs(a.Value - b.Value)).ToList();


                    // new we calculate the average and standard deviation
                    var count = spread.Count;
                    var avg = spread.Average();
                    //Perform the Sum of (value-avg)^2
                    var sum = spread.Sum(d => Math.Pow(d - avg, 2));
                    var sd = Math.Sqrt(sum / count);

                    list.Add((marketA.Key, marketB.Key, avg, sd));
                }
            }

            foreach (var tuple in list)
            {
                Console.WriteLine("{0,12} {1,12}\t{2,8:#.###} {3,8:#.###}", tuple.MarketA, tuple.MarketB, tuple.Average, tuple.StdDev);
            }

        }

        private static void PlotClosePrices(List<KeyValuePair<string, IEnumerable<KeyValuePair<DateTimeOffset, double>>>> closePriceTimeSeriesByExchange)
        {
            var model = new PlotModel
            {
                Title = "Markets",
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            foreach (var priceTimeSeries in closePriceTimeSeriesByExchange)
            {
                var cs = new LineSeries() { Title = priceTimeSeries.Key, StrokeThickness = 0.2 };
                cs.Points.AddRange(priceTimeSeries.Value.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
                model.Series.Add(cs);
            }
            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Maximum = Axis.ToDouble(DateTime.Now), StringFormat = "MMMM" });

            using (var stream = File.Create(@"D:\temp\plot.pdf"))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);

            }
        }
    }
}
