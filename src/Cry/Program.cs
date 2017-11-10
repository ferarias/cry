using System;
using System.Threading.Tasks;
using Deedle;
using System.Collections.Generic;
using System.Collections;

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

            var markets = await CoinSnapshot.GetCryptoCurrencyMarkets(fsym, tsym);
            var exchangeCloseByDate = await GetExchangeCloseByDate(fsym, tsym, markets);
            exchangeCloseByDate.Print();
        }

        private static async Task<Frame<DateTimeOffset, string>> GetExchangeCloseByDate(string fsym, string tsym, IEnumerable<KeyValuePair<string, double>> markets)
        {
            var closeByExchange = Frame.CreateEmpty<DateTimeOffset, string>();
            foreach (var market in markets)
            {
                System.Console.Write($"{market}...");
                var ohlc = await HistoDay.FetchCryptoOhlcByExchange(fsym, tsym, market.Key);
                var filtered = ohlc.Where(x => x.Key.AddMonths(6) > DateTimeOffset.Now).GetColumn<float>("close");
                System.Console.WriteLine("downloaded");
                closeByExchange.AddColumn(market.Key, filtered);
            }
            return closeByExchange;
        }
    }
}
