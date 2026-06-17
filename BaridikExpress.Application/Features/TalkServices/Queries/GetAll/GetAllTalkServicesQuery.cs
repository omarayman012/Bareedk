using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.TalkServices.DTOs;

namespace BaridikExpress.Application.Features.TalkServices.Queries.GetAll
{
    public sealed record GetAllTalkServicesQuery(
      string? Name,
      DateTime? FromDate,
      DateTime? ToDate,
      int PageNumber = 1,
      int PageSize = 10)
      : IRequest<Result<PaginatedList<GetTalkServiceDto>>>;
}
