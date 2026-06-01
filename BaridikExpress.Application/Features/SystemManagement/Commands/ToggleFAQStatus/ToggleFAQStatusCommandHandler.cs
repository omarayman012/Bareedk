using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.ToggleFAQStatus;

public sealed class ToggleFAQStatusCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<ToggleFAQStatusCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        ToggleFAQStatusCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch FAQ

        var faq = await db.FAQs
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (faq is null)
            return Result<bool>.Failure(localizer["FAQNotFound"], 404);

        #endregion

        #region Toggle & Save

        faq.ToggleStatus();
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        var message = faq.IsActive
            ? localizer["FAQActivatedSuccessfully"]
            : localizer["FAQDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}