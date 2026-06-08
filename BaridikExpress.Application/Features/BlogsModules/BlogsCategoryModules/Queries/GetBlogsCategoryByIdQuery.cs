using BaridikExpress.Application.Features.BlogsModules.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries
{
    public class GetBlogsCategoryByIdQuery : IRequest<Result<GetBlogsCategoryByIdDto>>
    {
        public Guid Id { get; set; }
    }
}
