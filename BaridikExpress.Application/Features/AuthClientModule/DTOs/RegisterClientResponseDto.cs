using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.DTOs
{
    public class RegisterClientResponseDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public LocalizedDto CareerFieldName { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLink { get; set; }
        public bool AcceptTerms { get; set; }
        public bool AcceptPrivacy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; } = string.Empty;

    }
}
