using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Queries.GetSubAdminEmployeeById;

public class GetSubAdminEmployeeByIdQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer)
        : IRequestHandler<GetSubAdminEmployeeByIdQuery, Result<SubAdminEmployeeResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<SubAdminEmployeeResponse>> Handle(
        GetSubAdminEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var subAdmin = await _context.SubAdminEmployees
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subAdmin is null)
            return Result<SubAdminEmployeeResponse>.Failure(_localizer["EmployeeNotFound"]);

        var response = new SubAdminEmployeeResponse
        {
            Id = subAdmin.Id,
            Name = subAdmin.User.FullName,
            Image = subAdmin.User.ProfileImageUrl,
            Email = subAdmin.User.Email!,
            PhoneNumber = subAdmin.User.PhoneNumber,
            Role = subAdmin.Role?.Name ?? string.Empty,
            CreatedBy = subAdmin.CreatedBy?.FullName ?? string.Empty,
            CreatedAt = subAdmin.CreatedAt,
            UpdatedBy = subAdmin.UpdatedBy?.FullName,
            UpdatedAt = subAdmin.UpdatedAt,
            IsActive = subAdmin.IsActive,
        };

        return Result<SubAdminEmployeeResponse>.Success(response, _localizer["EmployeeRetrievedSuccessfully"]);
    }
}