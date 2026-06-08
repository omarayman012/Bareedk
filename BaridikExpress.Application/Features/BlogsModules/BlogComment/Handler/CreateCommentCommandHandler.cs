
using BaridikExpress.Application.Features.BlogsModules.BlogComment.Commands;
using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Domain.Entities.NotificationModules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules.BlogComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<Guid>>
{
    private readonly IApplicationDbContext _context;
    private readonly IGetCurrentUserRepository _currentUser;
    private readonly IStringLocalizer _localizer;
    private readonly INotificationService _notificationService;

    public CreateCommentCommandHandler(
        IApplicationDbContext context,
        IGetCurrentUserRepository currentUser,
        IStringLocalizer localizer,
        INotificationService notificationService)
    {
        _context = context;
        _currentUser = currentUser;
        _localizer = localizer;
        _notificationService = notificationService;
    }

    public async Task<Result<Guid>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _currentUser.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Result<Guid>.Failure(_localizer["UserNotAuthenticated"], 401);

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(x => x.Id == request.BlogId, cancellationToken);
            if (blog == null)
                return Result<Guid>.Failure(_localizer["BlogNotFound"], 404);

            if (request.ParentId.HasValue)
            {
                var parentExists = await _context.BlogComments
                    .AnyAsync(x => x.Id == request.ParentId, cancellationToken);
                if (!parentExists)
                    return Result<Guid>.Failure(_localizer["ParentCommentNotFound"], 404);
            }

            var user = await _context.ApplicationUsers
                .Where(x => x.Id == userId)
                .Select(x => new { x.FullName })
                .FirstOrDefaultAsync(cancellationToken);

            var comment = new BaridikExpress.Domain.Entities.BlogsModules.BlogComment
            {
                Id = Guid.NewGuid(),
                BlogId = request.BlogId,
                UserId = userId,
                Content = request.Content,
                ParentId = request.ParentId
            };

            _context.BlogComments.Add(comment);

            Notification? notification = null;
            var blogOwnerId = blog.CreatedById;

            if (!string.IsNullOrWhiteSpace(blogOwnerId) && blogOwnerId != userId)
            {
                notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    UserId = blogOwnerId,
                    Title = "New Comment",
                    Message = $"{user?.FullName ?? "Someone"} commented on your post",
                    BlogId = request.BlogId,
                    CommentId = comment.Id
                };
                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync(cancellationToken);

            if (notification != null)
            {
                await _notificationService.SendAsync(blogOwnerId!, new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    userName = user?.FullName,
                    commentId = comment.Id
                });
            }

            return Result<Guid>.Success(comment.Id, _localizer["CommentCreatedSuccessfully"]);
        }
        catch (DbUpdateException)
        {
            return Result<Guid>.Failure(_localizer["DatabaseError"], 500);
        }
        catch (Exception)
        {
            return Result<Guid>.Failure(_localizer["UnexpectedError"], 500);
        }
    }
}