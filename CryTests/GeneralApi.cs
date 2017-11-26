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

                var priceList = await connector.GetPrice("BTC", new string[] { "USD", "AUD", "EUR" });
                Console.WriteLine(priceList);
            }
            
        }

    }
}
