using BaridikExpress.Application.Features.BlogsModules.DTOs;
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

    public class CommentRealtimeService(
        IHubContext<CommentHub> hubContext)
        : ICommentRealtimeService
    {
        public async Task SendCommentCreatedAsync(Guid blogId, CommentResponse comment)
        {
            await hubContext.Clients
                .Group(blogId.ToString())
                .SendAsync("ReceiveComment", comment);
        }

        public async Task SendReactionUpdatedAsync(Guid blogId, Guid commentId, int likes, int dislikes)
        {
            await hubContext.Clients
                .Group(blogId.ToString())
                .SendAsync("ReceiveReaction", new
                {
                    commentId,
                    likes,
                    dislikes
                });
        }
    }
}
