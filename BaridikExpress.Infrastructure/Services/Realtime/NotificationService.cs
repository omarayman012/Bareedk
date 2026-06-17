using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Infrastructure.Services.Realtime.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BaridikExpress.Infrastructure.Services.Realtime;

public sealed class NotificationService(
    IHubContext<NotificationHub> hub)
    : INotificationService
{
    private const string ReceiveNotificationMethod = "ReceiveNotification";

    public Task SendAsync(
        string userId,
        object data,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Task.CompletedTask;
        }

        return hub.Clients
            .User(userId)
            .SendAsync(
                ReceiveNotificationMethod,
                data,
                cancellationToken);
    }

    public Task SendAsync(
        IReadOnlyCollection<string> userIds,
        object data,
        CancellationToken cancellationToken = default)
    {
        if (userIds.Count == 0)
        {
            return Task.CompletedTask;
        }

        var validUserIds = userIds
            .Where(userId => !string.IsNullOrWhiteSpace(userId))
            .Distinct()
            .ToList();

        if (validUserIds.Count == 0)
        {
            return Task.CompletedTask;
        }

        return hub.Clients
            .Users(validUserIds)
            .SendAsync(
                ReceiveNotificationMethod,
                data,
                cancellationToken);
    }
}