namespace BaridikExpress.Application.Features.CareerFields.Commands.DeleteCareerFields
{
    public record DeleteCareerFieldsCommand(
        List<Guid> Ids
    ) : IRequest<Result<bool>>;
}
