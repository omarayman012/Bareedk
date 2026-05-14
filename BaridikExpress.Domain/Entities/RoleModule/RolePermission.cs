using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BaridikExpress.Domain.Entities.RoleModule
{
    public class RolePermission
    {
        [Key]
        public Guid RolePermissionId { get; set; }
        public string RoleId { get; set; } =string.Empty;
        public Guid PermissionId { get; set; }
        public IdentityRole Role { get; set; } = default!;
        public Permission Permission { get; set; }=default!;
    }
}
