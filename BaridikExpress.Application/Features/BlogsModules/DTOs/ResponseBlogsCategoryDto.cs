using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awael_Al_Joudah.Application.DTO.BlogsCategoryModules
{
    public class ResponseBlogsCategoryDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; }
        public int? Priorty { get; set; }
        public LocalizedDto Description { get; set; } 
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }

    }
}
