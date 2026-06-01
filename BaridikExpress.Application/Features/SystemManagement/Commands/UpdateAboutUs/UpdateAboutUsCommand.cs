using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateAboutUs;

public sealed class UpdateAboutUsCommand : IRequest<Result<bool>>
{
    public string? TitleAr { get; set; }
    public string? TitleEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public IFormFile? Image { get; set; }
    public IFormFile? Video { get; set; }
    public string? ExternalLinkYoutube { get; set; }
}