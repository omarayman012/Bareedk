namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.DeleteCountry;

public class DeleteCountryCommand:IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = [];
}
