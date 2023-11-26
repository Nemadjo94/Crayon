using Crayon.Application.Dtos;
using Crayon.Application.Interfaces;

using Microsoft.Extensions.Configuration;

namespace Crayon.Infrastructure.Services
{
    public sealed class CcpApiService : ICcpApiService
    {
        private readonly IHttpService _httpService;
        private readonly string _baseUrl;

        public CcpApiService(IHttpService httpService, IConfiguration configuration)
        {
            _httpService = httpService;
            _baseUrl = configuration.GetRequiredSection("CcpApi")["BaseUrl"]?.ToString()
                ?? throw new ArgumentNullException(_baseUrl, "CcpApi:BaseUrl cannot be null.");
        }

        public async Task<IEnumerable<CloudServiceDto>> GetCloudServicesAsync()
            => await _httpService.GetAsync<IEnumerable<CloudServiceDto>>($"{_baseUrl}/CloudService");
    }
}
