using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Http
{
    public static class HttpClientHelper
    {
        public static readonly HttpClient Client = new HttpClient();

        public static async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            HttpResponseMessage response = await Client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public static HttpResponseMessage Get(string requestUri)
        {
            HttpResponseMessage response = Client.GetAsync(requestUri).Result;
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
