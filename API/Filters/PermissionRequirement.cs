using BaridikExpress.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BaridikExpress.API.Filters
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement(string permissionName)
        {
            PermissionName = permissionName;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IApplicationDbContext _dbContext;

        public PermissionHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // JWTs in this project use `sub` for user id (Program.cs clears inbound claim mapping),
            // while some Identity flows may still emit NameIdentifier.
            var userId =
                context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                ?? context.User.FindFirst("sub")?.Value
                ?? context.User.FindFirst("nameid")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Fail();
                return;
            }

            var userExists = await _dbContext.ApplicationUsers
                .AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                context.Fail();
                return;
            }

            var roleIds = await _dbContext.UserRoles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            if (!roleIds.Any())
            {
                context.Fail();
                return;
            }

            var roles = await _dbContext.Roles
                .Where(r => roleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();

            if (roles.Contains("Admin"))
            {
                context.Succeed(requirement);
                return;
            }

            var hasPermission = await _dbContext.RolePermissions
                .Include(rp => rp.Permission)
                .AnyAsync(rp =>
                    roleIds.Contains(rp.RoleId) &&
                    rp.Permission.PermissionName == requirement.PermissionName);

            if (hasPermission)
                context.Succeed(requirement);
            else
                context.Fail();
        }
    }
}