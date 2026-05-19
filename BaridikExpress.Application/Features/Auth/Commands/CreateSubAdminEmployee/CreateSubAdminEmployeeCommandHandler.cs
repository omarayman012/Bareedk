using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateSubAdminEmployee;

public class CreateSubAdminEmployeeCommandHandler
    : IRequestHandler<CreateSubAdminEmployeeCommand, Result<SubAdminEmployeeResponse>>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly IStringLocalizer _localizer;

    public CreateSubAdminEmployeeCommandHandler(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        IStringLocalizer localizer)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _fileStorage = fileStorage;
        _localizer = localizer;
    }

    public async Task<Result<SubAdminEmployeeResponse>> Handle(
        CreateSubAdminEmployeeCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return Result<SubAdminEmployeeResponse>.Failure(_localizer["EmailAlreadyExists"]);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            var existingPhone = _userManager.Users
                .Any(x => x.PhoneNumber == request.PhoneNumber);
            if (existingPhone)
                return Result<SubAdminEmployeeResponse>.Failure(_localizer["PhoneNumberAlreadyExists"]);
        }

        var role = await _roleManager.FindByIdAsync(request.RoleId);
        if (role is null)
            return Result<SubAdminEmployeeResponse>.Failure(_localizer["RoleNotFound"]);

        string? profileImageUrl = null;
        if (request.ProfileImage is not null)
        {
            using var stream = request.ProfileImage.OpenReadStream();
            profileImageUrl = await _fileStorage.SaveFileAsync(
                stream,
                request.ProfileImage.FileName,
                "employees"
            );
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            ProfileImageUrl = profileImageUrl,
            CreatedAt = DateTime.UtcNow,
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            return Result<SubAdminEmployeeResponse>.Failure(errors);
        }

        await _userManager.AddToRoleAsync(user, role.Name!);

        var subAdmin = new SubAdminEmployee
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Gender = request.Gender.HasValue ? request.Gender.Value.ToString() : null,
            RoleId = request.RoleId,
        };
        await _context.SubAdminEmployees.AddAsync(subAdmin, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var response = new SubAdminEmployeeResponse
        {
            Id = subAdmin.Id,
            Name = user.FullName,
            Image = user.ProfileImageUrl,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber,
            Role = role.Name!,
            Gender=subAdmin.Gender,
            CreatedBy = user.CreatedById ?? string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedBy = null,
            UpdatedAt = null,
            IsActive = true,
        };

        return Result<SubAdminEmployeeResponse>.Success(response, _localizer["EmployeeCreatedSuccessfully"], 201);
    }
}