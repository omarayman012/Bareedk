namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.DeleteGovernment;

public class DeleteGovernmentCommand : IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = [];

}
