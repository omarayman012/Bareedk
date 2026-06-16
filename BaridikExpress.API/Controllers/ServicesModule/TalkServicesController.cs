using BaridikExpress.Application.Features.TalkServices.Commands.Create;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public sealed class TalkServicesController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateTalkServiceCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}