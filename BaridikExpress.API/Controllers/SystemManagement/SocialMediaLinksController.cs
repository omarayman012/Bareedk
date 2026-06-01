using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSocialMediaLinks;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetSocialMediaLinks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SystemManagement;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class SocialMediaLinksController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSocialMediaLinksQuery(), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateSocialMediaLinksCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}