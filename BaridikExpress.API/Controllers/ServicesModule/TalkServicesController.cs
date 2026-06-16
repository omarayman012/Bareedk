using BaridikExpress.Application.Features.TalkServices.Commands.Create;
using BaridikExpress.Application.Features.TalkServices.Commands.Delete;
using BaridikExpress.Application.Features.TalkServices.Queries.GetAll;
using BaridikExpress.Application.Features.TalkServices.Queries.GetById;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
public sealed class TalkServicesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllTalkServicesQuery query)
    {
        var result = await mediator.Send(query);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(
            new GetTalkServiceByIdQuery(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTalkServiceCommand command)
    {
        var result = await mediator.Send(command);

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteTalkServicesCommand command)
    {
        var result = await mediator.Send(command);

        return StatusCode(result.StatusCode, result);
    }
}