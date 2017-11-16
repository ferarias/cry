using System;
using System.Collections.Generic;
using System.Linq;

namespace Cry
{
    public class Comparison : List<ComparisonRow>
    {
        public Comparison(string m1, string m2, IDictionary<DateTimeOffset, double> series1, IDictionary<DateTimeOffset, double> series2)
        {
            Market1 = m1;
            Market2 = m2;
            AddRange(from series1Row in series1
                     join series2Row in series2 on series1Row.Key equals series2Row.Key
                     select new ComparisonRow { Date = series1Row.Key, Coin1 = series1Row.Value, Coin2 = series2Row.Value });

        }
        public string Market1 { get; set; }
        public string Market2 { get; set; }
    }
}
