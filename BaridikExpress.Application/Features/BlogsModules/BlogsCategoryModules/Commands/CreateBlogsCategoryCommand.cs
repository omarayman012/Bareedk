using Awael_Al_Joudah.Application.DTO.BlogsCategoryModules;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands
{
    public class CreateBlogsCategoryCommand : IRequest<Result<ResponseBlogsCategoryDto>>

    {

        public string NameAr { get; set; } = string.Empty;

        public string NameEn { get; set; } = string.Empty;

        public int? Priorty { get; set; }

        public string? DescriptionAr { get; set; } 

        public string? DescriptionEn { get; set; } 

        public IFormFile Image { get; set; } 
        public bool IsActive { get; set; } = true;
    }

}
