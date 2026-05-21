using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Customer.DTO;

namespace BaridikExpress.Application.Features.Customer.Queries.GetAllCustomers;

public sealed class GetAllCustomersQuery : BaseFilter, IRequest<Result<PaginatedList<CustomerListItemResponse>>>
{
    public string? Name { get; set; }
    public decimal? TotalOrdersMin { get; set; }
    public decimal? TotalOrdersMax { get; set; }
    public decimal? TotalSpentMin { get; set; }
    public decimal? TotalSpentMax { get; set; }
}