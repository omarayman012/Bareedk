using BaridikExpress.Application.Features.Announcements.Queries.GetAllAnnouncementsMobile;
using BaridikExpress.Application.Features.Announcements.Queries.GetAnnouncementById;
using BaridikExpress.Application.Features.OurPlans.Commands.CreatePlan;
using BaridikExpress.Application.Features.OurPlans.Commands.DeletePlan;
using BaridikExpress.Application.Features.OurPlans.Commands.TogglePlanStatus;
using BaridikExpress.Application.Features.OurPlans.Commands.UpdatePlan;
using BaridikExpress.Application.Features.OurPlans.Commands.UploadPlans;
using BaridikExpress.Application.Features.OurPlans.DTO;
using BaridikExpress.Application.Features.OurPlans.Queries.GetAllPlans;
using BaridikExpress.Application.Features.OurPlans.Queries.GetPlanById;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.OurPlans
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public class OurPlansController(IMediator mediator, IExcelService excelService) : ControllerBase

    {
        private readonly IMediator _mediator = mediator;
        private readonly IExcelService _excelService = excelService;

        [HttpGet("GetAll")]
        [HasPermission(Permissions.OurPlansRead)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllPlansQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ShowOneDetails/{id}")]
        [HasPermission(Permissions.OurPlansRead)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetPlanByIdQuery(id));

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Create")]
        [HasPermission(Permissions.OurPlansCreate)]
        public async Task<IActionResult> Create(
            [FromBody] CreatePlanCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Update")]
        [HasPermission(Permissions.OurPlansUpdate)]
        public async Task<IActionResult> Update(
            [FromBody] UpdatePlanCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Delete")]
        [HasPermission(Permissions.OurPlansDelete)]
        public async Task<IActionResult> Delete(
            [FromBody] DeletePlanCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("ToggleStatus/{id}")]
        [HasPermission(Permissions.OurPlansUpdate)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _mediator.Send(
                new TogglePlanStatusCommand(id));

            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("ExportExcel")]
        [HasPermission(Permissions.OurPlansRead)]
        public async Task<IActionResult> ExportData(
        [FromQuery] GetAllPlansQuery query)
        {
            var result = await _mediator.Send(query);

            var exportData = result.Data!.Items
                .Select(x => new PlanExcelDto
                {
                    NameAr = x.Name.AR ?? string.Empty,
                    NameEn = x.Name.EN ?? string.Empty,

                    DescriptionAr = x.Description?.AR,
                    DescriptionEn = x.Description?.EN,

                    Type = x.Type.ToString(),

                    FeaturesAr = string.Join(" | ", x.Features.AR),
                    FeaturesEn = string.Join(" | ", x.Features.EN)
                });

            var file = await _excelService
                .DownloadDataAsync<PlanExcelDto>(exportData);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Plans.xlsx");
        }

        [HttpGet("GetExcelTemplate")]
        [HasPermission(Permissions.OurPlansRead)]
        public async Task<IActionResult> GetExcelTemplate()
        {
            var file = await _excelService
                .GenerateTemplateAsync<PlanExcelDto>();

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "PlansTemplate.xlsx");
        }


        [HttpPost("UploadExcel")]
        [HasPermission(Permissions.OurPlansCreate)]
        public async Task<IActionResult> UploadExcel(
    IFormFile file,
    CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new UploadPlansCommand(file),
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }


        #region Mobile

        [HttpGet("mobile/GetAllOurPlans")]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = "client-v1")]
        public async Task<IActionResult> GetMobileOurPlans(
        [FromQuery] GetAllPlansQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("mobile/ShowOneDetails/{id}")]
        [AllowAnonymous]
        [ApiExplorerSettings(GroupName = "client-v1")]
        public async Task<IActionResult> GetByIdforMobile(Guid id)
        {
            var result = await _mediator.Send(
                new GetPlanByIdQuery(id));

            return StatusCode(result.StatusCode, result);
        }
        #endregion

    }

}
