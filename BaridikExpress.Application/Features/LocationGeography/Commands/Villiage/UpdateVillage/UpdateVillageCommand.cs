namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateVillage;

public class UpdateVillageCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid? CityId { get; set; }
    public Guid? GovernmentId { get; set; }
    public Guid? CountryId { get; set; }
}