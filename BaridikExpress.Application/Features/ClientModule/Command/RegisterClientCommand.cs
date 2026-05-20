using BaridikExpress.Application.Features.ClientModule.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Command
{
    public class RegisterClientCommand : IRequest<Result<RegisterClientResponseDto>>
    {
        // معلومات المستخدم الأساسية
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        // معلومات العميل
        public Guid CareerFieldId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLink { get; set; }

        // الموافقات
        public bool AcceptTerms { get; set; }
        public bool AcceptPrivacy { get; set; }
    }
}
