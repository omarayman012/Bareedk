using BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;
using BaridikExpress.Application.Features.DeliveryTypes.Commands.DeleteDeliveryTypes;
using BaridikExpress.Application.Features.DeliveryTypes.Commands.ToggleDeliveryTypeStatus;
using BaridikExpress.Application.Features.DeliveryTypes.Commands.UpdateDeliveryType;
using BaridikExpress.Application.Features.DeliveryTypes.Queries.GetAllDeliveryTypes;
using BaridikExpress.Application.Features.DeliveryTypes.Queries.GetDeliveryTypeById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.DeliveryTypeModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class DeliveryTypeController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateDeliveryTypeCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateDeliveryTypeCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDeliveryTypesQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetDeliveryTypeByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(new ToggleDeliveryTypeStatusCommand(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteList")]
    public async Task<IActionResult> DeleteList([FromBody] DeleteDeliveryTypesCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}