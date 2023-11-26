using Crayon.Application.Dtos;
using Crayon.Application.Features.Services.GetServicesFromCcp;

using Microsoft.AspNetCore.Mvc;

namespace Crayon.API.Controllers
{
    public class ServiceController : BaseController
    {
        [HttpGet]
        public async Task<IEnumerable<ServiceDto>> GetServices()
            => await Mediator.Send(new GetServicesFromCcpCommand());
    }
}
