using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.DTOs;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetBlogCategory
{
    public sealed record GetBlogsSelectMenuQuery(string? Name)
       : IRequest<Result<List<SelectMenuResponse>>>;
}
