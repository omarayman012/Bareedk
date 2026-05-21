using BaridikExpress.Application.Features.LocationGeography.Dto.Village;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetAll;

public class GetAllVillageQuery
    : IRequest<Result<PaginatedList<VillageDto>>>
{
    public string? Name { get; set; }

    public Guid? CityId { get; set; }

    public Guid? GovernmentId { get; set; }

    public Guid? CountryId { get; set; }

    public bool? IsActive { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}