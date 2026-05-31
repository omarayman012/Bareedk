using BaridikExpress.Application.Features.SystemManagement.Commands.CreateFAQ;
using BaridikExpress.Application.Features.SystemManagement.Commands.DeleteFAQs;
using BaridikExpress.Application.Features.SystemManagement.Commands.ToggleFAQStatus;
using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateFAQ;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetAllFAQs;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetFAQById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SystemManagement;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]

public class FAQController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]

    public async Task<IActionResult> GetAll([FromQuery] GetAllFAQsQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]

    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetFAQByIdQuery(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFAQCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateFAQCommand command,
        CancellationToken cancellationToken)
    {
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(new ToggleFAQStatusCommand(id));
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteList([FromBody] DeleteFAQsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}