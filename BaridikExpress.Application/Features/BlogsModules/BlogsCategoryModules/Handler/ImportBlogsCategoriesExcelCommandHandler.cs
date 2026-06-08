
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class ImportBlogsCategoriesExcelCommandHandler
        : IRequestHandler<ImportBlogsCategoriesExcelCommand, Result<ImportBlogsCategoryExcelResultDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer _localizer;

        public ImportBlogsCategoriesExcelCommandHandler(
            IApplicationDbContext applicationDbContext,
            IExcelService excelService,
            IStringLocalizer localizer)
        {
            _applicationDbContext = applicationDbContext;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<Result<ImportBlogsCategoryExcelResultDto>> Handle(
            ImportBlogsCategoriesExcelCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return Result<ImportBlogsCategoryExcelResultDto>.Failure(
                        _localizer["FileIsRequired"],
                        400);
                }

                var excelUploadResult =
                    await _excelService.UploadAsync<
                        ImportBlogsCategoryExcelDto,
                        BlogsCategory>(
                        request.File,
                        dto => new BlogsCategory
                        {
                            Id = Guid.NewGuid(),
                            NameAr = dto.NameAr,
                            NameEn = dto.NameEn,
                            Priorty = dto.Priorty,
                            DescriptionAr = dto.DescriptionAr,
                            DescriptionEn = dto.DescriptionEn,
                            IsActive = dto.IsActive ?? true,
                            Image = string.Empty,
                            CreatedAt = DateTime.UtcNow
                        });
                var validEntitiesToInsert = new List<BlogsCategory>();
                var skippedRows = new List<int>(excelUploadResult.SkippedRows);
                var messages = new List<string>();

                var existingNamesAr = await _applicationDbContext.BlogsCategorys
                    .AsNoTracking()
                    .Where(category => category.NameAr != null)
                    .Select(category => category.NameAr!.Trim().ToLower())
                    .ToListAsync(cancellationToken);

                var existingNamesEn = await _applicationDbContext.BlogsCategorys
                    .AsNoTracking()
                    .Where(category => category.NameEn != null)
                    .Select(category => category.NameEn!.Trim().ToLower())
                    .ToListAsync(cancellationToken);

                var namesArInCurrentBatch = new HashSet<string>();
                var namesEnInCurrentBatch = new HashSet<string>();

                for (int index = 0; index < excelUploadResult.UploadedItems.Count; index++)
                {
                    var rowNumber = index + 2;
                    var item = excelUploadResult.UploadedItems[index];

                    var nameAr = item.NameAr?.Trim();
                    var nameEn = item.NameEn?.Trim();

                    if (string.IsNullOrWhiteSpace(nameAr) && string.IsNullOrWhiteSpace(nameEn))
                    {
                        skippedRows.Add(rowNumber);
                        messages.Add($"Row {rowNumber}: NameAr or NameEn is required.");
                        continue;
                    }

                    if (item.Priorty.HasValue && (item.Priorty < 0 || item.Priorty > 10))
                    {
                        skippedRows.Add(rowNumber);
                        messages.Add($"Row {rowNumber}: Priority must be between 0 and 10.");
                        continue;
                    }

                    var normalizedNameAr = string.IsNullOrWhiteSpace(nameAr) ? null : nameAr.ToLower();
                    var normalizedNameEn = string.IsNullOrWhiteSpace(nameEn) ? null : nameEn.ToLower();

                    var existsInDatabase =
                        (!string.IsNullOrWhiteSpace(normalizedNameAr) && existingNamesAr.Contains(normalizedNameAr)) ||
                        (!string.IsNullOrWhiteSpace(normalizedNameEn) && existingNamesEn.Contains(normalizedNameEn));

                    if (existsInDatabase)
                    {
                        skippedRows.Add(rowNumber);
                        messages.Add($"Row {rowNumber}: Category already exists in database.");
                        continue;
                    }

                    var existsInCurrentBatch =
                        (!string.IsNullOrWhiteSpace(normalizedNameAr) && namesArInCurrentBatch.Contains(normalizedNameAr)) ||
                        (!string.IsNullOrWhiteSpace(normalizedNameEn) && namesEnInCurrentBatch.Contains(normalizedNameEn));

                    if (existsInCurrentBatch)
                    {
                        skippedRows.Add(rowNumber);
                        messages.Add($"Row {rowNumber}: Duplicate category in uploaded file.");
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(normalizedNameAr))
                    {
                        namesArInCurrentBatch.Add(normalizedNameAr);
                    }

                    if (!string.IsNullOrWhiteSpace(normalizedNameEn))
                    {
                        namesEnInCurrentBatch.Add(normalizedNameEn);
                    }

                    var blogsCategory = new BlogsCategory
                    {
                        Id = Guid.NewGuid(),
                        NameAr = nameAr ?? nameEn ?? string.Empty,
                        NameEn = nameEn ?? nameAr ?? string.Empty,
                        Priorty = item.Priorty,
                        DescriptionAr = string.IsNullOrWhiteSpace(item.DescriptionAr) ? null : item.DescriptionAr.Trim(),
                        DescriptionEn = string.IsNullOrWhiteSpace(item.DescriptionEn) ? null : item.DescriptionEn.Trim(),
                        IsActive = item.IsActive ,
                        Image = string.Empty,
                        CreatedAt = DateTime.UtcNow
                    };

                    validEntitiesToInsert.Add(blogsCategory);
                }

                if (validEntitiesToInsert.Any())
                {
                    await _applicationDbContext.BlogsCategorys.AddRangeAsync(validEntitiesToInsert, cancellationToken);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }

                var result = new ImportBlogsCategoryExcelResultDto
                {
                    TotalRows = excelUploadResult.TotalRows,
                    UploadedCount = validEntitiesToInsert.Count,
                    SkippedCount = skippedRows.Count,
                    SkippedRows = skippedRows.Distinct().OrderBy(x => x).ToList(),
                    Messages = messages
                };

                return Result<ImportBlogsCategoryExcelResultDto>.Success(
                    result,
                    _localizer["BlogsCategoriesImportedSuccessfully"],
                    200);
            }
            catch
            {
                return Result<ImportBlogsCategoryExcelResultDto>.Error(
                    _localizer["FailedToImportBlogsCategories"],
                    500);
            }
        }
    }
}
