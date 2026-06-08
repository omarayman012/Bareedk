using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.BlogsModules
{

    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Authorize]
    public class BlogsCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogsCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
        [FromQuery] string? name,
        [FromQuery] bool? isActive,
        [FromQuery] string? createdById,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int? pageSize = 10,
        [FromQuery] int pageNumber = 1)
        {
            var query = new GetAllBlogsCategoriesQuery
            {
                Name = name,
                IsActive = isActive,
                CreatedById = createdById,
                FromDate = fromDate,
                ToDate = toDate,
                pageSize = pageSize,
                PageNumber = pageNumber
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] UpdateBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("togglestatus/{id:guid}")]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var command = new ToggleBlogsCategoryStatusCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetBlogsCategoryByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("importtemplate")]
        public async Task<IActionResult> Import([FromForm] ImportBlogsCategoriesExcelCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("exporttemplate")]
        public async Task<IActionResult> Export()
        {
            var result = await _mediator.Send(new ExportBlogsCategoriesExcelQuery());

            if (!result.IsSuccess || result.Data == null)
            {
                return StatusCode(result.StatusCode, result);
            }

            return File(
                result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "BlogsCategories.xlsx");
        }

        [HttpGet("downloadtemplate")]
        public async Task<IActionResult> DownloadTemplate()
        {
            var result = await _mediator.Send(new DownloadBlogsCategoryTemplateQuery());

            if (!result.IsSuccess || result.Data == null)
            {
                return StatusCode(result.StatusCode, result);
            }

            return File(
                result.Data,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "BlogsCategoriesTemplate.xlsx");
        }

    }
}
