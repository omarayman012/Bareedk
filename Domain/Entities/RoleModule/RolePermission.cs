using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

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
