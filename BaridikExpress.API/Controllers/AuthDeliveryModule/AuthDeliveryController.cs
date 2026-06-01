using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.AuthDeliveryModule
{
    [ApiController]
    [Route("api/v1/delivery/[controller]")]
    [ApiExplorerSettings(GroupName = "delivery-v1")]
    public class AuthDeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthDeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("RegisterDriver")]
        public async Task<IActionResult> RegisterDriver([FromForm] RegisterDeliveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }

        [Authorize ("SuperAdmin")]
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
        [Authorize("SuperAdmin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
             [FromQuery] string? search = null,
             [FromQuery] bool? isApproved = null,
             [FromQuery] DateTime? approvedFrom = null,
             [FromQuery] DateTime? approvedTo = null,
             [FromQuery] string? country = null,
             [FromQuery] string? gov = null,
             [FromQuery] string? city = null,
             [FromQuery] string? village = null,
             [FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 10)
        {
            var query = new GetAllDeliveriesQuery
            {
                Search = search,
                IsApproved = isApproved,
                ApprovedFrom = approvedFrom,
                ApprovedTo = approvedTo,

                Country = country,
                Gov = gov,
                City = city,
                Village = village,

                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [Authorize("SuperAdmin")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetDeliveryByIdQuery
                {
                    Id = id
                });

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
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
        public async Task<IActionResult> VerifyEmailOtp(
          [FromBody] VerifyEmailOtpCommand command)
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
        public async Task<IActionResult> VerifyPhoneOtp(
          [FromBody] VerifyPhoneOtpCommand command)
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
        [HttpPost("resend-phone-otp")]
        public async Task<IActionResult> ResendPhoneOtp([FromBody] ResendPhoneOtpCommand command)
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
    }
}
