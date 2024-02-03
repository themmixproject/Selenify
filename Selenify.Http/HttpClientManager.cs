namespace Selenify.Http
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
            RequestErrorHandler errorHandler = new RequestErrorHandler();
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
