using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Domain.Entities.RoleModule;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateRole;

public class CreateRoleCommandHandler(
    RoleManager<IdentityRole> roleManager,
    IApplicationDbContext context,
    IStringLocalizer localizer
) : IRequestHandler<CreateRoleCommand, Result<CreateRoleResponse>>
{
    public async Task<Result<CreateRoleResponse>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var existingRole = await roleManager.FindByNameAsync(request.Name);
            if (existingRole != null)
                return Result<CreateRoleResponse>.Failure(
                    localizer["RoleAlreadyExists"], 400);

            var role = new IdentityRole
            {
                Name = request.Name,
                NormalizedName = request.Name.ToUpperInvariant()
            };

            var createResult = await roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result<CreateRoleResponse>.Failure(
                    string.Format(localizer["FailedToCreateRole"], errors), 400);
            }

            var permissionDtos = new List<PermissionDto>();

            if (request.PermissionIds != null && request.PermissionIds.Any())
            {
                var existingPermissions = await context.Permissions
                    .Where(p => request.PermissionIds.Contains(p.PermissionId))
                    .ToListAsync(cancellationToken);

                var invalidIds = request.PermissionIds
                    .Except(existingPermissions.Select(p => p.PermissionId))
                    .ToList();

                if (invalidIds.Any())
                {
                    await roleManager.DeleteAsync(role);
                    return Result<CreateRoleResponse>.Failure(
                        string.Format(localizer["InvalidPermissionIds"],
                        string.Join(", ", invalidIds)), 400);
                }

                var rolePermissions = existingPermissions.Select(p => new RolePermission
                {
                    RolePermissionId = Guid.NewGuid(),
                    RoleId = role.Id,
                    PermissionId = p.PermissionId
                }).ToList();

                await context.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                permissionDtos = existingPermissions
                    .Select(p => new PermissionDto(p.PermissionId, p.PermissionName))
                    .ToList();
            }

            var response = new CreateRoleResponse(
                Id:role.Id,
                Name: role.Name!,
                Permissions: permissionDtos  
            );

            return Result<CreateRoleResponse>.Success(
                response,
                localizer["RoleCreatedSuccessfully"],
                201);
        }
        catch (Exception ex)
        {
            var role = await roleManager.FindByNameAsync(request.Name);
            if (role != null)
                await roleManager.DeleteAsync(role);

            return Result<CreateRoleResponse>.Error(
                string.Format(localizer["FailedToCreateRole"], ex.Message), 500);
        }
    }
}