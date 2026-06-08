namespace BaridikExpress.Application.Features.ClientAddresses.Commands.DeleteAddresses
{
    public record DeleteAddressesCommand(
        List<Guid> Ids
    ) : IRequest<Result<bool>>;
}
