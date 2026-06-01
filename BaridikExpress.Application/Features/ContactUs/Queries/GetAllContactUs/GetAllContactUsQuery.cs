using BaridikExpress.Application.Features.ContactUs.DTOs;

namespace BaridikExpress.Application.Features.ContactUs.Queries.GetAllContactUs;

public sealed record GetAllContactUsQuery(
    string? Name,
    bool? IsRead,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<Result<PaginatedList<ContactUsResponse>>>;