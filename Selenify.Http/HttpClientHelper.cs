using System.Net;

namespace Selenify.Http
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
