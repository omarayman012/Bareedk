using BaridikExpress.Application.Features.OurPlans.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.OurPlans;
using BaridikExpress.Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.OurPlans.Commands.UploadPlans;

public sealed class UploadPlansCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context)
    : IRequestHandler<
        UploadPlansCommand,
        Result<ExcelUploadResult<Plan>>>
{
    public async Task<Result<ExcelUploadResult<Plan>>> Handle(
        UploadPlansCommand request,
        CancellationToken cancellationToken)
    {
        var result = await excelService
            .UploadAsync<PlanExcelDto, Plan>(
                request.File,

                dto => new Plan(
                    dto.NameEn,
                    dto.NameAr,
                    Enum.Parse<PlanType>(dto.Type),
                    dto.FeaturesEn
                        .Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList(),
                    dto.FeaturesAr
                        .Split('|', StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .ToList(),
                    dto.DescriptionEn,
                    dto.DescriptionAr),

                async entity => await context.Plans
                    .AsNoTracking()
                    .AnyAsync(x =>
                        x.NameAr == entity.NameAr ||
                        x.NameEn == entity.NameEn,
                        cancellationToken),

                entity => $"{entity.NameAr}|{entity.NameEn}",
                cancellationToken);

        return Result<ExcelUploadResult<Plan>>
            .Success(result);
    }
}