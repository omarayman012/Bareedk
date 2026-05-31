using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ContactUs.Commands.DeleteContactUs;

public sealed class DeleteContactUsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteContactUsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteContactUsCommand request,
        CancellationToken cancellationToken)
    {
        var items = await db.ContactUs
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(items.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeContactUsNotFound"]);

        db.ContactUs.RemoveRange(items);
        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["ContactUsDeletedSuccessfully"]);
    }
}