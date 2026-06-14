using BaridikExpress.Application.Features.Currencies.Commands.CreateCurrency;
using BaridikExpress.Application.Features.Currencies.Commands.DeleteCurrency;
using BaridikExpress.Application.Features.Currencies.Commands.UpdateCurrency;
using BaridikExpress.Application.Features.Currencies.Queries.GetAllCurrencies;
using BaridikExpress.Application.Features.Currencies.Queries.GetCurrencyById;
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

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
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
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new DeleteCurrencyCommand(id), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}