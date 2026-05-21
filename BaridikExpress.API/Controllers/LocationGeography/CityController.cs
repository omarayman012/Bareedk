using BaridikExpress.Application.Features.LocationGeography.Commands.City.CreateCity;
using BaridikExpress.Application.Features.LocationGeography.Commands.City.DeleteCity;
using BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateCity;
using BaridikExpress.Application.Features.LocationGeography.Commands.City.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Queries.City.GetAll;
using BaridikExpress.Application.Features.LocationGeography.Queries.City.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Tags("Cities")]
public class CityController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/city?pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllCityQuery query)
    {
        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // GET: api/city/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetCityByIdQuery
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // POST: api/city
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCityCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    // PUT: api/city
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCityCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // DELETE: api/city
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteCityCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // PATCH: api/city/toggle-status/{id}
    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _mediator.Send(
            new UpdateCityToggleStatusCommand
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}