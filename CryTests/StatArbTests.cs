using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cry;
using Xunit;
using Xunit.Abstractions;

namespace CryTests
{
    public class StatArbTests
    {

        private readonly ITestOutputHelper _output;

        public StatArbTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void BacklogTest()
        {
            var ci = new CultureInfo("en-US");
            var testData = new List<ComparisonRow>();
            using (var reader = File.OpenText("testdata.csv"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(';');
                    testData.Add(new ComparisonRow
                    {
                        Date = DateTimeOffset.Parse(fields[0], ci),
                        Exchange1Value = double.Parse(fields[1]),
                        Exchange2Value = double.Parse(fields[2])
                    });
                }
            }



            var comparison = new Comparison(testData)
            {
                FiatCurrency = "USD",
                CryptoCurrency = "ETH",
                Market1 = "Exmo",
                Market2 = "Kraken"
            };

            var originalConsoleOut = Console.Out;
            using (var writer = new StringWriter())
            {
                Console.SetOut(writer);
                const double investment = 10000;
                StatArb.BackTesting(comparison, investment);

                writer.Flush();
                var log = writer.GetStringBuilder().ToString();
                _output.WriteLine(log);
            }
            Console.SetOut(originalConsoleOut);


        }
    }
}
