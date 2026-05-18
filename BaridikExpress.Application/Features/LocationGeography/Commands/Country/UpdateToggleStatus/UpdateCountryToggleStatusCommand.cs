namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateToggleStatus;

public class UpdateCountryToggleStatusCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
