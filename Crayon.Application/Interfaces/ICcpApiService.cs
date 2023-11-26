using Crayon.Application.Dtos;

namespace Crayon.Application.Interfaces
{
    public interface ICcpApiService
    {
        public Task<IEnumerable<CloudServiceDto>> GetCloudServicesAsync();
    }
}