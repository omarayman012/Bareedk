using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.CreateVillage;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.DeleteVillage;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.UpdateVillage;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetAll;
using BaridikExpress.Application.Features.LocationGeography.Queries.Villiage.GetById;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VillageController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    // GET api/village
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllVillageQuery query)
    {
        var result = await _sender.Send(query);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // GET api/village/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(
            new GetVillageByIdQuery
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // POST api/village
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateVillageCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? StatusCode(201, result)
            : StatusCode(result.StatusCode, result);
    }

    // PUT api/village
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateVillageCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // DELETE api/village
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteVillageCommand command)
    {
        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // PATCH api/village/toggle-status/{id}
    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _sender.Send(
            new UpdateVillageToggleStatusCommand
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}