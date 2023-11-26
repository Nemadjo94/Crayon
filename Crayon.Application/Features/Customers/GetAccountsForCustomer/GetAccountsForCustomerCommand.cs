using Crayon.Application.Dtos;
using Crayon.Application.Interfaces;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Crayon.Application.Features.Customers.GetAccountsForCustomer
{
    public sealed class GetAccountsForCustomerCommand : IRequest<IEnumerable<AccountDto>>
    {
        public int CustomerId { get; set; }

        public GetAccountsForCustomerCommand(int customerId)
        {
            CustomerId = customerId;
        }
    }

    public sealed class GetAccountsForCustomerCommandValidator : AbstractValidator<GetAccountsForCustomerCommand>
    {
        private readonly IDataContext _dataContext;

        public GetAccountsForCustomerCommandValidator(IDataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.CustomerId).NotEmpty()
                .MustAsync(async (customerId, ct) => await _dataContext.Customers.AnyAsync(a => a.Id == customerId, ct))
                .WithMessage("Customer with Id: {PropertyValue} does not exist.");
        }
    }

    public sealed class GetAccountsForCustomerCommandHandler : IRequestHandler<GetAccountsForCustomerCommand, IEnumerable<AccountDto>>
    {
        private readonly IDataContext _dataContext;

        public GetAccountsForCustomerCommandHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<AccountDto>> Handle(GetAccountsForCustomerCommand request, CancellationToken cancellationToken)
        {
            var accountsForCustomer = await _dataContext.Accounts
                .Where(ac => ac.Customer.Id == request.CustomerId)
                .Select(ac => new AccountDto { Id = ac.Id, Name = ac.Name })
                .ToListAsync(cancellationToken: cancellationToken);

            return accountsForCustomer;
        }
    }
}
