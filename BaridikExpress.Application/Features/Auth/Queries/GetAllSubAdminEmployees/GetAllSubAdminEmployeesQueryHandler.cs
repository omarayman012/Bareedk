using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Queries.GetAllSubAdminEmployees;

public class GetAllSubAdminEmployeesQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer)
        : IRequestHandler<GetAllSubAdminEmployeesQuery, Result<PaginatedList<SubAdminEmployeeResponse>>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<PaginatedList<SubAdminEmployeeResponse>>> Handle(
        GetAllSubAdminEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.SubAdminEmployees
            .Include(x => x.User)
            .Include(x => x.Role)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x => x.User.FullName.Contains(request.Name));

        if (request.IsActive.HasValue)
            query = query.Where(x => x.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.CreatedById))
            query = query.Where(x => x.CreatedById == request.CreatedById);

        if (request.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt <= request.ToDate.Value);

        var mapped = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new SubAdminEmployeeResponse
            {
                Id = x.Id,
                Name = x.User.FullName,
                Image = x.User.ProfileImageUrl,
                Email = x.User.Email!,
                PhoneNumber = x.User.PhoneNumber,
                Role = x.Role != null ? x.Role.Name! : string.Empty,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : string.Empty,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                UpdatedAt = x.UpdatedAt,
                IsActive = x.IsActive,
            });

        var result = await PaginatedList<SubAdminEmployeeResponse>.CreateAsync(
            mapped,
            request.PageNumber,
            request.PageSize
        );

        return Result<PaginatedList<SubAdminEmployeeResponse>>.Success(result, _localizer["EmployeesRetrievedSuccessfully"]);
    }
}