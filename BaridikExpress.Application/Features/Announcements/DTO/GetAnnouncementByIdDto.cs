using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Announcements.DTO
{
    public sealed class GetAnnouncementByIdDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; } = default!;
        public LocalizedDto? Description { get; set; } = default!;
        public string? Discount { get; set; }
        public string BackgroundColor { get; set; } = string.Empty; 
        public string TextColor { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}
