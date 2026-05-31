using BaridikExpress.Application.Features.SystemManagement.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetAllFAQs;

public sealed class GetAllFAQsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetAllFAQsQuery, Result<PaginatedList<FAQResponse>>>
{
    #region Handle

    public async Task<Result<PaginatedList<FAQResponse>>> Handle(
        GetAllFAQsQuery request,
        CancellationToken cancellationToken)
    {
        #region Build Query

        var query = db.FAQs.AsNoTracking().AsQueryable();

        #endregion

        #region Filters

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.QuestionAr.Contains(request.Name) ||
                x.QuestionEn.Contains(request.Name));

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);

        #endregion

        #region Projection

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new FAQResponse(
                x.Id,
                x.QuestionAr,
                x.QuestionEn,
                x.AnswerAr,
                x.AnswerEn,
                x.CreatedBy != null ? x.CreatedBy.FullName : null,
                x.CreatedAt,
                x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                x.UpdatedAt));

        #endregion

        #region Paginate

        var result = await PaginatedList<FAQResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        #endregion

        return Result<PaginatedList<FAQResponse>>.Success(result);
    }

    #endregion
}