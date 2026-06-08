namespace BaridikExpress.Application.Features.BlogsModules.DTOs;
public class BlogSeoResponse
{
    public NameDto? MetaTitle { get; set; }
    public SlugDto? Slug { get; set; }
    public MetaKeywordsDto? MetaKeywords { get; set; }
    public string? MetaImage { get; set; }
    public DescriptionDto? MetaDescription { get; set; }

}
