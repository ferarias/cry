using System;

namespace CryCompareApi
{
    public partial class ApiConnector
    {
        public class ApiResponse<T>
        {
            public ResponseType Response { get; set; }
            public Uri BaseImageUrl { get; set; }
            public Uri BaseLinkUrl { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
            public int Type { get; set; }

        }

    }
}