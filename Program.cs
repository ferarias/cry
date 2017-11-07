using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cry
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }


        private static async Task MainAsync(string[] args)
        {
            // define a pair
            const string fsym = "ETH";
            const string tsym = "USD";

            var url = "https://www.cryptocompare.com/api/data/coinsnapshot/?fsym=" + fsym + "&tsym=" + tsym;

            var client = new HttpClient();
            var result = await client.GetStringAsync(url);
            JObject obj = JObject.Parse(result);


            var market = new Dictionary<string, double>();
            var d = obj["Data"]["Exchanges"];
            foreach (var i in d)
            {
                market.Add((string)i["MARKET"], Math.Round(double.Parse((string)i["VOLUME24HOUR"]), 2));
            }

            var dic = market.ToList();
            dic.Sort((pair1, pair2) => -pair1.Value.CompareTo(pair2.Value));


            // Cryptocurrency Markets according to Volume traded within last 24 hours
            foreach (var item in dic)
            {
                System.Console.WriteLine("{0:s}\t{1:f}", item.Key, item.Value);
            }
                





        }
    }
}
