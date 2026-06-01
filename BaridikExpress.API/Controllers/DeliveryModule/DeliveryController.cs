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
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
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

  
    }
}
