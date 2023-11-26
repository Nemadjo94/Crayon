using Crayon.Application.Interfaces;

using System.Net.Http.Json;

namespace Crayon.Infrastructure.Services
{
    public sealed class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _clientFactory;

        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var client = _clientFactory.CreateClient();
            return await client.GetFromJsonAsync<T>(url);
        }
    }
}
