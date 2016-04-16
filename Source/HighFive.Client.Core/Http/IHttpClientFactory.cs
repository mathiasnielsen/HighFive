using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HighFive.Client.Core.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}
