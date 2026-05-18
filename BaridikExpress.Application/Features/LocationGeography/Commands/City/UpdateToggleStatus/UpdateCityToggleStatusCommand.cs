namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateToggleStatus;

public class UpdateCityToggleStatusCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
