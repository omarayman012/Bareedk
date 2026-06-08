using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;

public class CreateCountryCommand : IRequest<Result<CreateCountryResponse>>
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string PhoneCode { get; set; } = string.Empty; 
}
