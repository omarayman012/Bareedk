using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries
{
    public class GetAllBlogsCategoriesQuery
     : IRequest<Result<PaginatedList<GetAllBlogsCategoryDto>>>
    {
        public string? Name { get; set; } 

        public bool? IsActive { get; set; }

        public string? CreatedById { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? pageSize { get; set; } = 10;

        public int PageNumber { get; set; } = 1;
    }
}