using BaridikExpress.Application.Features.Notification.Commands.Create;
using BaridikExpress.Application.Features.Notification.Queries.GetAllNotifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.Notification;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpPost("Send")]
    public async Task<IActionResult> Send([FromForm] CreateSendNotificationCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }
}