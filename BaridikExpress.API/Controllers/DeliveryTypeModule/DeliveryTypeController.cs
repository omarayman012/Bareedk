using BaridikExpress.Application.Features.DeliveryTypes.Commands.CreateDeliveryType;
using BaridikExpress.Application.Features.DeliveryTypes.Queries.GetAllDeliveryTypes;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.DeliveryTypeModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
public class DeliveryTypeController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateDeliveryTypeCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDeliveryTypesQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }
}