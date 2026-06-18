using BaridikExpress.Application.Features.AuthDeliveryModule.Command;
using BaridikExpress.Application.Features.DeliveryModule.Command;
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


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAdded(
             [FromQuery] string? search = null,
             [FromQuery] bool? isApproved = null,
             [FromQuery] DateTime? approvedFrom = null,
             [FromQuery] DateTime? approvedTo = null,
             [FromQuery] Guid? country = null,
             [FromQuery] Guid? gov = null,
             [FromQuery] Guid? city = null,
             [FromQuery] Guid? village = null,
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetAllExtrnalDeliveries")]
        public async Task<IActionResult> GetAllExtrnalDeliveries(
             [FromQuery] string? search = null,
             [FromQuery] bool? isApproved = null,
             [FromQuery] DateTime? approvedFrom = null,
             [FromQuery] DateTime? approvedTo = null,
             [FromQuery] Guid? country = null,
             [FromQuery] Guid? gov = null,
             [FromQuery] Guid? city = null,
             [FromQuery] Guid? village = null,
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



        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
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

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(
             [FromForm] UpdateDeliveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(
         [FromBody] DeleteDeliveryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("CreateByAdmin")]
        public async Task<IActionResult> CreateByAdmin(
           [FromForm] CreateDeliveryByAdminCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("ApproveDelivery/{id}")]
        public async Task<IActionResult> ApproveDelivery(string id)
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
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("RejectDelivery/{id}")]
        public async Task<IActionResult> RejectDelivery(string id)
        {
            var command = new RejectDeliveryCommand
            {
                DeliveryId = id
            };

            var result = await _mediator.Send(command);

            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("DeleteDeliveryByAdmin/{id}")]
        public async Task<IActionResult> DeleteDeliveryByAdmin(Guid id)
        {
            var result = await _mediator.Send(new DeleteDeliveryByAdminCommand
            {
                DeliveryId = id
            });

            return StatusCode(result.StatusCode, result);
        }
    }
}
