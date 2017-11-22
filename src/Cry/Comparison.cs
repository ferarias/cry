using System;
using System.Collections.Generic;
using System.Linq;

namespace Cry
{
    public class Comparison : List<ComparisonRow>
    {
        public Comparison(IEnumerable<ComparisonRow> data)
        {
            AddRange(data);
        }

        public Comparison(IDictionary<DateTimeOffset, double> series1, IDictionary<DateTimeOffset, double> series2)
        {
            AddRange(from series1Row in series1
                     join series2Row in series2 on series1Row.Key equals series2Row.Key
                     select new ComparisonRow { Date = series1Row.Key, Exchange1Value = series1Row.Value, Exchange2Value = series2Row.Value });

        }

        public string FiatCurrency { get; set; }
        public string CryptoCurrency { get; set; }

        public string Market1 { get; set; }
        public string Market2 { get; set; }
    }
}
