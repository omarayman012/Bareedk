using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Entities.BlogsModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.BlogModules
{

    public interface IBlogService
    {
        Task<List<BlogTag>> HandleTagsAsync(List<TagDto> tags, Guid blogId);
        Task<BlogSeo> HandleSeoAsync(CreateBlogSeoDto? seo, Blog blog);
    }
}
