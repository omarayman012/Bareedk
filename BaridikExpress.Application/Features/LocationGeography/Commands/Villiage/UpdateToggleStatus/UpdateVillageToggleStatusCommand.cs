namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateToggleStatus;

public class UpdateVillageToggleStatusCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
}
