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
[Route("api/v1/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Tags("Cities")]
public class CityController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllCityQuery query)
    {
        var result = await mediator.Send(query);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetCityByIdQuery
        {
            Id = id
        });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCityCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
     Guid id,
     [FromBody] UpdateCityCommand command)
    {
        command.Id = id;

        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCityCommand command)
    {
        var result = await mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(new UpdateCityToggleStatusCommand
        {
            Id = id
        });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}