using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.AuthModule
{
    public class RefreshToken: BaseEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime ExpiresOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public bool IsActive => RevokedOn == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
    }
}
