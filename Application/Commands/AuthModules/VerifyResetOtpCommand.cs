using MediatR;

namespace BaridikExpress.Application.Commands.AuthModules
{
    public record VerifyResetOtpCommand(string Email, string Otp)
        : IRequest<Result<string>>;
}