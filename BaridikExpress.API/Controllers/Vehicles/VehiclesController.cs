using BaridikExpress.Application.Features.Vehicles.Commands.CreateVehicles;
using BaridikExpress.Application.Features.Vehicles.Commands.DeleteVehicles;
using BaridikExpress.Application.Features.Vehicles.Commands.ToggleVehicleStatus;
using BaridikExpress.Application.Features.Vehicles.Commands.UpdateVehicles;
using BaridikExpress.Application.Features.Vehicles.Commands.UploadVehicles;
using BaridikExpress.Application.Features.Vehicles.DTO;
using BaridikExpress.Application.Features.Vehicles.Queries.GetAllVehicles;
using BaridikExpress.Application.Features.Vehicles.Queries.GetVehicleById;
using BaridikExpress.Application.Interfaces.File;

namespace BaridikExpress.API.Controllers.Vehicles
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public class VehiclesController(
        IMediator mediator,
        IExcelService excelService)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly IExcelService _excelService = excelService;

        [HttpGet("GetAll")]
        [HasPermission(Permissions.VehiclesRead)]
        public async Task<IActionResult> GetAll(
            [FromQuery] GetAllVehiclesQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("ShowOneDetails/{id}")]
        [HasPermission(Permissions.VehiclesRead)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetVehicleByIdQuery(id));

            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("Create")]
        [HasPermission(Permissions.VehiclesCreate)]
        public async Task<IActionResult> Create(
            [FromForm] CreateVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("Update")]
        [HasPermission(Permissions.VehiclesUpdate)]
        public async Task<IActionResult> Update(
            [FromForm] UpdateVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Delete")]
        [HasPermission(Permissions.VehiclesDelete)]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteVehicleCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("ToggleStatus/{id}")]
        [HasPermission(Permissions.VehiclesUpdate)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _mediator.Send(
                new ToggleVehicleStatusCommand(id));

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("Export")]
        [HasPermission(Permissions.VehiclesRead)]
        public async Task<IActionResult> ExportData(
            [FromQuery] GetAllVehiclesQuery query)
        {
            var result = await _mediator.Send(query);

            var file = await _excelService
                .DownloadDataAsync<GetAllVehiclesDto>(
                    result.Data!.Items);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Vehicles.xlsx");
        }

        [HttpPost("Upload")]
        [HasPermission(Permissions.VehiclesCreate)]
        public async Task<IActionResult> UploadExcel(
            IFormFile file,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new UploadVehiclesCommand(file),
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }
    }
}