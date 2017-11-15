using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Cry
{
    internal class Program
    {
        // some constants...
        private const int MaxItemsToRetrieveExchangeData = 2000;
        private static readonly string BasePath = AppDomain.CurrentDomain.BaseDirectory;

        private const int MaxMarkets = 15;
        private const double ClosePriceThreshold = 100;

        private static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }


        private static async Task MainAsync()
        {
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

                var timeSeries = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market.Key, MaxItemsToRetrieveExchangeData);
                if (timeSeries.Any())
                    closePriceTimeSeriesByExchange.Add(market.Key, timeSeries);

                Console.WriteLine($"downloaded ({closePriceTimeSeriesByExchange.Count} of {MaxMarkets})");

                if (closePriceTimeSeriesByExchange.Count == MaxMarkets) break;
            }
            PlotClosePrices(closePriceTimeSeriesByExchange, "Markets");

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

            var market1TimeSeries = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market1, MaxItemsToRetrieveExchangeData);
            var market2TimeSeries = await HistoDay.GetExchangeCloseTimeSeries(fsym, tsym, market2, MaxItemsToRetrieveExchangeData);
            var comparison = from series1Row in market1TimeSeries
                             join series2Row in market2TimeSeries on series1Row.Key equals series2Row.Key
                             select new Comparison { Date = series1Row.Key, Coin1 = series1Row.Value, Coin2 = series2Row.Value };
            PlotMarketPairComparison(market1, market1TimeSeries, market2, market2TimeSeries, $"{market1} vs {market2}");

            Console.WriteLine("--------Date ----Market A ----Market B");
            foreach (var item in comparison)
            {
                Console.WriteLine("{0,12} {1,8:#.###} {2,8:#.###}", item.Date, item.Coin1, item.Coin2);
            }

            StatArb.BackTesting(comparison);

        }

        

        private static void PlotMarketPairComparison(
            string market1,
            IDictionary<DateTimeOffset, double> series1,
            string market2,
            IDictionary<DateTimeOffset, double> series2,
            string title)

        {
            var model = new PlotModel
            {
                Title = title,
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

            model.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Maximum = Axis.ToDouble(DateTime.Now),
                StringFormat = "MMMM"
            });

            using (var stream = File.Create(Path.Combine(BasePath, $"{title}.pdf")))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);
            }
        }


        private static void PlotClosePrices(
            Dictionary<string, IDictionary<DateTimeOffset, double>> closePriceTimeSeriesByExchange,
        string title)
        {
            var model = new PlotModel
            {
                Title = title,
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.BottomCenter,
                LegendOrientation = LegendOrientation.Horizontal,
                LegendBorderThickness = 0
            };

            foreach (var priceTimeSeries in closePriceTimeSeriesByExchange)
            {
                var cs = new LineSeries() { Title = priceTimeSeries.Key, StrokeThickness = 0.2 };
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


            using (var stream = File.Create(Path.Combine(BasePath, $"{title}.pdf")))
            {
                var pdfExporter = new PdfExporter { Width = 600, Height = 400 };
                pdfExporter.Export(model, stream);
            }
        }
    }
}