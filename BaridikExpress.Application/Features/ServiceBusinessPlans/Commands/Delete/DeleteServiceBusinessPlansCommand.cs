namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Delete;

public sealed class DeleteServiceBusinessPlansCommand
: IRequest<Result<bool>>
{
    public List<Guid> Ids { get; set; } = [];
}
