using BaridikExpress.Application.Features.CareerFields.DTO;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.CareerFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.UploadCareerFields
{
    public class UploadCareerFieldsCommandHandler(
       IExcelService excelService,
       IApplicationDbContext context)
       : IRequestHandler<UploadCareerFieldsCommand, Result<ExcelUploadResult<CareerField>>>
    {
        private readonly IExcelService _excelService = excelService;
        private readonly IApplicationDbContext _context = context;

        public async Task<Result<ExcelUploadResult<CareerField>>> Handle(
      UploadCareerFieldsCommand request,
      CancellationToken cancellationToken)
        {
            var result = await _excelService.UploadAsync<CareerFieldExcelDto, CareerField>(
                request.File,

                // ✅ استخدام الـ Public Constructor الصح
                mapper: dto => new CareerField(dto.NameEn, dto.NameAr),

                existsChecker: async entity =>
                    await _context.CareerFields.AsNoTracking().AnyAsync(x =>
                        x.Name.Ar.Trim().ToLower() == entity.Name.Ar.Trim().ToLower() ||
                        x.Name.En.Trim().ToLower() == entity.Name.En.Trim().ToLower(),
                        cancellationToken),

                inFileKeySelector: entity => $"{entity.Name.Ar}|{entity.Name.En}",

                cancellationToken: cancellationToken
            );

            return Result<ExcelUploadResult<CareerField>>.Success(result);
        }
    }
}
