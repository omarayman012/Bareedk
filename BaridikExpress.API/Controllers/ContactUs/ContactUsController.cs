
using BaridikExpress.Application.Features.ContactUs.Commands.DeleteContactUs;
using BaridikExpress.Application.Features.ContactUs.Commands.MarkAllAsRead;
using BaridikExpress.Application.Features.ContactUs.Commands.MarkAsRead;
using BaridikExpress.Application.Features.ContactUs.Commands.SendContactUs;
using BaridikExpress.Application.Features.ContactUs.Commands.SendEmail;
using BaridikExpress.Application.Features.ContactUs.Commands.SendSms;
using BaridikExpress.Application.Features.ContactUs.Queries.ExportContactUs;
using BaridikExpress.Application.Features.ContactUs.Queries.GetAllContactUs;
using BaridikExpress.Application.Features.ContactUs.Queries.GetContactUsById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ContactUs;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ContactUsController(IMediator mediator) : ControllerBase
{
    [HttpPost("Send")]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> Send([FromBody] SendContactUsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllContactUsQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetContactUsByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("MarkAsRead/{id}")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var result = await mediator.Send(new MarkAsReadCommand(id));
       return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("MarkAsReadBulk")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> MarkAsReadBulk([FromBody] MarkAsReadBulkCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteList")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> DeleteList([FromBody] DeleteContactUsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Export")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> Export([FromQuery] ExportContactUsQuery query)
    {
        var bytes = await mediator.Send(query);
        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"ContactUs_{DateTime.UtcNow:yyyyMMdd_HHmmss}.xlsx");
    }

    [HttpPost("SendEmail")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SendEmail([FromForm] SendEmailCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("SendSms")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public async Task<IActionResult> SendSms([FromBody] SendSmsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}