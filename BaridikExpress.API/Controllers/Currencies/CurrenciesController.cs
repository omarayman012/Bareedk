using BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;
using BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;
using BaridikExpress.Application.Features.Currencies.Commands.UpdateCurrency;
using BaridikExpress.Application.Features.Currencies.Commands.UploadCurrencies;
using BaridikExpress.Application.Features.Currencies.DTO;
using BaridikExpress.Application.Features.Currencies.Queries.GetAllCurrencies;
using BaridikExpress.Application.Features.Currencies.Queries.GetCurrencyById;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.Currencies;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IExcelService _excelService;

    public CurrenciesController(IMediator mediator, IExcelService excelService)
    {
        _mediator = mediator;
        _excelService = excelService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAllCurrenciesQuery(search, pageNumber, pageSize),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetCurrencyByIdQuery(id),
            cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateCurrencyCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(
     Guid id,
     [FromBody] UpdateCurrencyCommand command,
     CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(command with { Id = id }, cancellationToken);

        if (command is null)
            return StatusCode(result.StatusCode, result.Message);

        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(
     [FromBody] List<Guid> ids,
     CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new DeleteCurrencyCommand(ids), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("Template")]
    public async Task<IActionResult> Template()
    {
        var file = await _excelService.DownloadTemplateAsync<CurrencyExcelImportDto>();

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "CurrencyTemplate.xlsx");
    }

    [HttpGet("Export")]
    public async Task<IActionResult> Export(
        [FromQuery] string? search,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(
            new GetAllCurrenciesQuery(search, 1, int.MaxValue),
            cancellationToken);

        var exportData = result.Data!.Items
            .Select(x => new CurrencyExcelExportDto
            {
                NameEn = x.NameEn,
                NameAr = x.NameAr,
                CurrencyCode = x.CurrencyCode,
                CurrencySymbol = x.CurrencySymbol,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy,
                UpdatedAt = x.UpdatedAt
            });

        var file = await _excelService.DownloadDataAsync(exportData);

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Currencies.xlsx");
    }

    [HttpPost("ImportExcelFile")]
    public async Task<IActionResult> ImportExcelFile(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UploadCurrenciesCommand(file), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}