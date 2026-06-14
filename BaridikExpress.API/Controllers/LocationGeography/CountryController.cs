
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.DeleteCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.ImportCountries;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.ExportCountries;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetALL;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetById;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetCountryTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class CountryController(ISender sender) : ControllerBase
{
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCountryQuery query)
    {
        var result = await sender.Send(query);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await sender.Send(new GetCountryByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCountryCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? StatusCode(201, result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCountryCommand command)
    {
        command = command with { Id = id };
        var result = await sender.Send(command);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCountryCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await sender.Send(new UpdateCountryToggleStatusCommand { Id = id });
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("template")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTemplate(CancellationToken ct)
    {
        var result = await sender.Send(new GetCountryTemplateQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "CountryTemplate.xlsx")
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken ct)
    {
        var result = await sender.Send(new ImportCountriesCommand(file), ct);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(CancellationToken ct)
    {
        var result = await sender.Send(new ExportCountriesQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "Countries.xlsx")
            : StatusCode(result.StatusCode, result);
    }
}