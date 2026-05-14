using BaridikExpress.Application.Features.DeliveryModule.Command;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.DeliveryModule
{
    [ApiController]
    [Route("api/deliveries")]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
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
    }
}
