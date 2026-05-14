using MediatR;

namespace BaridikExpress.Application.Features.Auth.Commands.VerifyResetOtp
{
    public record VerifyResetOtpCommand(string Email, string Otp)
        : IRequest<Result<string>>;
}