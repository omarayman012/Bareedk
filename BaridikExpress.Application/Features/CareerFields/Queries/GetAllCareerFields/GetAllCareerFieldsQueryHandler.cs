using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CareerFields.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Queries.GetAllCareerFields;

public class GetAllCareerFieldsQueryHandler(
    IGenericRepository<CareerField> repo,
    IStringLocalizer localizer
) : IRequestHandler<
        GetAllCareerFieldsQuery,
        Result<PaginatedList<GetAllCareerFieldsDto>>
    >
{
    private readonly IGenericRepository<CareerField> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<PaginatedList<GetAllCareerFieldsDto>>> Handle(
        GetAllCareerFieldsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var (nameEn, nameAr) =
                NormalizeHelper.Normalize(request.Name, request.Name);

            query = query.Where(x =>
                x.Name.En.Contains(nameEn) ||
                x.Name.Ar.Contains(nameAr)
            );
        }
        query = query.ApplyCommonFilters(request);

        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllCareerFieldsDto
            {
                Id = x.Id,
                Name = new LocalizedDto
                {
                    AR = x.Name.Ar,
                    EN = x.Name.En
                },
                CreatedBy = x.CreatedBy!.FullName,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : null,

                UpdatedAt = x.UpdatedAt,
                IsActive = x.IsActive
            });

        var paginatedResult =
            await PaginatedList<GetAllCareerFieldsDto>
                .CreateAsync(
                    result,
                    request.PageNumber,
                    request.PageSize
                );

        return Result<PaginatedList<GetAllCareerFieldsDto>>
            .Success(
                paginatedResult,
                _localizer["OperationCompletedSuccessfully"]
            );
    }
}