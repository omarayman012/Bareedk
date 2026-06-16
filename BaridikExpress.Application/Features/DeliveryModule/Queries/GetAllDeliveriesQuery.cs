using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Queries
{
    public class GetAllDeliveriesQuery : IRequest<Result<PagedResult<GetAllDeliveriesDto>>>
    {
        // 🔍 Search
        public string? Search { get; set; }

        // ✔️ Approval
        public bool? IsApproved { get; set; }

        // 📅 Approved Date Range
        public DateTime? ApprovedFrom { get; set; }
        public DateTime? ApprovedTo { get; set; }

        // 📍 Location
        public Guid? Country { get; set; }
        public Guid? Gov { get; set; }
        public Guid? City { get; set; }
        public Guid? Village { get; set; }

        // 📄 Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
