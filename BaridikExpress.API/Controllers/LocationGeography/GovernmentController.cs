using BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.DeleteGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.ToggleStatusGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateGovernment;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetAll;
using BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "location-geography-v1")]
[Tags("Governments")]
public class GovernmentController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    // GET: api/government?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllGovernmentQuery query)
    {
        var result = await _sender.Send(query);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // GET: api/government/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(
            new GetGovernmentByIdQuery
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // POST: api/government
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateGovernmentCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    // PUT: api/government
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateGovernmentCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // DELETE: api/government
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteGovernmentCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // PATCH: api/government/toggle-status/{id}
    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _sender.Send(
            new UpdateGovernmentToggleStatusCommand
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}