using System;
using System.Collections.Generic;
using System.Linq;

namespace Cry
{
    public class Spread
    {
        public string Market1 { get; set; }
        public string Market2 { get; set; }
        public double Average { get; set; }
        public double StandardDeviation { get; set; }

        public static IEnumerable<Spread>
            GetSpreads(IDictionary<string, IDictionary<DateTimeOffset, double>> closePriceTimeSeriesByExchange)
        {
            var spreads = DictionaryHelper.GetKeyCombinations(closePriceTimeSeriesByExchange, (ass, bss) =>
                {
                    // spread is the difference of the price between the two markets
                    return (from a in ass
                            join b in bss on a.Key equals b.Key
                            select (a.Key, Value1: a.Value, Value2: b.Value))
                        .ToDictionary(x => x.Key, x => Math.Abs(x.Value1 - x.Value2));
                })
                .ToDictionary(x => (Market1: x.Key1, Market2: x.Key2), x => x.Value);

            return from marketSpread in spreads
                let count = marketSpread.Value.Count
                let avg = marketSpread.Value.Values.Average()
                let sum = marketSpread.Value.Values.Sum(d => Math.Pow(d - avg, 2))
                orderby avg
                select new Spread
                {
                    Market1 = marketSpread.Key.Market1,
                    Market2 = marketSpread.Key.Market2,
                    Average = avg,
                    StandardDeviation = Math.Sqrt(sum / count)
                };
        }
    }
}