using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.DeleteFAQs;

public sealed class DeleteFAQsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<DeleteFAQsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteFAQsCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch FAQs

        var faqs = await db.FAQs
            .Where(x => request.Ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var notFoundIds = request.Ids
            .Except(faqs.Select(x => x.Id))
            .ToList();

        if (notFoundIds.Count > 0)
            return Result<bool>.Failure(localizer["SomeFAQsNotFound"]);

        #endregion

        #region Delete & Save

        db.FAQs.RemoveRange(faqs);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        return Result<bool>.Success(true, localizer["FAQsDeletedSuccessfully"]);
    }
}