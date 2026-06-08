using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.BlogsCategoryModules
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
        [HttpGet]
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
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteBlogsCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("toggle-status/{id:guid}")]
        public async Task<IActionResult> ToggleActiveStatus(Guid id)
        {
            var command = new ToggleBlogsCategoryStatusCommand
            {
                Id = id
            };

            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetBlogsCategoryByIdQuery
            {
                Id = id
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost("import")]
        public async Task<IActionResult> Import([FromForm] ImportBlogsCategoriesExcelCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("export")]
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

        [HttpGet("template")]
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
