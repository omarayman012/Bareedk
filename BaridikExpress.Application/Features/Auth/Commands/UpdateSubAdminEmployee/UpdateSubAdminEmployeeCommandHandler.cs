using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.UpdateSubAdminEmployee;

public class UpdateSubAdminEmployeeCommandHandler(
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager,
    IApplicationDbContext context,
    IFileStorageService fileStorage,
    IStringLocalizer localizer)
        : IRequestHandler<UpdateSubAdminEmployeeCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IApplicationDbContext _context = context;
    private readonly IFileStorageService _fileStorage = fileStorage;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        UpdateSubAdminEmployeeCommand request, CancellationToken cancellationToken)
    {
        var subAdmin = await _context.SubAdminEmployees
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subAdmin is null)
            return Result<bool>.Failure(_localizer["EmployeeNotFound"]);

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            var existingEmail = await _userManager.Users
                .AnyAsync(x => x.Email == request.Email && x.Id != subAdmin.UserId, cancellationToken);
            if (existingEmail)
                return Result<bool>.Failure(_localizer["EmailAlreadyExists"]);

            subAdmin.User.Email = request.Email;
            subAdmin.User.UserName = request.Email;
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var existingPhone = await _userManager.Users
                .AnyAsync(x => x.PhoneNumber == request.PhoneNumber && x.Id != subAdmin.UserId, cancellationToken);
            if (existingPhone)
                return Result<bool>.Failure(_localizer["PhoneNumberAlreadyExists"]);

            subAdmin.User.PhoneNumber = request.PhoneNumber;
        }

        if (!string.IsNullOrWhiteSpace(request.RoleId))
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            if (role is null)
                return Result<bool>.Failure(_localizer["RoleNotFound"]);

            if (subAdmin.RoleId != request.RoleId)
            {
                var currentRoles = await _userManager.GetRolesAsync(subAdmin.User);
                await _userManager.RemoveFromRolesAsync(subAdmin.User, currentRoles);
                await _userManager.AddToRoleAsync(subAdmin.User, role.Name!);
                subAdmin.RoleId = request.RoleId;
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(subAdmin.User);
            var resetResult = await _userManager.ResetPasswordAsync(subAdmin.User, token, request.Password);
            if (!resetResult.Succeeded)
            {
                var errors = string.Join(", ", resetResult.Errors.Select(e => e.Description));
                return Result<bool>.Failure(errors);
            }
        }

        if (request.ProfileImage is not null)
        {
            using var stream = request.ProfileImage.OpenReadStream();
            subAdmin.User.ProfileImageUrl = await _fileStorage.UpdateFileAsync(
                stream,
                request.ProfileImage.FileName,
                subAdmin.User.ProfileImageUrl,
                "employees"
            );
        }

        if (!string.IsNullOrWhiteSpace(request.FullName))
            subAdmin.User.FullName = request.FullName;
        if (request.Gender.HasValue)
            subAdmin.Gender = request.Gender.Value.ToString();

        subAdmin.User.UpdatedAt = DateTime.UtcNow;

        var updateResult = await _userManager.UpdateAsync(subAdmin.User);
        if (!updateResult.Succeeded)
        {
            var errors = string.Join(", ", updateResult.Errors.Select(e => e.Description));
            return Result<bool>.Failure(errors);
        }

        _context.SubAdminEmployees.Update(subAdmin);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, _localizer["EmployeeUpdatedSuccessfully"]);
    }
}