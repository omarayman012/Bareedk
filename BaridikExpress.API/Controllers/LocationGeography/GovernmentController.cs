using BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.DeleteGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.ImportGovernments;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.ToggleStatusGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.ExportGovernments;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetAll;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetById;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetGovernmentTemplate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Tags("Governments")]
public class GovernmentController(ISender sender) : ControllerBase
{
    private const string ExcelContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllGovernmentQuery query)
    {
        var result = await sender.Send(query);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await sender.Send(new GetGovernmentByIdQuery { Id = id });
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGovernmentCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateGovernmentCommand command)
    {
        command.Id = id;
        var result = await sender.Send(command);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteGovernmentCommand command)
    {
        var result = await sender.Send(command);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await sender.Send(new UpdateGovernmentToggleStatusCommand { Id = id });
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("template")]
    public async Task<IActionResult> GetTemplate(CancellationToken ct)
    {
        var result = await sender.Send(new GetGovernmentTemplateQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "GovernmentTemplate.xlsx")
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken ct)
    {
        var result = await sender.Send(new ImportGovernmentsCommand(file), ct);
        return result.IsSuccess ? Ok(result) : StatusCode(result.StatusCode, result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export(CancellationToken ct)
    {
        var result = await sender.Send(new ExportGovernmentsQuery(), ct);
        return result.IsSuccess
            ? File(result.Data!, ExcelContentType, "Governments.xlsx")
            : StatusCode(result.StatusCode, result);
    }
}