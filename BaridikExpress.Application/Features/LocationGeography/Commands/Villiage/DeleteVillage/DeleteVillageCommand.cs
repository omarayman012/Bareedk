namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.DeleteVillage;

public class DeleteVillageCommand:IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = [];

}
