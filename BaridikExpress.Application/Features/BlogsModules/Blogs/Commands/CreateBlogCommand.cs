using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;

public class CreateBlogCommand : IRequest<Result<BlogDetailsResponse>>
{
    public string TitleAr { get; set; }
    public string TitleEn { get; set; }
    public string DescriptionAr { get; set; }
    public string DescriptionEn { get; set; }

    public Guid BlogsCategoryId { get; set; }
    public Guid BlogsAuthorId { get; set; }
    public Guid? PublishingHouseId { get; set; }

    public DateOnly CreatedDate { get; set; }
    public TimeOnly CreatedTime { get; set; }

    public IFormFile Image { get; set; }
    public bool IsActive { get; set; }

    public List<TagDto> Tags { get; set; } = new();
    public CreateBlogSeoDto? Seo { get; set; }
}