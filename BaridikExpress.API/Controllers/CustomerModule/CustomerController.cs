using BaridikExpress.Application.Features.Customer.Commands.CreateCustomer;
using BaridikExpress.Application.Features.Customer.Commands.DeleteCustomers;
using BaridikExpress.Application.Features.Customer.Commands.ToggleCustomerStatus;
using BaridikExpress.Application.Features.Customer.Commands.UpdateCustomer;
using BaridikExpress.Application.Features.Customer.Queries.GetAllCustomers;
using BaridikExpress.Application.Features.Customer.Queries.GetCustomerById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.CustomerModule
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Authorize]
    public class CustomerController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateCustomerCommand command)
        {
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateCustomerCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await mediator.Send(new GetCustomerByIdQuery(id));
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCustomersQuery query)
        {
            var result = await mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("ToggleStatus/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await mediator.Send(new ToggleCustomerStatusCommand(id));
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("DeleteList")]
        public async Task<IActionResult> DeleteList([FromBody] DeleteCustomersCommand command)
        {
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}