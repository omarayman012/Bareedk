using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using BaridikExpress.Application.Features.Notification.DTOs;
using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Domain.Entities.BlogsModules;
using BaridikExpress.Domain.Entities.NotificationModules;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment;

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

    public async Task<Result<int>> Handle(
        ToggleCommentReactionCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = _currentUser.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Result<int>.Failure(
                    _localizer["UserNotAuthenticated"],
                    401);
            }

            var comment = await _context.BlogComments
                .Where(x => x.Id == request.CommentId)
                .Select(x => new
                {
                    x.Id,
                    x.BlogId,
                    x.UserId
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (comment is null)
            {
                return Result<int>.Failure(
                    _localizer["CommentNotFound"],
                    404);
            }

            var existingReaction = await _context.CommentReactions
                .FirstOrDefaultAsync(
                    x => x.CommentId == request.CommentId &&
                         x.UserId == userId,
                    cancellationToken);

            if (existingReaction is null)
            {
                _context.CommentReactions.Add(new CommentReaction
                {
                    Id = Guid.NewGuid(),
                    CommentId = request.CommentId,
                    UserId = userId,
                    Type = request.Type
                });
            }
            else if (existingReaction.Type == request.Type)
            {
                _context.CommentReactions.Remove(existingReaction);
            }
            else
            {
                existingReaction.Type = request.Type;
            }

            Notification? notification = null;
            string? commentOwnerId = comment.UserId;
            string? reactorName = null;

            if (!string.IsNullOrWhiteSpace(commentOwnerId) && commentOwnerId != userId)
            {
                var user = await _context.ApplicationUsers
                    .AsNoTracking()
                    .Where(x => x.Id == userId)
                    .Select(x => new
                    {
                        x.FullName
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                reactorName = string.IsNullOrWhiteSpace(user?.FullName)
                    ? "Someone"
                    : user.FullName;

                notification = Notification.Create(
                    userId: commentOwnerId,
                    titleAr: "تفاعل جديد",
                    titleEn: "New Reaction",
                    messageAr: $"{reactorName} تفاعل مع تعليقك",
                    messageEn: $"{reactorName} reacted to your comment",
                    blogId: comment.BlogId,
                    commentId: comment.Id);

                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            var reactionCounts = await _context.CommentReactions
                .AsNoTracking()
                .Where(x => x.CommentId == request.CommentId)
                .GroupBy(x => x.Type)
                .Select(x => new
                {
                    Type = x.Key,
                    Count = x.Count()
                })
                .ToListAsync(cancellationToken);

            var likes = reactionCounts
                .FirstOrDefault(x => x.Type == ReactionType.Like)
                ?.Count ?? 0;

            var dislikes = reactionCounts
                .FirstOrDefault(x => x.Type == ReactionType.Dislike)
                ?.Count ?? 0;

            await _realtimeService.SendReactionUpdatedAsync(
                comment.BlogId,
                request.CommentId,
                likes,
                dislikes);

            if (notification is not null && !string.IsNullOrWhiteSpace(commentOwnerId))
            {
                var realtimeMessage = new RealtimeNotificationMessage(
                    TitleAr: notification.TitleAr,
                    TitleEn: notification.TitleEn,
                    DescriptionAr: notification.MessageAr,
                    DescriptionEn: notification.MessageEn,
                    ImageUrl: notification.ImageUrl);

                await _notificationService.SendAsync(
                    commentOwnerId,
                    realtimeMessage,
                    cancellationToken);
            }

            return Result<int>.Success(
                likes,
                _localizer["ReactionUpdatedSuccessfully"]);
        }
        catch (DbUpdateException)
        {
            return Result<int>.Failure(
                _localizer["DatabaseError"],
                500);
        }
        catch (Exception)
        {
            return Result<int>.Failure(
                _localizer["UnexpectedError"],
                500);
        }
    }
}