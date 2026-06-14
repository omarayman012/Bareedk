using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.CreateVillage;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.DeleteVillage;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.ImportVillages;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateVillage;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.ExportVillages;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetAll;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetById;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetVillageTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Tags("Villages")]
public class VillageController(ISender sender) : ControllerBase
{
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllVillageQuery query)
    {
        var result = await sender.Send(query);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await sender.Send(new GetVillageByIdQuery { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVillageCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateVillageCommand command)
    {
        command.Id = id;
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteVillageCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await sender.Send(new UpdateVillageToggleStatusCommand { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpGet("template")]
    public async Task<IActionResult> GetTemplate(CancellationToken ct)
    {
        var result = await sender.Send(new GetVillageTemplateQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "VillageTemplate.xlsx")
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken ct)
    {
        var result = await sender.Send(new ImportVillagesCommand(file), ct);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(CancellationToken ct)
    {
        var result = await sender.Send(new ExportVillagesQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "Villages.xlsx")
            : StatusCode(result.StatusCode, result);
    }
}