using System;
using System.Threading.Tasks;
using Deedle;

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
            const string exchange = "CCCAGG";

            var markets = await CoinSnapshot.GetCryptoCurrencyMarkets(fsym, tsym);
            foreach (var item in markets)
            {
                Console.WriteLine("{0:s}\t{1:f}", item.Key, item.Value);
            }


            //var ohlc = await HistoDay.FetchCryptoOhlcByExchange(fsym, tsym, exchange);

            foreach (var market in markets)
            {
                System.Console.Write($"{market}...");
                var df = await HistoDay.FetchCryptoOhlcByExchange(fsym, tsym, market.Key);
                var filtered = df.Where(x => x.Key.AddMonths(6) > DateTimeOffset.Now).GetColumn<float>("close");
                System.Console.WriteLine("downloaded");
            }

        }
    }
}
