using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Commands.ToggleCustomerStatus;

public sealed class ToggleCustomerStatusCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<ToggleCustomerStatusCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        ToggleCustomerStatusCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Customer

        var customer = await db.Customers
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (customer is null)
            return Result<bool>.Failure(localizer["CustomerNotFound"], 404);

        #endregion

        #region Toggle & Save

        customer.ToggleStatus();

        await db.SaveChangesAsync(cancellationToken);

        #endregion

        var message = customer.IsActive
            ? localizer["CustomerActivatedSuccessfully"]
            : localizer["CustomerDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }

    #endregion
}