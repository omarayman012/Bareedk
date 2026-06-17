using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.Realtime
{

    public interface INotificationService
    {
        Task SendAsync(string userId, object data);
    }
}
