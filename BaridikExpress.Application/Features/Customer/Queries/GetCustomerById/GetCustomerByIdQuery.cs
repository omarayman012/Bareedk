using BaridikExpress.Application.Features.Customer.Dtos;

namespace BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;

public record GetCustomerByIdQuery(Guid Id)
     : IRequest<Result<CustomerDetailsResponse>>;
