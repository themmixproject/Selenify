using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenify.Common.Http
{
    public class RequestErrorHandler : DelegatingHandler
    {
        public RequestErrorHandler()
        {
            this.InnerHandler = new HttpClientHandler();
        }

        public RequestErrorHandler(HttpClientHandler httpClientHandler)
        {
            this.InnerHandler = httpClientHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            try
            {
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (HttpRequestException ex)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                string exceptionMessage = ex.ToString() + "\n--Response Body:\n" + responseBody + "\n";
                throw new HttpRequestException(exceptionMessage);
            }
        }
    }
}
