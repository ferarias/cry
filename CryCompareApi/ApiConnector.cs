using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CryCompareApi
{
    public class ApiConnector : IDisposable
    {
        private const string BaseUrl = "https://www.cryptocompare.com";
        public const int DefaultRetries = 3;
        public const int DefaultRetryWait = 1000;

        private readonly HttpClient _client = new HttpClient();

        public ApiConnector()
        {
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
        }


        public async Task<ApiResponse> GetAsync(string requestUri)
        {
            var retries = Retries;

            while (retries > 0)
            {
                retries--;

                var result = await _client.GetAsync(requestUri);
                if (!result.IsSuccessStatusCode) continue;
                
                var content = await result.Content.ReadAsStringAsync();
                var parsedResult = JsonConvert.DeserializeObject<ApiResponse>(content);

                return parsedResult;
            }
            return null;
        }

        public class ApiResponse
        {
            public string Response { get; set; }
            public string BaseImageUrl { get; set; }
            public string BaseLinkUrl { get; set; }
            public string Message { get; set; }
            public string Data { get; set; }
            public int Type { get; set; }

        }

    }
}