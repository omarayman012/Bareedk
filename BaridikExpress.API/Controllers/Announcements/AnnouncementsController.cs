using BaridikExpress.Application.Features.Announcements.Commands.CreateAnnouncement;
using BaridikExpress.Application.Features.Announcements.Commands.DeleteAnnouncement;
using BaridikExpress.Application.Features.Announcements.Commands.ToggleAnnouncementStatus;
using BaridikExpress.Application.Features.Announcements.Commands.UpdateAnnouncement;
using BaridikExpress.Application.Features.Announcements.Commands.UploadAnnouncements;
using BaridikExpress.Application.Features.Announcements.DTO;
using BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncements;
using BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncementsMobile;
using BaridikExpress.Application.Features.Announcements.Queries.GetAnnouncementById;
using BaridikExpress.Application.Features.Banners.Queries.GetAllBannersMobile;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.Announcements;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
public class AnnouncementsController(
    IMediator mediator,
    IExcelService excelService)
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IExcelService _excelService = excelService;

    [HttpGet("GetAll")]
    [HasPermission(Permissions.AnnouncementsRead)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllAnnouncementsQuery query)
    {
        var result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("ShowOneDetails/{id}")]
    [HasPermission(Permissions.AnnouncementsRead)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetAnnouncementByIdQuery(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    [HasPermission(Permissions.AnnouncementsCreate)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAnnouncementCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update")]
    [HasPermission(Permissions.AnnouncementsUpdate)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateAnnouncementCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("Delete")]
    [HasPermission(Permissions.AnnouncementsDelete)]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteAnnouncementCommand command)
    {
        var result = await _mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPatch("ToggleStatus/{id}")]
    [HasPermission(Permissions.AnnouncementsUpdate)]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _mediator.Send(
            new ToggleAnnouncementStatusCommand(id));

        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("ExportExcel")]
    [HasPermission(Permissions.AnnouncementsRead)]
    public async Task<IActionResult> ExportData(
        [FromQuery] GetAllAnnouncementsQuery query)
    {
        var result = await _mediator.Send(query);

        var exportData = result.Data!.Items
            .Select(x => new AnnouncementExcelDto
            {
                TitleAr = x.Title.AR ?? string.Empty,
                TitleEn = x.Title.EN ?? string.Empty,
                TextColor = x.TextColor ?? string.Empty,
                BackgroundColor = x.BackgroundColor ?? string.Empty
            });

        var file = await _excelService
            .DownloadDataAsync<AnnouncementExcelDto>(exportData);

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Announcements.xlsx");
    }

    [HttpGet("GetExcelTemplate")]
    [HasPermission(Permissions.AnnouncementsRead)]
    public async Task<IActionResult> GetExcelTemplate()
    {
        var file = await _excelService
            .GenerateTemplateAsync<AnnouncementExcelDto>();

        return File(
            file,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "AnnouncementsTemplate.xlsx");
    }

    [HttpPost("UploadExcel")]
    [HasPermission(Permissions.AnnouncementsCreate)]
    public async Task<IActionResult> UploadExcel(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new UploadAnnouncementsCommand(file),
            cancellationToken);

        return StatusCode(result.StatusCode, result);
    }

    #region Mobile

    [HttpGet("GetAllAnnouncements")]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "client-v1")]
    public async Task<IActionResult> GetMobileAnnouncements(
    [FromQuery] GetAllAnnouncementsMobileQuery query)
    {
        var result = await _mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    #endregion


}