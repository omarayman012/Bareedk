using BaridikExpress.Application.Features.LocationGeography.Dto.Government;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;

public class CreateGovernmentCommand:IRequest<Result<GovernmentDto>>
{
    public string NameAr { get; set; }=string.Empty;
    public string NameEn { get; set; }=string.Empty;
    public Guid CountryId { get; set; }
}
