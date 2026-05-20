using BaridikExpress.Application.Features.Customer.Commands.CreateCustomer;
using BaridikExpress.Application.Features.Customer.Commands.DeleteCustomers;
using BaridikExpress.Application.Features.Customer.Commands.ToggleCustomerStatus;
using BaridikExpress.Application.Features.Customer.Queries.GetAllCustomers;
using BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.CustomerModule
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetCustomerByIdQuery(id));
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCustomersQuery query)
        {
            var result = await _mediator.Send(query);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpPatch("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _mediator.Send(new ToggleCustomerStatusCommand(id));
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }

        [HttpDelete("DeleteList")]
        public async Task<IActionResult> DeleteList([FromBody] DeleteCustomersCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);
            return Ok(result);
        }
    }
}