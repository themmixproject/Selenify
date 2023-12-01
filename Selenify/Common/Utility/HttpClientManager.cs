using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Utility
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
                    _client = new HttpClient();
                }

                return _client;
            }
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
