namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;

public class UpdateCountryCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? NameAr { get; set; }
    public string?NameEn { get; set; }


}
