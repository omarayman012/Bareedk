using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetAllFAQs;

public sealed class GetAllFAQsQuery : BaseFilter, IRequest<Result<PaginatedList<FAQResponse>>>
{
    public string? Name { get; set; }
}