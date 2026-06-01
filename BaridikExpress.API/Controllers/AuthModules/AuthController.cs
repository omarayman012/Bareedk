using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Features.AuthClientModule.Queries;
using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.AuthModules
{
    [ApiController]
    [Route("api/v1/auth/[controller]")]
    [ApiExplorerSettings(GroupName = "auth-v1")]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [AllowAnonymous]
        [HttpPost("RegisterClient")]
        public async Task<IActionResult> Register([FromBody] RegisterClientCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("send-email-otp")]
        public async Task<IActionResult> SendEmailOtp([FromBody] SendEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("resend-email-otp")]
        public async Task<IActionResult> ResendEmailOtp([FromBody] ResendEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("send-phone-otp")]
        public async Task<IActionResult> SendPhoneOtp([FromBody] SendPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("verify-phone-otp")]
        public async Task<IActionResult> VerifyPhoneOtp([FromBody] VerifyPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("resend-phone-otp")]
        public async Task<IActionResult> ResendPhoneOtp([FromBody] ResendPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password-email")]
        public async Task<IActionResult> ForgotPasswordEmail([FromBody] ForgotPasswordByEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [AllowAnonymous]
        [HttpPost("forgot-password-phone")]
        public async Task<IActionResult> ForgotPasswordPhone([FromBody] ForgotPasswordByPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("confirm-reset-email-otp")]
        public async Task<IActionResult> ConfirmResetEmailOtp([FromBody] ConfirmResetPasswordEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [AllowAnonymous]
        [HttpPost("confirm-reset-phone-otp")]
        public async Task<IActionResult> ConfirmResetPhoneOtp([FromBody] ConfirmResetPasswordPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [AllowAnonymous]
        [HttpPost("reset-password-phone")]
        public async Task<IActionResult> ResetPasswordPhone([FromBody] ResetPasswordPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("reset-password-email")]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] ResetPasswordEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("RegisterDriver")]
        public async Task<IActionResult> RegisterDriver([FromForm] RegisterDeliveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }

        [Authorize("SuperAdmin")]
        [HttpPost("CreateByAdmin")]
        public async Task<IActionResult> CreateByAdmin(
             [FromForm] CreateDeliveryByAdminCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }
        [Authorize("SuperAdmin")]
        [HttpPatch("ApproveDelivery/{id}")]
        public async Task<IActionResult> ApproveDelivery(Guid id)
        {
            var command = new ApproveDeliveryCommand
            {
                DeliveryId = id
            };

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }
      
       
   
    }
}
