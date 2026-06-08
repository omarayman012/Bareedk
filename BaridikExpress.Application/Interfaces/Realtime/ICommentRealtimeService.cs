using BaridikExpress.Application.Features.BlogsModules.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.Realtime
{

    public interface ICommentRealtimeService
    {
        Task SendCommentCreatedAsync(Guid blogId, CommentResponse comment);
        Task SendReactionUpdatedAsync(Guid blogId, Guid commentId, int likes, int dislikes);
    }

}
