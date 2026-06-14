using BaridikExpress.Application.Features.Services.Commands.CreateService;
using BaridikExpress.Application.Features.Services.Commands.DeleteServices;
using BaridikExpress.Application.Features.Services.Commands.ImportServices;
using BaridikExpress.Application.Features.Services.Commands.ToggleServiceStatus;
using BaridikExpress.Application.Features.Services.Commands.UpdateService;
using BaridikExpress.Application.Features.Services.Queries.DownloadServiceTemplate;
using BaridikExpress.Application.Features.Services.Queries.ExportServices;
using BaridikExpress.Application.Features.Services.Queries.GetAllServices;
using BaridikExpress.Application.Features.Services.Queries.GetServiceById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class ServiceController(IMediator mediator) : ControllerBase
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create(
        [FromForm] CreateServiceCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateServiceCommand command)
    {
        command.Id = id;

        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetAll")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllServicesQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(
            new GetServiceByIdQuery(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(
            new ToggleServiceStatusCommand(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("DeleteList")]
    public async Task<IActionResult> DeleteList(
        [FromBody] DeleteServicesCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Template")]
    public async Task<IActionResult> DownloadTemplate()
    {
        var fileBytes = await mediator.Send(
            new DownloadServiceTemplateQuery());

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ServiceTemplate.xlsx");
    }

    [HttpGet("Export")]
    public async Task<IActionResult> Export()
    {
        var fileBytes = await mediator.Send(
            new ExportServicesQuery());

        return File(
            fileBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Services.xlsx");
    }

    [HttpPost("Import")]
    public async Task<IActionResult> Import(
        [FromForm] ImportServicesCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}