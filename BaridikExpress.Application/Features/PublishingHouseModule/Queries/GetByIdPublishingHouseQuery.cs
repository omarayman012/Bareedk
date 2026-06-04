using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Queries
{
    public class GetByIdPublishingHouseQuery
         : IRequest<Result<PublishingHouseDetailsDto>>
    {
        public Guid Id { get; set; }
    }
}
