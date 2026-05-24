using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthDeliveryModule.Command
{
    public class ApproveDeliveryCommand : IRequest<Result<string>>
    {
        public Guid DeliveryId { get; set; }
    }
}
