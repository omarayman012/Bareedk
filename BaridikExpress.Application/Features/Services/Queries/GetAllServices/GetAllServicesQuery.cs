using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Services.DTOs;

namespace BaridikExpress.Application.Features.Services.Queries.GetAllServices;

public sealed class GetAllServicesQuery : BaseFilter, IRequest<Result<PaginatedList<ServiceResponse>>>
{
    public string? Name { get; set; }
}
