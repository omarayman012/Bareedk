using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Notification.Commands.Create;

public sealed class CreateSendNotificationCommand : IRequest<Result<bool>>
{
    public string ?TitleAr { get; set; } = string.Empty;
    public string ?TitleEn { get; set; } = string.Empty;
    public string ?DescriptionAr { get; set; } = string.Empty;
    public string ? DescriptionEn { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }

    public List<Guid> ClientsCreatedByAdmin { get; set; } = new List<Guid>();
    public List<Guid> DeliveriesCreatedByAdmin { get; set; } = new List<Guid>();
    public List<Guid> ClientsExternalRegistration { get; set; } = new List<Guid>();
    public List<Guid> DeliveriesExternalRegistration { get; set; } = new List<Guid>();
}