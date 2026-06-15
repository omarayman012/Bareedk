using BaridikExpress.Application.Features.BlogsModules.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Queries
{

    public class GetFavoriteBlogsQuery : IRequest<Result<PaginatedList<FavoriteBlogDto>>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string? Name { get; set; }
    }
}
