namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateCity;

public class UpdateCityCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public Guid GovernmentId { get; set; }
    public Guid? CountryId { get; set; } 
}