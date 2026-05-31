using BaridikExpress.Application.Features.ContactUs.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ContactUs.Queries.GetAllContactUs;

public sealed class GetAllContactUsQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllContactUsQuery, Result<PaginatedList<ContactUsResponse>>>
{
    public async Task<Result<PaginatedList<ContactUsResponse>>> Handle(
        GetAllContactUsQuery request,
        CancellationToken cancellationToken)
    {
        var query = db.ContactUs.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x => x.Name.Contains(request.Name));    
        if (request.IsRead.HasValue)
            query = query.Where(x => x.IsRead == request.IsRead.Value);

        var projected = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ContactUsResponse(
                x.Id,
                x.Name,
                x.Email,
                x.Phone,
                x.Message,
                x.IsRead,
                x.CreatedAt));

        var result = await PaginatedList<ContactUsResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        return Result<PaginatedList<ContactUsResponse>>
            .Success(result, localizer["ContactUsRetrievedSuccessfully"]);
    }
}