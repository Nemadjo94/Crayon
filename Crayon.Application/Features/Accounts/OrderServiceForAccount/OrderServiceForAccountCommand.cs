using Crayon.Application.Exceptions;
using Crayon.Application.Interfaces;
using Crayon.Domain.Consts;
using Crayon.Domain.Entities;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Crayon.Application.Features.Accounts.OrderServiceForAccount
{
    public sealed class OrderServiceForAccountCommand : IRequest<Unit>
    {
        public int AccountId { get; set; }

        public int ServiceId { get; set; }

        public int Quantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public OrderServiceForAccountCommand(int accountId, int serviceId, int quantity, DateTime startDate, DateTime endDate)
        {
            AccountId = accountId;
            ServiceId = serviceId;
            Quantity = quantity;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public sealed class OrderServiceForAccountCommandValidator : AbstractValidator<OrderServiceForAccountCommand>
    {
        private readonly IDataContext _dataContext;

        public OrderServiceForAccountCommandValidator(IDataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.AccountId).NotEmpty()
                .MustAsync(async (accountId, ct) => await _dataContext.Accounts.AnyAsync(a => a.Id == accountId, ct))
                .WithMessage("Account with Id: {PropertyValue} does not exist.");

            RuleFor(x => x)
                .MustAsync(async (command, ct) =>
                    !(await _dataContext.AccountServices.AnyAsync(ac => ac.ServiceId == command.ServiceId && ac.AccountId == command.AccountId, ct)))
                .WithMessage("Service already assigned to account.");


            RuleFor(x => x.ServiceId).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);

            RuleFor(x => x.StartDate).NotEmpty().LessThan(x => x.EndDate);

            RuleFor(x => x.EndDate).NotEmpty().GreaterThan(x => x.StartDate);
        }
    }

    public sealed class OrderServiceForAccountCommandHandler : IRequestHandler<OrderServiceForAccountCommand, Unit>
    {
        private readonly IDataContext _dataContext;
        private readonly ICcpApiService _ccpApiService;

        public OrderServiceForAccountCommandHandler(IDataContext dataContext, ICcpApiService ccpApiService)
        {
            _dataContext = dataContext;
            _ccpApiService = ccpApiService;
        }

        public async Task<Unit> Handle(OrderServiceForAccountCommand request, CancellationToken cancellationToken)
        {
            var service = await _dataContext.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken: cancellationToken);

            if (service is null)
            {
                var ccpService = (await _ccpApiService.GetCloudServicesAsync())
                    .FirstOrDefault(x => x.Id == request.ServiceId) 
                    ?? throw new RestException(System.Net.HttpStatusCode.NotFound, $"The service with Id: {request.ServiceId} does not exist.");

                service = new Service
                {
                    Id = ccpService.Id,
                    Name = ccpService.Name,
                    Description = ccpService.Description
                };

                _dataContext.Services.Add(service);
            }

            var accountService = new AccountService
            {
                AccountId = request.AccountId,
                Service = service,
                Status = ServiceStatus.Active,
                Quantity =  request.Quantity,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _dataContext.AccountServices.Add(accountService);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
