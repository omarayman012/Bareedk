namespace BaridikExpress.Application.Features.Announcements.Commands.ToggleAnnouncementStatus;

public record ToggleAnnouncementStatusCommand(
    Guid Id
) : IRequest<Result<bool>>;