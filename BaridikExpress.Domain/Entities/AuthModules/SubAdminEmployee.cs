using System.Data;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Domain.Entities.AuthModules
{
    public class SubAdminEmployee : BaseEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public string? Gender { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public IdentityRole? Role { get; set; }

  
    }
}