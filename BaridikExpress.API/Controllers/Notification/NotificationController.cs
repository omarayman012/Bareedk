// NotificationController.cs
using BaridikExpress.Application.Features.Notification.Commands.Create;
using BaridikExpress.Application.Features.Notification.Commands.Delete;
using BaridikExpress.Application.Features.Notification.Commands.MarkAsRead;
using BaridikExpress.Application.Features.Notification.Commands.Update;
using BaridikExpress.Application.Features.Notification.Queries.GetAllNotifications;
using BaridikExpress.Application.Features.Notification.Queries.GetMyNotifications;
using BaridikExpress.Application.Features.Notification.Queries.GetNotificationById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.Notification;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpPost("Send")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> Send([FromForm] CreateSendNotificationCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllNotificationsQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetNotificationByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> Update(
     Guid id,
     [FromForm] UpdateNotificationCommand command)
    {
        command.Id = id;

        var result = await mediator.Send(command);

        return StatusCode(result.StatusCode, result);
    }
    [HttpDelete("DeleteList")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> DeleteList([FromBody] DeleteNotificationsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("MyNotifications")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> GetMyNotifications([FromQuery] GetMyNotificationsQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("MarkAsRead")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> MarkAsRead([FromBody] MarkNotificationsAsReadCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}