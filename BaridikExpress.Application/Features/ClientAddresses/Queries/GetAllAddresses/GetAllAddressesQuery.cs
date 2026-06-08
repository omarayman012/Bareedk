using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.ClientAddresses.DTOs;

namespace BaridikExpress.Application.Features.ClientAddresses.Queries.GetAllAddresses;

public class GetAllAddressesQuery : BaseFilter, IRequest<Result<PaginatedList<GetAllAddressesDto>>>
{
    public string? Search { get; set; }
}
