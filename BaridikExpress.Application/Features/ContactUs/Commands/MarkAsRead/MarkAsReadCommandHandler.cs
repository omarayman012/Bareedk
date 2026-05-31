using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ContactUs.Commands.MarkAsRead;

public sealed class MarkAsReadCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<MarkAsReadCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        MarkAsReadCommand request,
        CancellationToken cancellationToken)
    {
        var contactUs = await db.ContactUs
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (contactUs is null)
            return Result<bool>.Failure(localizer["ContactUsNotFound"], 404);

        contactUs.MarkAsRead();
        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["MarkedAsReadSuccessfully"]);
    }
}