namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.ToggleStatusGovernment;

public class UpdateGovernmentToggleStatusCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
