using Crayon.Application.Dtos;
using Crayon.Application.Interfaces;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Crayon.Application.Features.Accounts.GetServicesForAccount
{
    public sealed class GetServicesForAccountCommand : IRequest<IEnumerable<ServiceDto>>
    {
        public int AccountId { get; set; }

        public GetServicesForAccountCommand(int accountId)
        {
            AccountId = accountId;
        }
    }

    public sealed class GetServicesForAccountCommandValidator : AbstractValidator<GetServicesForAccountCommand>
    {
        private readonly IDataContext _dataContext;

        public GetServicesForAccountCommandValidator(IDataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.AccountId).NotEmpty()
                .MustAsync(async (accountId, ct) => await _dataContext.Accounts.AnyAsync(a => a.Id == accountId, ct))
                .WithMessage("Account with Id: {PropertyValue} does not exist.");
        }
    }

    public sealed class GetServicesForAccountCommandHandler : IRequestHandler<GetServicesForAccountCommand, IEnumerable<ServiceDto>>
    {
        private readonly IDataContext _dataContext;

        public GetServicesForAccountCommandHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<ServiceDto>> Handle(GetServicesForAccountCommand request, CancellationToken cancellationToken)
        {
            var servicesForAccount = await _dataContext.AccountServices
                .Where(ac => ac.AccountId == request.AccountId)
                .Select(ac => new ServiceDto { Id = ac.Service.Id, Name = ac.Service.Name, Description = ac.Service.Description })
                .ToListAsync(cancellationToken: cancellationToken);

            return servicesForAccount;
        }
    }
}
