using BaridikExpress.Application.Features.Banners.Commands.CreateBanner;
using BaridikExpress.Application.Features.Banners.Commands.DeleteBanner;
using BaridikExpress.Application.Features.Banners.Commands.UpdateBanner;
using BaridikExpress.Application.Common.Abstractions.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.Banners;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class BannersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = nameof(Permissions.BannersCreate))]
    public async Task<IActionResult> Create([FromForm] CreateBannerCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = nameof(Permissions.BannersUpdate))]
    public async Task<IActionResult> Update(Guid id, [FromForm] UpdateBannerCommand command)
    {
        var updateCommand = new UpdateBannerCommand(
            id,
            command.TitleAr,
            command.TitleEn,
            command.DescriptionAr,
            command.DescriptionEn,
            command.Image);
        var result = await mediator.Send(updateCommand);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    [Authorize(Policy = nameof(Permissions.BannersDelete))]
    public async Task<IActionResult> Delete([FromBody] DeleteBannerCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
