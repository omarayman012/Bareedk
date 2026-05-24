using BaridikExpress.Application.Features.Auth.Commands.ResetPassword;
using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Features.AuthClientModule.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.AuthClientModule
{
    [ApiController]
    [Authorize]
    [Route("api/client/[controller]")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public class AuthClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

      
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterClientCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] Guid? careerFieldId = null,
            [FromQuery] DateTime? createdFrom = null,
            [FromQuery] DateTime? createdTo = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllClientsQuery
            {
                Search = search,
                CareerFieldId = careerFieldId,
                CreatedFrom = createdFrom,
                CreatedTo = createdTo,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

      

        [HttpPost("send-email-otp")]
        public async Task<IActionResult> SendEmailOtp([FromBody] SendEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOtp([FromBody] VerifyEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resend-email-otp")]
        public async Task<IActionResult> ResendEmailOtp([FromBody] ResendEmailOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

       
        [HttpPost("send-phone-otp")]
        public async Task<IActionResult> SendPhoneOtp([FromBody] SendPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("verify-phone-otp")]
        public async Task<IActionResult> VerifyPhoneOtp([FromBody] VerifyPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("resend-phone-otp")]
        public async Task<IActionResult> ResendPhoneOtp([FromBody] ResendPhoneOtpCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

    
        [HttpPost("forgot-password-email")]
        public async Task<IActionResult> ForgotPasswordEmail([FromBody] ForgotPasswordByEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

       

        [HttpPost("forgot-password-phone")]
        public async Task<IActionResult> ForgotPasswordPhone([FromBody] ForgotPasswordByPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

     
        [HttpPost("confirm-reset-email-otp")]
        public async Task<IActionResult> ConfirmResetEmailOtp([FromBody] ConfirmResetPasswordEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

     

        [HttpPost("confirm-reset-phone-otp")]
        public async Task<IActionResult> ConfirmResetPhoneOtp([FromBody] ConfirmResetPasswordPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

      
        [HttpPost("reset-password-phone")]
        public async Task<IActionResult> ResetPasswordPhone([FromBody] ResetPasswordPhoneCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reset-password-email")]
        public async Task<IActionResult> ResetPasswordEmail([FromBody] ResetPasswordEmailCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
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
    }
}