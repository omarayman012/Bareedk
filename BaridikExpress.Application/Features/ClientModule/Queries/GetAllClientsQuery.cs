using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Queries
{
    public class GetAllClientsQuery : IRequest<Result<PagedResult<GetAllClientsDto>>>
    {
        public string? Search { get; set; }

        public Guid? CareerFieldId { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
