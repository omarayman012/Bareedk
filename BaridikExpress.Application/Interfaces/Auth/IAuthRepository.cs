using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByEmailOrPhoneAsync(string target);
        Task<RoleDto?> GetRoleByNameAsync(string roleName);
        Task<List<PermissionDto>> GetPermissionsByRoleIdAsync(string roleId);
        Task<List<string>> GetAllPermissionNamesAsync();
        Task UpdateUserAsync(User user);
    }
}
