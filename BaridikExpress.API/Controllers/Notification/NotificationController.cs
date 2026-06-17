using BaridikExpress.Application.Features.Notification.Commands.Create;
using BaridikExpress.Application.Features.Notification.Commands.Delete;
using BaridikExpress.Application.Features.Notification.Commands.MarkAsRead;
using BaridikExpress.Application.Features.Notification.Commands.Resend;
using BaridikExpress.Application.Features.Notification.Commands.Update;
using BaridikExpress.Application.Features.Notification.Queries.GetAllNotifications;
using BaridikExpress.Application.Features.Notification.Queries.GetMyNotifications;
using BaridikExpress.Application.Features.Notification.Queries.GetNotificationById;
using MediatR;
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
    public async Task<IActionResult> Send(
        [FromForm] CreateSendNotificationCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Resend/{id:guid}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> Resend(
      Guid id,
      CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ResendNotificationCommand
            {
                SendNotificationId = id
            },
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id:guid}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetNotificationByIdQuery(id),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id:guid}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateNotificationCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;

        var result = await mediator.Send(command, cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteList")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> DeleteList(
        [FromBody] DeleteNotificationsCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("MyNotifications")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> GetMyNotifications(
        [FromQuery] GetMyNotificationsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("MarkAsRead")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> MarkAsRead(
        [FromBody] MarkNotificationsAsReadCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}