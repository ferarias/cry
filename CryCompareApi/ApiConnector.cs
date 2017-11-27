using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

namespace CryCompareApi
{
    public partial class ApiConnector : IDisposable
    {
        private const string OldBaseUrl = "https://www.cryptocompare.com";
        private const string BaseUrl = "https://min-api.cryptocompare.com/";
        public const int DefaultRetries = 3;
        public const int DefaultRetryWait = 1000;
        private const string DefaultExchange = "CCCAGG";
        private readonly HttpClient _oldClient = new HttpClient();
        private readonly HttpClient _client = new HttpClient();

        public ApiConnector()
        {
            _oldClient.BaseAddress = new Uri(OldBaseUrl);
            _oldClient.DefaultRequestHeaders.Accept.Clear();
            _oldClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri(BaseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Retries = DefaultRetries;
            RetryWait = TimeSpan.FromMilliseconds(DefaultRetryWait);
        }

        public int Retries { get; set; }
        public TimeSpan RetryWait { get; set; }

        public void Dispose()
        {
            _client?.Dispose();
            _oldClient?.Dispose();
        }


        public async Task<ApiResponse<CoinList>> GetCoinList()
        {
            return await GetApiResponseAsync<CoinList>("api/data/coinlist");
        }

        public async Task<CoinPriceList> GetPrice(string fromSymbol, IEnumerable<string> toSymbols,
            string exchange = DefaultExchange, string nameOfYourApp = "", bool sign = false, bool tryConversion = true)
        {
            var requestUri = new StringBuilder($"data/price?fsym={fromSymbol}&tsyms={string.Join(',', toSymbols)}");
            if (!string.IsNullOrWhiteSpace(exchange) && exchange != DefaultExchange)
                requestUri.Append($"&e={exchange}");
            if (!string.IsNullOrWhiteSpace(nameOfYourApp))
                requestUri.Append($"&extraParams={nameOfYourApp}");
            if(sign)
                requestUri.Append("&sign=true");
            if(!tryConversion)
                requestUri.Append("&tryConversion=false");

            return await GetAsync<CoinPriceList>(requestUri.ToString());
        }

        public async Task<CoinPriceMatrix> GetPriceMulti(IEnumerable<string> fromSymbols, IEnumerable<string> toSymbols,
            string exchange = DefaultExchange, string nameOfYourApp = "", bool sign = false, bool tryConversion = true)
        {
            var requestUri = new StringBuilder($"data/pricemulti?fsyms={string.Join(',', fromSymbols)}&tsyms={string.Join(',', toSymbols)}");
            if (!string.IsNullOrWhiteSpace(exchange) && exchange != DefaultExchange)
                requestUri.Append($"&e={exchange}");
            if (!string.IsNullOrWhiteSpace(nameOfYourApp))
                requestUri.Append($"&extraParams={nameOfYourApp}");
            if(sign)
                requestUri.Append("&sign=true");
            if(!tryConversion)
                requestUri.Append("&tryConversion=false");

            return await GetAsync<CoinPriceMatrix>(requestUri.ToString());
        }

        public async Task<RawDisplayResponse> GetPriceMultiFull(IEnumerable<string> fromSymbols, IEnumerable<string> toSymbols,
            string exchange = DefaultExchange, string nameOfYourApp = "", bool sign = false, bool tryConversion = true)
        {
            var requestUri = new StringBuilder($"data/pricemultifull?fsyms={string.Join(',', fromSymbols)}&tsyms={string.Join(',', toSymbols)}");
            if (!string.IsNullOrWhiteSpace(exchange) && exchange != DefaultExchange)
                requestUri.Append($"&e={exchange}");
            if (!string.IsNullOrWhiteSpace(nameOfYourApp))
                requestUri.Append($"&extraParams={nameOfYourApp}");
            if(sign)
                requestUri.Append("&sign=true");
            if(!tryConversion)
                requestUri.Append("&tryConversion=false");

            return await GetAsync<RawDisplayResponse>(requestUri.ToString());
        }

        public async Task<T> GetAsync<T>(string requestUri)
        {
            var retries = Retries;

            while (retries > 0)
            {
                retries--;

                var result = await _client.GetAsync(requestUri);
                if (!result.IsSuccessStatusCode) continue;

                var content = await result.Content.ReadAsStringAsync();
                var parsedResult = JsonConvert.DeserializeObject<T>(content);

                return parsedResult;
            }
            return default(T);
        }

        public async Task<ApiResponse<T>> GetApiResponseAsync<T>(string requestUri)
        {
            var retries = Retries;

            while (retries > 0)
            {
                retries--;

                var result = await _oldClient.GetAsync(requestUri);
                if (!result.IsSuccessStatusCode) continue;

                var content = await result.Content.ReadAsStringAsync();
                var parsedResult = JsonConvert.DeserializeObject<ApiResponse<T>>(content);

                return parsedResult;
            }
            return null;
        }

    }
}