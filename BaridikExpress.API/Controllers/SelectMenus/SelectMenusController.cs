using BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SelectMenus;

[ApiExplorerSettings(GroupName = "admin-v1")]
[Tags("Select Menus")]
[Route("api/select-menus")]
[ApiController]
public class SelectMenusController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("nationalities")]
    public async Task<IActionResult> GetNationalities(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetNationalitiesSelectMenuQuery(name),
            cancellationToken);

        return Ok(result);
    }
}