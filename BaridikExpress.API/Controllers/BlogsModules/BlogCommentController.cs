using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.BlogsModules
{

    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Authorize]
    public class BlogCommentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IExcelService _excelService;

        public BlogCommentController(IMediator mediator, IExcelService excelService)
        {
            _mediator = mediator;
            _excelService = excelService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetComments(
           [FromQuery] Guid blogId,
           [FromQuery] string? name,
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
        {
            var query = new GetCommentsQuery
            {
                BlogId = blogId,
                Name = name,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("comment")]
        public async Task<IActionResult> GetById([FromQuery] GetCommentByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("comment/reaction")]
        public async Task<IActionResult> React([FromBody] ToggleCommentReactionCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("comment/reactions")]
        public async Task<IActionResult> GetReactions([FromQuery] GetCommentReactionsQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete("comments")]
        public async Task<IActionResult> DeleteComments([FromBody] DeleteCommentsCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("comment")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("export")]
        public async Task<IActionResult> Export(
        [FromQuery] Guid blogId,
        CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new GetCommentsExportQuery(blogId), cancellationToken);

            if (!result.IsSuccess)
                return Ok(result);

            var file = await _excelService.DownloadDataAsync<CommentExportDto>(result.Data);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Comments.xlsx");
        }
    }
}
