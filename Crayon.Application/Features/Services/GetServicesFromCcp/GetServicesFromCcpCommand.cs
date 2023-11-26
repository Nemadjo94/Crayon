using Crayon.Application.Dtos;
using Crayon.Application.Interfaces;

using MediatR;

namespace Crayon.Application.Features.Services.GetServicesFromCcp
{
    public sealed class GetServicesFromCcpCommand : IRequest<IEnumerable<ServiceDto>>
    {
    }

    public sealed class GetServicesFromCcpCommandHandler : IRequestHandler<GetServicesFromCcpCommand, IEnumerable<ServiceDto>>
    {
        private readonly ICcpApiService _ccpApiService;

        public GetServicesFromCcpCommandHandler(ICcpApiService ccpApiService)
        {
            _ccpApiService = ccpApiService;
        }

        public async Task<IEnumerable<ServiceDto>> Handle(GetServicesFromCcpCommand request, CancellationToken cancellationToken)
        {
            var services = (await _ccpApiService.GetCloudServicesAsync())
                .Select(x => new ServiceDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                });

            return services;
        }
    }
}
