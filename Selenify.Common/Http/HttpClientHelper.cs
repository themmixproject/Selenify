using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Http
{
    static class HttpClientHelper
    {
        public static HttpClient CreateClientWithCookieContainer(CookieContainer cookieContainer)
        {
            HttpClientHandler handler = new HttpClientHandler{
                CookieContainer = cookieContainer
            };

            return new HttpClient(handler);
        }
    }
}
