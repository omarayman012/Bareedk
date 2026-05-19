using BaridikExpress.Application.Features.Auth.Commands.ChangePassword;
using BaridikExpress.Application.Features.Auth.Commands.ConfirmEmail;
using BaridikExpress.Application.Features.Auth.Commands.CreateAccount;
using BaridikExpress.Application.Features.Auth.Commands.ForgotPassword;
using BaridikExpress.Application.Features.Auth.Commands.Login;
using BaridikExpress.Application.Features.Auth.Commands.Logout;
using BaridikExpress.Application.Features.Auth.Commands.RefreshToken;
using BaridikExpress.Application.Features.Auth.Commands.ResendConfirmEmail;
using BaridikExpress.Application.Features.Auth.Commands.ResetPassword;
using BaridikExpress.Application.Features.Auth.Commands.ValidateToken;
using BaridikExpress.Application.Features.Auth.Commands.VerifyResetOtp;
using BaridikExpress.Application.Features.Users.Queries.GetCurrentUserProfile;
using BaridikExpress.Application.Features.Users.Queries.GetMyPermissions;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.AuthModules
{
    [ApiController]
    [Route("api/v1/auth/[controller]")]
    [ApiExplorerSettings(GroupName = "auth-v1")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Login/")]
        public async Task<ActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("logout")]
        [HasPermission(Permissions.AuthManage)]
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("verify-reset-otp")]
        public async Task<IActionResult> VerifyResetOtp([FromBody] VerifyResetOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("change-password")]
        [HasPermission(Permissions.AuthManage)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("me/permissions")]
        [Authorize]
        public async Task<IActionResult> GetMyPermissions()
        {
            var result = await _mediator.Send(new GetMyPermissionsQuery());
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetCurrentUserProfileQuery());
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("validate-token")]
        public async Task<IActionResult> ValidateToken()
        {
            var result = await _mediator.Send(new ValidateTokenCommand());
            return StatusCode(result.StatusCode, result);
        }


    }
}
