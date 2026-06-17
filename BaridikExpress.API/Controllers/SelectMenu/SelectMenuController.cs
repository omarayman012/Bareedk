using BaridikExpress.Application.Features.Blogs.Queries.SelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetBlogCategory;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetRolesSelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Currency;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Nationalities;
using BaridikExpress.Domain.Entities.Banners;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BaridikExpress.Domain.Entities.OurPlans;

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
            new GetSelectMenuQuery<Country> { Name = name },
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
            new GetSelectMenuQuery<Government> { ParentId = parentId, Name = name },
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
            new GetSelectMenuQuery<City> { ParentId = parentId, Name = name },
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
            new GetSelectMenuQuery<Village> { ParentId = parentId, Name = name },
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
           new GetSelectMenuQuery<CareerField>
           {
               Name = name
           },
           cancellationToken);


        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("vehicles")]
    [AllowAnonymous]
    public async Task<IActionResult> GetVehicles(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
           new GetSelectMenuQuery<Vehicle>
           {
               Name = name
           },
           cancellationToken);


        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("currencies")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCurrencies(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetCurrenciesQuery(),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Banners")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBanners(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
           new GetSelectMenuQuery<Banner>
           {
               Name = name
           },
           cancellationToken);


        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("roles")]
    [Authorize]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetRolesSelectMenuQuery(),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("blogs")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogs(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetBlogsSelectMenuQuery(name),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("blogs-categories")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogsCategories(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetBlogsCategoriesSelectMenuQuery(name),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("blogs-authors")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBlogsAuthors(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetBlogsAuthorsSelectMenuQuery(name),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("tags")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTags(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetTagsSelectMenuQuery(name),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("publishing-houses")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublishingHouses(
        [FromQuery] string? name,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new GetPublishingHousesSelectMenuQuery(name),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("ourPlans")]
    [AllowAnonymous]
    public async Task<IActionResult> GetOurPlans(
    [FromQuery] string? name,
    CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
           new GetSelectMenuQuery<Plan>
           {
               Name = name
           },
           cancellationToken);


        return StatusCode(result.StatusCode, result);
    }

}