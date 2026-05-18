using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetPermissions;

public class GetPermissionsQueryHandler(
    IApplicationDbContext context,
    IStringLocalizer localizer
) : IRequestHandler<GetPermissionsQuery, Result<List<PermissionDto>>>
{
    public async Task<Result<List<PermissionDto>>> Handle(
        GetPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var permissions = await context.Permissions
            .Select(p => new PermissionDto(p.PermissionId, p.PermissionName))
            .ToListAsync(cancellationToken);

        if (!permissions.Any())
            return Result<List<PermissionDto>>.Failure(
                localizer["NoPermissionsFound"], 404);

        return Result<List<PermissionDto>>.Success(
          permissions,
          localizer["PermissionsRetrievedSuccessfully"],
        200);
    }
}