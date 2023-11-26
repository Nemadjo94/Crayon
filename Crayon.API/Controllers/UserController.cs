using Crayon.Application.Dtos;
using Crayon.Application.Features.Users.Login;
using Crayon.Application.Features.Users.Register;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crayon.API.Controllers
{
    public class UserController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginCommand command)
            => await Mediator.Send(command);

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterCommand command)
            => await Mediator.Send(command);
    }
}
