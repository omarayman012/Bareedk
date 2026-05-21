using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.DTOs;

namespace BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.Queries
{
    public sealed record GetNationalitiesSelectMenuQuery(
      string? Name
  ) : IRequest<IReadOnlyList<SelectMenuDto>>;
}
