using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Queries
{
    public class GetAllPublishingHouseQuery
        : IRequest<Result<PaginatedList<PublishingHouseGetAllDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Name { get; set; }
        public string? CreatedById { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
