namespace BaridikExpress.Application.Features.Notification.Commands.Resend;

public sealed class ResendNotificationCommand : IRequest<Result<bool>>
{
    public Guid SendNotificationId { get; set; }
}
