using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.Blogs.Queries
{
    public class GetBlogReactionsQuery : IRequest<Result<BlogReactionsResponse>>
    {
        public Guid BlogId { get; set; }
    }
}
