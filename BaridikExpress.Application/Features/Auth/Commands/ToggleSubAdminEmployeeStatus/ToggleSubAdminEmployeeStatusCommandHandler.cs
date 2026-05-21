using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.ToggleSubAdminEmployeeStatus;

public class ToggleSubAdminEmployeeStatusCommandHandler(
    UserManager<User> userManager,
    IApplicationDbContext context,
    IStringLocalizer localizer)
        : IRequestHandler<ToggleSubAdminEmployeeStatusCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IApplicationDbContext _context = context;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
    ToggleSubAdminEmployeeStatusCommand request, CancellationToken cancellationToken)
    {
        var subAdmin = await _context.SubAdminEmployees
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (subAdmin is null)
            return Result<bool>.Failure(_localizer["EmployeeNotFound"]);

        subAdmin.ToggleStatus();
        subAdmin.UpdatedAt = DateTime.UtcNow;

        _context.SubAdminEmployees.Update(subAdmin);
        await _context.SaveChangesAsync(cancellationToken);

        var message = subAdmin.IsActive
            ? _localizer["EmployeeActivatedSuccessfully"]
            : _localizer["EmployeeDeactivatedSuccessfully"];

        return Result<bool>.Success(true, message);
    }
}