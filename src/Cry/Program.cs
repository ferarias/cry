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
            const int maxMarkets = 15;

            // Get best markets to operate with
            var markets = await CoinSnapshot.GetCryptoCurrencyMarkets(fsym, tsym);

            var closePriceTimeSeriesByExchange = new List<KeyValuePair<string, IEnumerable<KeyValuePair<DateTimeOffset, double>>>>();
            foreach (var market in markets.Where(x => x.Value > 100))
            {
                Console.Write($"{market}...");

                var exchangeInfo = await HistoDay.GetExchangeData(fsym, tsym, market.Key, 2000);
                if (exchangeInfo.Count() > 1)
                {
                    var f = from c in exchangeInfo
                            where c.Value.TimeStamp.AddMonths(6) > DateTimeOffset.Now
                            select new KeyValuePair<DateTimeOffset, double>(c.Key, c.Value.CloseValue);
                    closePriceTimeSeriesByExchange.Add(new KeyValuePair<string, IEnumerable<KeyValuePair<DateTimeOffset, double>>>(market.Key, f));
                    Console.WriteLine($"downloaded ({closePriceTimeSeriesByExchange.Count} of {maxMarkets})");
                    if (closePriceTimeSeriesByExchange.Count == maxMarkets) break;
                }
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
            var orderedList = list.OrderByDescending(o => o.Average);

            Console.WriteLine("----Market 1 ----Market 2\t Average Std.Dev.");
            foreach (var tuple in orderedList)
            {
                Console.WriteLine("{0,12} {1,12}\t{2,8:#.###} {3,8:#.###}", tuple.MarketA, tuple.MarketB, tuple.Average, tuple.StdDev);
            }

            Console.Write("Market 1?");
            var market1 = Console.ReadLine();
            Console.Write("Market 2?");
            var market2 = Console.ReadLine();

            var dic1 = new Dictionary<DateTimeOffset, float>();
            var dic2 = new Dictionary<DateTimeOffset, float>();
            var df1 = await HistoDay.GetExchangeData(fsym, tsym, market1, 2000);
            if (df1.Count() > 1)
            {
                dic1 = df1.Where(c => c.Value.TimeStamp.AddMonths(6) > DateTimeOffset.Now)
                                    .ToDictionary(c => c.Key, c => c.Value.CloseValue);
            }
            var df2 = await HistoDay.GetExchangeData(fsym, tsym, market2, 2000);
            if (df2.Count() > 1)
            {
                dic2 = df2.Where(c => c.Value.TimeStamp.AddMonths(6) > DateTimeOffset.Now)
                                    .ToDictionary(c => c.Key, c => c.Value.CloseValue);
            }

            var data = new List<(DateTimeOffset Date, float MarketA, float MarketB)>();
            foreach (var item in dic1)
            {
                if (dic2.ContainsKey(item.Key))
                {
                    data.Add((Date: item.Key, MarketA: item.Value, MarketB: dic2[item.Key]));
                }
            }
            PlotMarketPairComparison(market1, dic1, market2, dic2);
            Console.WriteLine("--------Date ----Market A ----Market B");
            foreach (var tuple in data)
            {
                Console.WriteLine("{0,12} {1,8:#.###} {2,8:#.###}", tuple.Date, tuple.MarketA, tuple.MarketB);
            }

        }

        private static void PlotMarketPairComparison(
            string market1,
            Dictionary<DateTimeOffset, float> series1,
            string market2,
            Dictionary<DateTimeOffset, float> series2)
        {
            var model = new PlotModel
            {
                Title = market1 + " vs " + market2,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };


            var lineSeries1 = new LineSeries() { Title = market1, StrokeThickness = 1, Color = OxyColors.Blue };
            lineSeries1.Points.AddRange(series1.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
            model.Series.Add(lineSeries1);
            var lineSeries2 = new LineSeries() { Title = market2, StrokeThickness = 1, Color = OxyColors.Red };
            lineSeries2.Points.AddRange(series2.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
            model.Series.Add(lineSeries2);

            model.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, Maximum = Axis.ToDouble(DateTime.Now), StringFormat = "MMMM" });

            using (var stream = File.Create(@"E:\temp\Comparison.pdf"))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);

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

            using (var stream = File.Create(@"E:\temp\Markets.pdf"))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);

            }
        }
    }
}
