using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.ResendConfirmEmail
{
    public record ResendConfirmationEmailCommand(string Email)
      : IRequest<Result<bool>>;
}
