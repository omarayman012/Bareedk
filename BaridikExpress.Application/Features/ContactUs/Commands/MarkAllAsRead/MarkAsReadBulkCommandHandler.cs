namespace BaridikExpress.Application.Features.ContactUs.Commands.MarkAllAsRead;

public sealed class MarkAsReadBulkCommandHandler(
  IApplicationDbContext db,
  IStringLocalizer localizer)
  : IRequestHandler<MarkAsReadBulkCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        MarkAsReadBulkCommand request,
        CancellationToken cancellationToken)
    {
        var contactUsList = await db.ContactUs
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (contactUsList.Count == 0)
            return Result<bool>.Failure(localizer["ContactUsNotFound"], 404);

        foreach (var contactUs in contactUsList)
            contactUs.MarkAsRead();

        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["MarkedAsReadSuccessfully"]);
    }
}
