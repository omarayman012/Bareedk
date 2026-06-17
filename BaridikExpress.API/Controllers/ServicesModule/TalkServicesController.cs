using BaridikExpress.Application.Features.ContactUs.Commands.SendEmail;
using BaridikExpress.Application.Features.ContactUs.Commands.SendSms;
using BaridikExpress.Application.Features.TalkServices.Commands.Create;
using BaridikExpress.Application.Features.TalkServices.Commands.Delete;
using BaridikExpress.Application.Features.TalkServices.Queries.GetAll;
using BaridikExpress.Application.Features.TalkServices.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
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
    [AllowAnonymous]
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
    [HttpPost("SendSms")]
    public async Task<IActionResult> SendSms([FromBody] SendSmsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    [HttpPost("SendEmail")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> SendEmail([FromForm] SendEmailCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

}