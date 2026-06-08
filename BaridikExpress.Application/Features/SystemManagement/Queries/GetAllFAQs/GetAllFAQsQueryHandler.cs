using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetAllFAQs;

public sealed class GetAllFAQsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetAllFAQsQuery, Result<PaginatedList<FAQResponse>>>
{
    public async Task<Result<PaginatedList<FAQResponse>>> Handle(
        GetAllFAQsQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.FAQs
            .AsNoTracking()
            .ApplyCommonFilters(request);

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.QuestionAr.Contains(request.Name) ||
                x.QuestionEn.Contains(request.Name));

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new FAQResponse(
                x.Id,
                new LocalizeLang { AR = x.QuestionAr, EN = x.QuestionEn },
                new LocalizeLang { AR = x.AnswerAr, EN = x.AnswerEn },
                x.IsActive,
                x.CreatedBy!.FullName,
                x.CreatedAt,
                x.UpdatedBy!.FullName,
                x.UpdatedAt));

        var result = await PaginatedList<FAQResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        return Result<PaginatedList<FAQResponse>>.Success(result);
    }
}