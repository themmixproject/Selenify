using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Http
{
    public static class HttpClientManager
    {
        private static HttpClient? _client;
        public static HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    _client = CreateNewClient();
                }

                return _client;
            }
        }

        private static HttpClient CreateNewClient()
        {
            RequestErrorHandler errorHandler = new Selenify.Common.Http.RequestErrorHandler();
            return new HttpClient(errorHandler);
        }

        public static void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
            GC.SuppressFinalize(Client);
        }
    }
}
