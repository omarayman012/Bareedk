using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record ResendConfirmationEmailCommand(string Email)
      : IRequest<Result<bool>>;
}
