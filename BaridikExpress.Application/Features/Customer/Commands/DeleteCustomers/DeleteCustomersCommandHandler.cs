using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Commands.DeleteCustomers;

public sealed class DeleteCustomersCommandHandler(
    IApplicationDbContext db,
    UserManager<User> userManager,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteCustomersCommand, Result<bool>>
{
    #region Handle

    public async Task<Result<bool>> Handle(
        DeleteCustomersCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Customers

        var customers = await db.Customers
            .Where(c => request.Ids.Contains(c.Id))
            .ToListAsync(cancellationToken);

        var foundIds = customers.Select(c => c.Id).ToHashSet();
        var notFoundIds = request.Ids.Except(foundIds).ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeCustomersNotFound"]);

        #endregion

        #region Fetch Related Entities

        var customerIds = customers.Select(c => c.Id).ToList();
        var userIds = customers.Select(c => c.UserId).ToList();

        var contacts = await db.CustomerContacts
            .Where(x => customerIds.Contains(x.CustomerId))
            .ToListAsync(cancellationToken);

        var addresses = await db.CustomerAddresses
            .Where(x => customerIds.Contains(x.CustomerId))
            .ToListAsync(cancellationToken);

        var accounts = await db.CustomerAccounts
            .Where(x => customerIds.Contains(x.CustomerId))
            .ToListAsync(cancellationToken);

        var users = await userManager.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);

        #endregion

        #region Delete & Save (Transaction)

        await using var transaction = await db.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            db.CustomerContacts.RemoveRange(contacts);
            db.CustomerAddresses.RemoveRange(addresses);
            db.CustomerAccounts.RemoveRange(accounts);
            db.Customers.RemoveRange(customers);

            await db.SaveChangesAsync(cancellationToken);

            foreach (var user in users)
                await userManager.DeleteAsync(user);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        #endregion

        return Result<bool>.Success(true, localizer["CustomersDeletedSuccessfully"]);
    }

    #endregion
}