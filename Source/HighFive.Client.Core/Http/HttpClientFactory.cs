using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Client.Core.Http
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler();

            if (handler.SupportsAutomaticDecompression)
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            var httpClient = new HttpClient(handler);

            return httpClient;
        }
    }
}
