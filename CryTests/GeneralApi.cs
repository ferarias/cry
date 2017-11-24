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
            var connector = new ApiConnector();
            var response = await connector.GetAsync("api/data/coinlist");
            Console.WriteLine(response);
        }

    }
}
