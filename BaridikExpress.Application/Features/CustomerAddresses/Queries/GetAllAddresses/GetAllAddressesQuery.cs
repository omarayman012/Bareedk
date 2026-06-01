using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.CustomerAddresses.DTOs;

namespace BaridikExpress.Application.Features.CustomerAddresses.Queries.GetAllAddresses;

public class GetAllAddressesQuery : BaseFilter, IRequest<Result<PaginatedList<GetAllAddressesDto>>>
{
    public string? Search { get; set; }
}
