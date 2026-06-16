using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.CreateAnnouncement;

public sealed class CreateAnnouncementCommandHandler(
    IGenericRepository<Announcement> repo,
    IStringLocalizer localizer)
    : IRequestHandler<CreateAnnouncementCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateAnnouncementCommand request,
        CancellationToken cancellationToken)
    {
        var (titleAr, titleEn) = NormalizeHelper.Normalize(request.TitleAr, request.TitleEn);
        var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(request.DescriptionAr, request.DescriptionEn);

        var isExist = await repo.AnyAsync(
            x => x.TitleAr == titleAr || x.TitleEn == titleEn,
            cancellationToken);

        if (isExist)
            return Result<Guid>.Failure(
                localizer["AnnouncementAlreadyExists"],
                409);

        var banner = new Announcement(
            titleEn,
            titleAr,
            descriptionEn,
            descriptionAr,
            request.Discount,
            request.TextColor,
            request.BackgroundColor);

        await repo.AddAsync(banner, cancellationToken);
        return Result<Guid>.Success(
            banner.Id,
            localizer["AnnouncementCreatedSuccessfully"],
            201);
    }
}
