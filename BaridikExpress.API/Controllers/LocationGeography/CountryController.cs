using BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.DeleteCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateToggleStatus;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetALL;
using BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetById;
using Microsoft.AspNetCore.Authorization;


namespace BaridikExpress.API.Controllers.LocationGeography;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class CountryController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    // GET api/country?pageNumber=1&pageSize=10
    [HttpGet]
    [AllowAnonymous]

    public async Task<IActionResult> GetAll([FromQuery] GetAllCountryQuery query)
    {
        var result = await _sender.Send(query);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // GET api/country/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetCountryByIdQuery { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // POST api/country
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCountryCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess
            ? StatusCode(201, result)
            : StatusCode(result.StatusCode, result);
    }

    // PUT api/country/Id
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCountryCommand command)
    {
        command = command with { Id = id };
        var result = await _sender.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // DELETE api/country
    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteCountryCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // PATCH api/country/{id}/toggle-status
    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _sender.Send(new UpdateCountryToggleStatusCommand { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}