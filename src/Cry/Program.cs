using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cry;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace cry
{
    class Program
    {
        // some constants...
        private static string _basePath;

        const int MaxMarkets = 15;
        const double ClosePriceThreshold = 100;
        const int MaxItemsToRetrieveExchangeData = 2000;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }


        private static async Task MainAsync()
        {
            _basePath = AppDomain.CurrentDomain.BaseDirectory;

            // define a pair
            const string fsym = "ETH";
            const string tsym = "USD";

            // Get best markets to operate with
            var markets = await CoinSnapshot.GetVolume24HByMarket(fsym, tsym);

            var closePriceTimeSeriesByExchange = new Dictionary<string, IDictionary<DateTimeOffset, double>>();
            foreach (var market in markets
                .OrderByDescending(x => x.Value)
                .Where(x => x.Value > ClosePriceThreshold))
            {
                Console.Write($"{market}...");

                var dictionary = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market.Key);
                if(dictionary.Any())
                    closePriceTimeSeriesByExchange.Add(market.Key, dictionary);

                Console.WriteLine($"downloaded ({closePriceTimeSeriesByExchange.Count} of {MaxMarkets})");

                if (closePriceTimeSeriesByExchange.Count == MaxMarkets) break;
            }
            PlotClosePrices(closePriceTimeSeriesByExchange);

            var spreads = Spread.GetSpreads(closePriceTimeSeriesByExchange);

            Console.WriteLine("----Market 1 ----Market 2\t Average Std.Dev.");
            foreach (var spread in spreads)
            {
                Console.WriteLine("{0,12} {1,12}\t{2,8:#.###} {3,8:#.###}", spread.Market1, spread.Market2,
                    spread.Average, spread.StandardDeviation);
            }

            Console.Write("Market 1? ");
            var market1 = Console.ReadLine();
            Console.Write("Market 2? ");
            var market2 = Console.ReadLine();

            var dic1 = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market1);
            var dic2 = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market2);
            PlotMarketPairComparison(market1, dic1, market2, dic2, Path.Combine(_basePath, "Comparison.pdf"));


            var data = from i1 in dic1
                join i2 in dic2 on i1.Key equals i2.Key
                select new Comparison {Date = i1.Key, Coin1 = i1.Value, Coin2 = i2.Value};

            Console.WriteLine("--------Date ----Market A ----Market B");
            foreach (var item in data)
            {
                Console.WriteLine("{0,12} {1,8:#.###} {2,8:#.###}", item.Date, item.Coin1, item.Coin2);
            }
        }


        private static void PlotMarketPairComparison(
            string market1,
            IDictionary<DateTimeOffset, double> series1,
            string market2,
            IDictionary<DateTimeOffset, double> series2,
            string fileName)

        {
            var model = new PlotModel
            {
                Title = market1 + " vs " + market2,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };


            var lineSeries1 = new LineSeries() {Title = market1, StrokeThickness = 1, Color = OxyColors.Blue};
            lineSeries1.Points.AddRange(series1.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
            model.Series.Add(lineSeries1);
            var lineSeries2 = new LineSeries() {Title = market2, StrokeThickness = 1, Color = OxyColors.Red};
            lineSeries2.Points.AddRange(series2.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
            model.Series.Add(lineSeries2);

            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Maximum = Axis.ToDouble(DateTime.Now),
                StringFormat = "MMMM"
            });

            using (var stream = File.Create(fileName))
            {
                var pdfExporter = new PdfExporter {Width = 600, Height = 400};
                pdfExporter.Export(model, stream);
            }
        }


        private static void PlotClosePrices(
            Dictionary<string, IDictionary<DateTimeOffset, double>> closePriceTimeSeriesByExchange)
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
                var cs = new LineSeries() {Title = priceTimeSeries.Key, StrokeThickness = 0.2};
                cs.Points.AddRange(
                    priceTimeSeries.Value.Select(x => new DataPoint(Axis.ToDouble(x.Key.Date), x.Value)));
                model.Series.Add(cs);
            }
            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Maximum = Axis.ToDouble(DateTime.Now),
                StringFormat = "MMMM"
            });


            using (var stream = File.Create(Path.Combine(_basePath, "Markets.pdf")))
            {
                var pdfExporter = new PdfExporter {Width = 600, Height = 400};
                pdfExporter.Export(model, stream);
            }
        }
    }
}