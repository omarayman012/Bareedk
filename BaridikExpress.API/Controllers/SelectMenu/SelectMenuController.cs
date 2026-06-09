using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.GenericSelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Nationalities;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.Vehicles;
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
    public async Task<IActionResult> GetCountries(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Country>
            {
                Name = name
            },
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("governments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGovernments(
        [FromQuery] Guid? parentId,
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Government>
            {
                ParentId = parentId,
                Name = name
            },
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("cities")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCities(
        [FromQuery] Guid? parentId,
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<City>
            {
                ParentId = parentId,
                Name = name
            },
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("villages")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVillages(
        [FromQuery] Guid? parentId,
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetSelectMenuQuery<Village>
            {
                ParentId = parentId,
                Name = name
            },
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("nationalities")]
    [AllowAnonymous]
    public async Task<IActionResult> GetNationalities(
      [FromQuery] string? name,
      CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetNationalitiesSelectMenuQuery(name),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("CareerField")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCareerFieldes(
  [FromQuery] string? name,
  CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
           new GetSelectMenubaseQuery<CareerField>
           {
               Name = name
           },
           cancellationToken);

        return Ok(result);
    }


    [HttpGet("vehicles")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVehicles(
[FromQuery] string? name,
CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
           new GetSelectMenubaseQuery<Vehicle>
           {
               Name = name
           },
           cancellationToken);

        return Ok(result);
    }
}