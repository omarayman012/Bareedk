using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetFAQById;

public sealed class GetFAQByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetFAQByIdQuery, Result<FAQResponse>>
{
    #region Handle
    public async Task<Result<FAQResponse>> Handle(
        GetFAQByIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = await db.FAQs
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new FAQResponse(
                x.Id,
                new LocalizeLang { AR = x.QuestionAr, EN = x.QuestionEn },
                new LocalizeLang { AR = x.AnswerAr, EN = x.AnswerEn },
                x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result<FAQResponse>.Failure(localizer["FAQNotFound"], 404);

        return Result<FAQResponse>.Success(response, localizer["FAQRetrievedSuccessfully"]);
    }
    #endregion
}