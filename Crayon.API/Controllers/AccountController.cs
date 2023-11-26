using Crayon.Application.Dtos;
using Crayon.Application.Features.Accounts.GetServicesForAccount;
using Crayon.Application.Features.Accounts.OrderServiceForAccount;
using Crayon.Application.Features.Accounts.PatchServiceForAccount;

using Microsoft.AspNetCore.Mvc;

namespace Crayon.API.Controllers
{
    public class AccountController : BaseController
    {
        [HttpPost("{id}/services")]
        public async Task OrderServiceForAccount(int id, [FromBody] OrderServiceDto dto)
            => await Mediator.Send(new OrderServiceForAccountCommand(id, dto.ServiceId, dto.Quantity, dto.StartDate, dto.EndDate));

        [HttpGet("{id}/services")]
        public async Task<IEnumerable<ServiceDto>> GetServicesForAccount(int id)
            => await Mediator.Send(new GetServicesForAccountCommand(id));

        [HttpPatch("{id}/services/{serviceId}")]
        public async Task PatchServiceForAccount(int id, int serviceId, [FromBody] PatchAccountServiceDto dto)
            => await Mediator.Send(new PatchServiceForAccountCommand(id, serviceId, dto.Quantity, dto.Status, dto.EndDate));
    }
}
