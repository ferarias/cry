using System;
using System.Threading.Tasks;
using CryCompareApi;
using Xunit;

namespace CryTests
{
    public class GeneralApi
    {
        [Fact]
        public async Task TestConnector()
        {
            using (var connector = new ApiConnector())
            {
                var response = await connector.GetCoinList();
                if(response.Response == ApiConnector.ResponseType.Success)
                Console.WriteLine(response.Data);

                var priceList = await connector.GetPrice("BTC", new[] { "USD", "AUD", "EUR" });
                Console.WriteLine(priceList);

                var priceMatrix = await connector.GetPriceMulti(new[] { "ETH", "DASH", "XMR" }, new[] { "USD", "AUD", "EUR" });
                Console.WriteLine(priceMatrix);

                var priceRawDisplayFull = await connector.GetPriceMultiFull(new[] { "ETH", "DASH", "XMR" }, new string[] { "USD", "AUD", "EUR" });
                Console.WriteLine(priceMatrix);


            }
            
        }

    }
}
