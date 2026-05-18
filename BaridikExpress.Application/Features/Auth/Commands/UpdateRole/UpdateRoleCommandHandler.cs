using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Application.Features.Auth.Commands.UpdateRole;

public class UpdateRoleCommandHandler(
    RoleManager<IdentityRole> roleManager,
    IStringLocalizer localizer
) : IRequestHandler<UpdateRoleCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByIdAsync(request.Id);
        if (role == null)
            return Result<bool>.Failure(localizer["RoleNotFound"], 404);

        var nameExists = await roleManager.FindByNameAsync(request.Name);
        if (nameExists != null && nameExists.Id != request.Id)
            return Result<bool>.Failure(localizer["RoleAlreadyExists"], 400);

        role.Name = request.Name;
        role.NormalizedName = request.Name.ToUpperInvariant();

        var result = await roleManager.UpdateAsync(role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<bool>.Failure(
                string.Format(localizer["FailedToUpdateRole"], errors), 400);
        }

        return Result<bool>.Success(
            true,
            localizer["RoleUpdatedSuccessfully"],
            200);
    }
}