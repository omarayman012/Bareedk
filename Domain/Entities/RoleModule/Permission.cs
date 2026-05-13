using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.RoleModule
{
    public class Permission
    {
        [Key]
        public Guid PermissionId { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public ICollection<RolePermission> RolePermissions { get; set; }= new List<RolePermission>();
    }
}
