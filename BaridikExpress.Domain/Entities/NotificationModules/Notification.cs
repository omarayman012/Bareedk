using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.NotificationModules
{
    public class Notification : BaseEntity
    {
        public Guid Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public Guid? BlogId { get; set; }
        public Guid? CommentId { get; set; }
    }

}
