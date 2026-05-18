using BaridikExpress.Application.Features.LocationGeography.Dto.Village;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetById;

public class GetVillageByIdQuery
    : IRequest<Result<VillageDto>>
{
    public Guid Id { get; set; }
}