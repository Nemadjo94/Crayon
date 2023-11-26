using Crayon.Application.Dtos;
using Crayon.Application.Features.Customers.GetAccountsForCustomer;

using Microsoft.AspNetCore.Mvc;

namespace Crayon.API.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet("{id}/accounts")]
        public async Task<IEnumerable<AccountDto>> GetAccountsForCustomer(int id)
            => await Mediator.Send(new GetAccountsForCustomerCommand(id));
    }
}
