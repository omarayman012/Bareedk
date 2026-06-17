using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetSystemManagement;
using BaridikExpress.Domain.Entities.NotificationModules;
using Microsoft.AspNetCore.Authorization;


namespace BaridikExpress.API.Controllers.NotificationMessage;

[Route("api/v1/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class MessageNotifactionController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult>Get(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSystemManagementQuery<MessageNotification>(), cancellationToken);
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UpdateSystemManagementCommand<MessageNotification>command,CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}