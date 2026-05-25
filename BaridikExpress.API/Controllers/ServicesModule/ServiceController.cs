using BaridikExpress.Application.Features.Services.Commands.CreateService;
using BaridikExpress.Application.Features.Services.Commands.DeleteServices;
using BaridikExpress.Application.Features.Services.Commands.ToggleServiceStatus;
using BaridikExpress.Application.Features.Services.Commands.UpdateService;
using BaridikExpress.Application.Features.Services.Queries.GetAllServices;
using BaridikExpress.Application.Features.Services.Queries.GetServiceById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class ServiceController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] CreateServiceCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateServiceCommand command)
    {
        command.Id = id;
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllServicesQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetServiceByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(new ToggleServiceStatusCommand(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteList")]
    public async Task<IActionResult> DeleteList([FromBody] DeleteServicesCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}