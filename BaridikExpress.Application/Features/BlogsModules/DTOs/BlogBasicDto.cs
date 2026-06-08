using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class BlogBasicDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
    }
}
