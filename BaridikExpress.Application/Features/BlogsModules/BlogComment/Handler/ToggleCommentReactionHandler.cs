
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Domain.Entities.BlogsModules;
using BaridikExpress.Domain.Entities.NotificationModules;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment
{
    public class ToggleCommentReactionHandler(
        IApplicationDbContext context,
        IGetCurrentUserRepository currentUser,
        IStringLocalizer localizer,
        ICommentRealtimeService realtimeService,
        INotificationService notificationService)
        : IRequestHandler<ToggleCommentReactionCommand, Result<int>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IGetCurrentUserRepository _currentUser = currentUser;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly ICommentRealtimeService _realtimeService = realtimeService;
        private readonly INotificationService _notificationService = notificationService;

        public async Task<Result<int>> Handle(ToggleCommentReactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _currentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(userId))
                    return Result<int>.Failure(_localizer["UserNotAuthenticated"], 401);

                var comment = await _context.BlogComments
                    .FirstOrDefaultAsync(x => x.Id == request.CommentId, cancellationToken);

                if (comment == null)
                    return Result<int>.Failure(_localizer["CommentNotFound"], 404);

                var existing = await _context.CommentReactions
                    .FirstOrDefaultAsync(x =>
                        x.CommentId == request.CommentId &&
                        x.UserId == userId,
                        cancellationToken);

                if (existing == null)
                {
                    _context.CommentReactions.Add(new CommentReaction
                    {
                        Id = Guid.NewGuid(),
                        CommentId = request.CommentId,
                        UserId = userId,
                        Type = request.Type
                    });
                }
                else if (existing.Type == request.Type)
                {
                    _context.CommentReactions.Remove(existing);
                }
                else
                {
                    existing.Type = request.Type;
                }

                await _context.SaveChangesAsync(cancellationToken);

                var likes = await _context.CommentReactions
                    .CountAsync(x => x.CommentId == request.CommentId && x.Type == ReactionType.Like, cancellationToken);

                var dislikes = await _context.CommentReactions
                    .CountAsync(x => x.CommentId == request.CommentId && x.Type == ReactionType.Dislike, cancellationToken);

                await _realtimeService.SendReactionUpdatedAsync(
                    comment.BlogId,
                    request.CommentId,
                    likes,
                    dislikes
                );

                var commentOwnerId = comment.UserId;

                if (!string.IsNullOrWhiteSpace(commentOwnerId) && commentOwnerId != userId)
                {
                    var user = await _context.ApplicationUsers
                        .Where(x => x.Id == userId)
                        .Select(x => new
                        {
                            x.FullName
                        })
                        .FirstOrDefaultAsync(cancellationToken);

                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        UserId = commentOwnerId,
                        Title = "New Reaction",
                        Message = $"{user?.FullName ?? "Someone"} reacted to your comment",
                        BlogId = comment.BlogId,
                        CommentId = comment.Id
                    };

                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync(cancellationToken);

                    await _notificationService.SendAsync(commentOwnerId, new
                    {
                        notification.Id,
                        notification.Title,
                        notification.Message,
                        userName = user?.FullName,
                        commentId = comment.Id
                    });
                }

                return Result<int>.Success(likes, _localizer["ReactionUpdatedSuccessfully"]);
            }
            catch (DbUpdateException)
            {
                return Result<int>.Failure(_localizer["DatabaseError"], 500);
            }
            catch (Exception)
            {
                return Result<int>.Failure(_localizer["UnexpectedError"], 500);
            }
        }
    }
}