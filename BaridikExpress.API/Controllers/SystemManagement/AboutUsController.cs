using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateAboutUs;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetAboutUs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SystemManagement;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class AboutUsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetAboutUsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateAboutUsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}