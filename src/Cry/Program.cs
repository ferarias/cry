using System;
using System.Threading.Tasks;
using Deedle;
using System.Linq;
using System.Collections.Generic;

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
            const int maxExchanges = 20;

            var exchanges = await CoinSnapshot.GetCryptoCurrencyMarkets(fsym, tsym);

            var exchangeCloseByDate = Frame.CreateEmpty<DateTimeOffset, string>();
            foreach (var exchange in exchanges.Where(x => x.Value > 100))
            {
                Console.Write($"{exchange}...");

                var exchangeInfo = await HistoDay.GetExchangeData(fsym, tsym, exchange.Key);
                if (exchangeInfo.RowCount > 1)
                {
                    var filtered = exchangeInfo
                        .Where(x => x.Key.AddMonths(6) > DateTimeOffset.Now.Date)
                        .GetColumn<float>("close");

                    exchangeCloseByDate.AddColumn(exchange.Key, filtered);
                    Console.WriteLine("downloaded");

                }
            }

            exchangeCloseByDate.Print();

            var columnNames = exchangeCloseByDate.ColumnKeys.ToList();
            var spread = new List<SpreadType>();
            for (int i = 0; i < exchangeCloseByDate.ColumnCount; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (i != j)
                    {
                        var columnA = exchangeCloseByDate.GetColumnAt<float>(i);
                        var columnB = exchangeCloseByDate.GetColumnAt<float>(j);
                        var x = columnA.Values;
                        var y = columnB.Values;

                        if (columnA.Keys.Count() == x.Count() && columnB.Keys.Count() == y.Count())
                        {
                            var diff = x.Zip(y, (a, b) => Math.Abs(a - b));
                            var average = diff.Average();
                            var sumOfSquaresOfDifferences = diff.Select(val => (val - average) * (val - average)).Sum();
                            var sd = Math.Sqrt(sumOfSquaresOfDifferences / diff.Count());
                            var item = new SpreadType { Coin1 = columnNames[i], Coin2 = columnNames[j], Mean = average, StdDev = sd };
                            spread.Add(item);
                        }
                        else
                        {
                            ;
                        }
                    }
                }
            }

            Console.WriteLine("{0,-12}{1,-12}{2,-8}{3,-8}", "Coin1", "Coin2", "Mean", "Std Dev");
            foreach (var item in spread)
            {
                Console.WriteLine("{0,-12}{1,-12}{2,-8:N3}{3,-8:N3}", item.Coin1, item.Coin2, item.Mean, item.StdDev);
            }

        }

        private struct SpreadType
        {
            public string Coin1;
            public string Coin2;
            public double Mean;
            public double StdDev;
        }
    }


}
