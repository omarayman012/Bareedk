using BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.ServiceModules;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Import;

public sealed class ImportServiceBusinessPlansCommandHandler(
IApplicationDbContext db,
IExcelService excelService,
IStringLocalizer localizer)
: IRequestHandler<ImportServiceBusinessPlansCommand, Result<string>>
{
    public async Task<Result<string>> Handle(
    ImportServiceBusinessPlansCommand request,
    CancellationToken cancellationToken)
    {
        var result = await excelService.UploadAsync<
        ServiceBusinessPlanExcelDto,
        ServiceBusinessPlan>(
        request.File,



                dto => ServiceBusinessPlan.Create(
                    dto.NameEn ?? string.Empty,
                    dto.NameAr ?? string.Empty,
                    dto.DescriptionEn,
                    dto.DescriptionAr,
                    null),

                entity => db.ServiceBusinessPlans.AnyAsync(
                    x =>
                        x.NameEn == entity.NameEn ||
                        x.NameAr == entity.NameAr,
                    cancellationToken),

                entity =>
                    $"{entity.NameEn}_{entity.NameAr}",

                cancellationToken);

        return Result<string>.Success(
            result.Message,
            localizer["ServiceBusinessPlansImportedSuccessfully"]);
    }

}
