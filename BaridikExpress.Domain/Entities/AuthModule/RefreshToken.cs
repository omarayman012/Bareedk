using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.AuthModule
{
    public class RefreshToken : BaseEntity
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;
        public bool IsActive => RevokedOn == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    }
}