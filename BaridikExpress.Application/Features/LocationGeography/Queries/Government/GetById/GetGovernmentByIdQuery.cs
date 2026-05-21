using BaridikExpress.Application.Features.LocationGeography.Dto.Government;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetById;

public class GetGovernmentByIdQuery
    : IRequest<Result<GovernmentDto>>
{
    public Guid Id { get; set; }
}