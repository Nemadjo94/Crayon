using Crayon.Application.Dtos;
using Crayon.Application.Exceptions;
using Crayon.Application.Interfaces;

using FluentValidation;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System.Net;

namespace Crayon.Application.Features.Users.Register
{
    public sealed class RegisterCommand : IRequest<UserDto>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }

    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() 
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
        }
    }

    public sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IDataContext _context;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IJwtGenerator _jwtGenerator;

        public RegisterCommandHandler(
            IDataContext context,
            UserManager<Domain.Entities.User> userManager,
            IJwtGenerator jwtGenerator)
        {
            _context = context;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync(cancellationToken))
                throw new RestException(HttpStatusCode.BadRequest, "Email already exists");

            var user = new Domain.Entities.User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email.Split('@').First(),
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }

            throw new Exception("Problem creating user");
        }
    }
}
