using BaridikExpress.Application.Features.ClientAddresses.Commands.CreateAddress;
using BaridikExpress.Application.Features.ClientAddresses.Commands.DeleteAddresses;
using BaridikExpress.Application.Features.ClientAddresses.Commands.UpdateAddress;
using BaridikExpress.Application.Features.ClientAddresses.Queries.GetAllAddresses;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.ClientAddresses;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "client-v1")]
public class AddressesController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("GetAll")]
    [HasPermission(Permissions.ClientAddressesRead)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllAddressesQuery query)
    {
        var result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    [HasPermission(Permissions.ClientAddressesCreate)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAddressCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update")]
    [HasPermission(Permissions.ClientAddressesUpdate)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateAddressCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("Delete")]
    [HasPermission(Permissions.ClientAddressesDelete)]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteAddressesCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}