using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Location;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Nationalities;
using BaridikExpress.Domain.Entities.Location;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SelectMenu;

[ApiController]
[Route("api/v1/select-menu")]
[ApiExplorerSettings(GroupName = "admin-v1")]

public class SelectMenuController(IMediator mediator) : ControllerBase
{
    [HttpGet("countries")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCountries(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Country>(),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("governments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGovernments(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Government>(),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("cities")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCities(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<City>(),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("villages")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVillages(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Village>(),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }
    [HttpGet("nationalities")]
    public async Task<IActionResult> GetNationalities(
       [FromQuery] string? name,
       CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetNationalitiesSelectMenuQuery(name),
            cancellationToken);

        return Ok(result);
    }
}