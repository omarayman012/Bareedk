using BaridikExpress.Domain.Entities.AuthModule;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.RoleModule;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Domain.Entities.AuthModules
{
    public class User : IdentityUser,IAuditableEntity
    {

        public string FullName { get; set; } = string.Empty;

        public ICollection<RolePermission> Roles { get; set; } = new HashSet<RolePermission>();
        public string? ProfileImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }
        public string? CreatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedById { get; set; }
        public SubAdminEmployee? SubAdminEmployee { get; set; }

        public string? EmailOtp { get; set; }

        public DateTime? EmailOtpExpireAt { get; set; }
     
    
        public DateTime? EmailOtpLastSentAt { get; set; }


        public string? PhoneOtp { get; set; }
        public DateTime? PhoneOtpExpireAt { get; set; }
        public DateTime? PhoneOtpLastSentAt { get; set; }

        public string? ResetPasswordOtp { get; set; }
        public DateTime? ResetPasswordOtpExpireAt { get; set; }
        public DateTime? ResetPasswordOtpLastSentAt { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpireAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpireAt { get; set; }
    }
}
