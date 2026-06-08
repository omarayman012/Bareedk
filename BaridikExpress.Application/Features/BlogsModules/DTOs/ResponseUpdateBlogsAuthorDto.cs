using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awael_Al_Joudah.Application.DTO.BlogsAuthor
{
    public class ResponseUpdateBlogsAuthorDto
    {
        public Guid Id { get; set; }

        public LocalizedDto Name { get; set; } = new();

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public LocalizedNameDto BlogsCategory { get; set; }

        public bool IsActive { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
    }
}
