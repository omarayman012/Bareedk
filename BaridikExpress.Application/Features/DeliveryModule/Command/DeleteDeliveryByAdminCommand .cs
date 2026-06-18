using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Command
{
   
        public class DeleteDeliveryByAdminCommand : IRequest<Result<string>>
        {
            public Guid DeliveryId { get; set; }
        }
    
}

