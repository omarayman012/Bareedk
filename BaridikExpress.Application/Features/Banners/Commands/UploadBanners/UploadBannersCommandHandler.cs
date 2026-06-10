using BaridikExpress.Application.Features.Banners.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Commands.UploadBanners;

public class UploadBannersCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context)
    : IRequestHandler<
        UploadBannersCommand,
        Result<ExcelUploadResult<Banner>>>
{
    public async Task<Result<ExcelUploadResult<Banner>>> Handle(
        UploadBannersCommand request,
        CancellationToken cancellationToken)
    {
        var result = await excelService
            .UploadAsync<BannerExcelDto, Banner>(
                request.File,

                dto => new Banner(
                    dto.TitleEn,
                    dto.TitleAr,
                    dto.DescriptionEn,
                    dto.DescriptionAr,
                    dto.ImageUrl),

                async entity => await context.Banners
                    .AsNoTracking()
                    .AnyAsync(x =>
                        x.TitleAr == entity.TitleAr ||
                        x.TitleEn == entity.TitleEn,
                        cancellationToken),

                entity => $"{entity.TitleAr}|{entity.TitleEn}",

                cancellationToken);

        return Result<ExcelUploadResult<Banner>>
            .Success(result);
    }
}
