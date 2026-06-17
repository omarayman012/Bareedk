using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Infrastructure.Services.Realtime.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Infrastructure.Services.Realtime
{

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendAsync(string userId, object data)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return;

            await _hub.Clients.User(userId)
                .SendAsync("ReceiveNotification", data);
        }
    }
}
