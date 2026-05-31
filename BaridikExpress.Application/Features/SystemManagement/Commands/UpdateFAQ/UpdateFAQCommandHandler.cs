using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateFAQ;

public sealed class UpdateFAQCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateFAQCommand, Result<FAQResponse>>
{
    #region Handle

    public async Task<Result<FAQResponse>> Handle(
        UpdateFAQCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch

        var faq = await db.FAQs
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (faq is null)
            return Result<FAQResponse>.Failure(localizer["FAQNotFound"], 404);

        #endregion

        #region Update & Save

        faq.Update(request.QuestionAr, request.QuestionEn, request.AnswerAr, request.AnswerEn);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        #region Reload UpdatedBy

        await db.Entry(faq)
            .Reference(x => x.UpdatedBy)
            .LoadAsync(cancellationToken);

        #endregion

        return Result<FAQResponse>.Success(
            MapToResponse(faq),
            localizer["FAQUpdatedSuccessfully"]);
    }

    #endregion

    private static FAQResponse MapToResponse(FAQ f) =>
        new(f.Id,
            f.QuestionAr, f.QuestionEn,
            f.AnswerAr, f.AnswerEn,
            f.CreatedBy?.FullName,
            f.CreatedAt,
            f.UpdatedBy?.FullName,
            f.UpdatedAt);
}