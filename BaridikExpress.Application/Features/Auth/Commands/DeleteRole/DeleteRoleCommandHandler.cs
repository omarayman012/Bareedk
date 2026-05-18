using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Application.Features.Auth.Commands.DeleteRole;

public class DeleteRoleCommandHandler(
    RoleManager<IdentityRole> roleManager,
    IApplicationDbContext context,
    IStringLocalizer localizer
) : IRequestHandler<DeleteRoleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        DeleteRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id);
        if (role == null)
            return Result<bool>.Failure(localizer["RoleNotFound"], 404);

        var hasUsers = await context.UserRoles
            .AnyAsync(ur => ur.RoleId == request.Id, cancellationToken);

        if (hasUsers)
            return Result<bool>.Failure(localizer["RoleAssignedToUsers"], 400);

        var rolePermissions = await context.RolePermissions
            .Where(rp => rp.RoleId == request.Id)
            .ToListAsync(cancellationToken);

        if (rolePermissions.Any())
        {
            context.RolePermissions.RemoveRange(rolePermissions);
            await context.SaveChangesAsync(cancellationToken);
        }

        var result = await roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(
                string.Format(localizer["FailedToDeleteRole"], errors), 400);
        }

        return Result<bool>.Success(
            true,
            localizer["RoleDeletedSuccessfully"],
            200);
    }
}