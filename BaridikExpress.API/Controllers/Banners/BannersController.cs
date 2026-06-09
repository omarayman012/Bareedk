using BaridikExpress.Application.Features.Banners.Commands.CreateBanner;
using BaridikExpress.Application.Features.Banners.Commands.DeleteBanner;
using BaridikExpress.Application.Features.Banners.Commands.ToggleBannerStatus;
using BaridikExpress.Application.Features.Banners.Commands.UpdateBanner;
using BaridikExpress.Application.Features.Banners.Commands.UploadBanners;
using BaridikExpress.Application.Features.Banners.DTO;
using BaridikExpress.Application.Features.Banners.Queries.GetAllBanners;
using BaridikExpress.Application.Features.Banners.Queries.GetBannerById;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.API.Controllers.Banners;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
public class BannersController(IMediator mediator, IExcelService excelService)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IExcelService _excelService = excelService;

    [HttpGet("GetAll")]
    [HasPermission(Permissions.BannersRead)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllBannersQuery query)
    {
        var result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("ShowOneDetails/{id}")]
    [HasPermission(Permissions.BannersRead)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetBannerByIdQuery(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    [HasPermission(Permissions.BannersCreate)]
    public async Task<IActionResult> Create(
        [FromForm] CreateBannerCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update")]
    [HasPermission(Permissions.BannersUpdate)]
    public async Task<IActionResult> Update(
        [FromForm] UpdateBannerCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("Delete")]
    [HasPermission(Permissions.BannersDelete)]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteBannerCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id}")]
    [HasPermission(Permissions.BannersUpdate)]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _mediator.Send(
            new ToggleBannerStatusCommand(id));

        return StatusCode(result.StatusCode, result);
    }


    [HttpGet("Export")]
    [HasPermission(Permissions.BannersRead)]
    public async Task<IActionResult> ExportData(
        [FromQuery] GetAllBannersQuery query)
    {
        var result = await _mediator.Send(query);

        var exportData = result.Data!.Items
            .Select(x => new BannerExcelDto
            {
                TitleAr = x.Title.AR ?? string.Empty,
                TitleEn = x.Title.EN ?? string.Empty,
                DescriptionAr = x.Description.AR ?? string.Empty,
                DescriptionEn = x.Description.EN ?? string.Empty,
                ImageUrl = x.ImageUrl ?? string.Empty
            });

        var file = await _excelService
            .DownloadDataAsync<BannerExcelDto>(exportData);

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Banners.xlsx");
    }

    [HttpPost("Upload")]
    [HasPermission(Permissions.BannersCreate)]
    public async Task<IActionResult> UploadExcel(
    IFormFile file,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new UploadBannersCommand(file),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }


}