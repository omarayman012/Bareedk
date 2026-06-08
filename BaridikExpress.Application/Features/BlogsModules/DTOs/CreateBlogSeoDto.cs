using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs;
public class CreateBlogSeoDto
{
    public string? MetaTitleAr { get; set; }
    public string? MetaTitleEn { get; set; }
    public List<string> SlugAr { get; set; } = new();
    public List<string> SlugEn { get; set; } = new();
    public List<string>? MetaKeywordsAr { get; set; }
    public List<string>? MetaKeywordsEn { get; set; }
    public IFormFile? MetaImage { get; set; }
    public string? MetaDescriptionAr { get; set; }
    public string? MetaDescriptionEn { get; set; }
}