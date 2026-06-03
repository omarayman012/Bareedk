using BaridikExpress.Application.DTOs.DeliveryModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Queries
{
    public class GetDeliveryByIdQuery
      : IRequest<Result<GetDeliveryByIdDto>>
    {
        public string Id { get; set; }
    }
}
