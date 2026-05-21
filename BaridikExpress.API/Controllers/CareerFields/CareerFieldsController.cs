using BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields;
using BaridikExpress.Application.Features.CareerFields.Commands.DeleteCareerFields;
using BaridikExpress.Application.Features.CareerFields.Commands.ToggleCareerFieldStatus;
using BaridikExpress.Application.Features.CareerFields.Commands.UpdateCareerFields;
using BaridikExpress.Application.Features.CareerFields.Commands.UploadCareerFields;
using BaridikExpress.Application.Features.CareerFields.DTOs;
using BaridikExpress.Application.Features.CareerFields.Queries.GetAllCareerFields;
using BaridikExpress.Application.Features.CareerFields.Queries.GetCareerFieldById;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.API.Controllers.CareerFields
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]

    public class CareerFieldsController(IMediator mediator, IExcelService excelService) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IExcelService _excelService = excelService;
        [HttpGet]
        [HasPermission(Permissions.CareerFieldsRead)]
        public async Task<IActionResult> GetAll(
         [FromQuery] GetAllCareerFieldsQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        [HasPermission(Permissions.CareerFieldsRead)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetCareerFieldByIdQuery(id)
            );
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        [HasPermission(Permissions.CareerFieldsCreate)]
        public async Task<IActionResult> Create(
                [FromBody] CreateCareerFieldCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut]
        [HasPermission(Permissions.CareerFieldsUpdate)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateCareerFieldCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        [HasPermission(Permissions.CareerFieldsDelete)]
        public async Task<IActionResult> Delete(DeleteCareerFieldsCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("{id}/toggle-status")]
        [HasPermission(Permissions.CareerFieldsUpdate)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _mediator.Send(
                new ToggleCareerFieldStatusCommand(id)
            );

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("export")]
        [HasPermission(Permissions.CareerFieldsRead)]
        public async Task<IActionResult> ExportData([FromQuery] GetAllCareerFieldsQuery query)
        {
            var result = await _mediator.Send(query);
            var file = await _excelService.DownloadDataAsync<GetAllCareerFieldsDto>(
                result.Data!.Items);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "CareerFields.xlsx");
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new UploadCareerFieldsCommand(file), cancellationToken);
            return StatusCode(result.StatusCode, result);
        }
    }
}

