using Microsoft.AspNetCore.SignalR;

namespace BaridikExpress.Infrastructure.Services.Realtime.Hubs
{
    public class CommentHub : Hub
    {
        public async Task JoinBlog(string blogId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, blogId);
        }

        public async Task LeaveBlog(string blogId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, blogId);
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
            }

            await base.OnConnectedAsync();
        }
    }
}