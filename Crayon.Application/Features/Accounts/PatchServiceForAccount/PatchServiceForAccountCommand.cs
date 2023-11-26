using Crayon.Application.Interfaces;
using Crayon.Domain.Consts;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Crayon.Application.Features.Accounts.PatchServiceForAccount
{
    public sealed class PatchServiceForAccountCommand : IRequest<Unit>
    {
        public int AccountId { get; set; }

        public int ServiceId { get; set; }

        public int? Quantity { get; set; }

        public string? Status { get; set; }

        public DateTime? EndDate { get; set; }

        public PatchServiceForAccountCommand(int accountId, int serviceId, int? quantity, string? status, DateTime? endDate)
        {            
            AccountId = accountId;
            ServiceId = serviceId;
            Quantity = quantity;
            Status = status;
            EndDate = endDate;
        }
    }

    public sealed class PatchServiceForAccountCommandValidator : AbstractValidator<PatchServiceForAccountCommand>
    {
        private readonly IDataContext _dataContext;

        public PatchServiceForAccountCommandValidator(IDataContext dataContext)
        {
            _dataContext = dataContext;

            RuleFor(x => x.AccountId).NotEmpty()
                .MustAsync(async (accountId, ct) => await _dataContext.Accounts.AnyAsync(a => a.Id == accountId, ct))
                .WithMessage("Account with Id: {PropertyValue} does not exist.");

            RuleFor(x => x.ServiceId).NotEmpty()
                .MustAsync(async (serviceId, ct) => await _dataContext.Services.AnyAsync(a => a.Id == serviceId, ct))
                .WithMessage("Service with Id: {PropertyValue} does not exist.");

            RuleFor(x => x.Quantity).GreaterThan(0).When(x => x.Quantity.HasValue);

            RuleFor(x => x.Status).Must(x => x != null && x.Equals(ServiceStatus.Cancelled, StringComparison.OrdinalIgnoreCase)).When(x => x.Status != null);

            RuleFor(x => x)
                .MustAsync(async (command, ct) =>
                {
                    var accountService = await _dataContext.AccountServices.FirstOrDefaultAsync(x =>
                        x.AccountId == command.AccountId && x.ServiceId == command.ServiceId, ct);

                    return accountService != null && !(accountService.StartDate > command.EndDate);
                })
                .WithMessage("New EndDate cannot be before the StartDate.")
                .When(x => x.EndDate != null);
        }
    }

    public sealed class PatchServiceForAccountCommandHandler : IRequestHandler<PatchServiceForAccountCommand, Unit>
    {
        private readonly IDataContext _dataContext;

        public PatchServiceForAccountCommandHandler(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Unit> Handle(PatchServiceForAccountCommand request, CancellationToken cancellationToken)
        {
            var accountService = await _dataContext.AccountServices.FirstAsync(x =>
                x.AccountId == request.AccountId && x.ServiceId == request.ServiceId, cancellationToken);

            if (request.Quantity != null)
                accountService.Quantity = (int)request.Quantity;

            if (request.Status != null)
                accountService.Status = request.Status;

            if (request.EndDate != null)
                accountService.EndDate = (DateTime)request.EndDate;

            await _dataContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
