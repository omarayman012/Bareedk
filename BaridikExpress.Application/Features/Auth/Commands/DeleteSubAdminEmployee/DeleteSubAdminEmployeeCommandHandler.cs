using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.DeleteSubAdminEmployee;

public class DeleteSubAdminEmployeeCommandHandler(
    UserManager<User> userManager,
    IApplicationDbContext context,
    IStringLocalizer localizer)
        : IRequestHandler<DeleteSubAdminEmployeeCommand, Result<bool>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IApplicationDbContext _context = context;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<bool>> Handle(
        DeleteSubAdminEmployeeCommand request, CancellationToken cancellationToken)
    {
        if (request.Ids is null || !request.Ids.Any())
            return Result<bool>.Failure(_localizer["IdsRequired"]);

        var distinctIds = request.Ids.Distinct().ToList();

        var subAdmins = await _context.SubAdminEmployees
            .Include(x => x.User)
            .Where(x => distinctIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (!subAdmins.Any())
            return Result<bool>.Failure(_localizer["EmployeeNotFound"]);

        var foundIds = subAdmins.Select(x => x.Id).ToList();
        var notFoundIds = distinctIds.Except(foundIds).ToList();
        if (notFoundIds.Any())
            return Result<bool>.Failure(_localizer["SomeEmployeesNotFound"]);

        var hasNullUser = subAdmins.Any(x => x.User is null);
        if (hasNullUser)
            return Result<bool>.Failure(_localizer["EmployeeUserNotFound"]);

        var users = subAdmins.Select(x => x.User).ToList();

        _context.SubAdminEmployees.RemoveRange(subAdmins);
        await _context.SaveChangesAsync(cancellationToken);

        var failedDeletes = new List<string>();
        foreach (var user in users)
        {
            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                failedDeletes.Add(user.Email ?? user.Id);
        }

        if (failedDeletes.Any())
            return Result<bool>.Failure(
                string.Format(_localizer["SomeUsersFailedToDelete"], string.Join(", ", failedDeletes))
            );

        return Result<bool>.Success(true, _localizer["EmployeeDeletedSuccessfully"]);
    }
}