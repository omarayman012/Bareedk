using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Domain.Entities.SystemManagment;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.CreateFAQ;

public sealed class CreateFAQCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<CreateFAQCommand, Result<FAQResponse>>
{
    #region Handle
    public async Task<Result<FAQResponse>> Handle(
        CreateFAQCommand request,
        CancellationToken cancellationToken)
    {
        #region Create & Save
        var (questionAr, questionEn) = NormalizeHelper.Normalize(request.QuestionAr, request.QuestionEn);
        var (answerAr, answerEn) = NormalizeHelper.Normalize(request.AnswerAr, request.AnswerEn );

        var faq = FAQ.Create(
            questionAr,
            questionEn,
            answerAr,
            answerEn);

        await db.FAQs.AddAsync(faq, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
        #endregion

        #region Reload CreatedBy
        await db.Entry(faq)
            .Reference(x => x.CreatedBy)
            .LoadAsync(cancellationToken);
        #endregion

        return Result<FAQResponse>.Success(
            MapToResponse(faq),
            localizer["FAQCreatedSuccessfully"],
            201);
    }
    #endregion
    private static FAQResponse MapToResponse(FAQ f) =>
        new(f.Id,
            new LocalizeLang { AR = f.QuestionAr, EN = f.QuestionEn },
            new LocalizeLang { AR = f.AnswerAr, EN = f.AnswerEn },
            f.IsActive,
            f.CreatedBy?.FullName,
            f.CreatedAt,
            f.UpdatedBy?.FullName,
            f.UpdatedAt);
}