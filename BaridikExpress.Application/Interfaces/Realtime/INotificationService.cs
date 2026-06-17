namespace BaridikExpress.Application.Interfaces.Realtime;

public interface INotificationService
{
    Task SendAsync(
        string userId,
        object data,
        CancellationToken cancellationToken = default);

    Task SendAsync(
        IReadOnlyCollection<string> userIds,
        object data,
        CancellationToken cancellationToken = default);
}