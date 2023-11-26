using Crayon.Application.Dtos;
using Crayon.Application.Exceptions;
using Crayon.Application.Interfaces;

using FluentValidation;

using MediatR;
using Microsoft.AspNetCore.Identity;

using System.Net;

namespace Crayon.Application.Features.Users.Login
{
    public sealed class LoginCommand : IRequest<UserDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }

    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
        }
    }

    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;

        public LoginCommandHandler(
            UserManager<Domain.Entities.User> userManager,
            SignInManager<Domain.Entities.User> signInManager,
            IJwtGenerator jwtGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new RestException(HttpStatusCode.BadRequest, "Verify Your Email & Password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _jwtGenerator.CreateToken(user),
                };
            }

            throw new RestException(HttpStatusCode.Unauthorized);
        }
    }
}
