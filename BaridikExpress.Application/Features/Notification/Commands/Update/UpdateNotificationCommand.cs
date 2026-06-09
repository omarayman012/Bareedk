using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Notification.Commands.Update;

public sealed class UpdateNotificationCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? TitleAr { get; set; }
    public string? TitleEn { get; set; }
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
    public IFormFile? Image { get; set; }
}