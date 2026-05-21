using BaridikExpress.Application.Features.CommanDTO.Localizes;
using ServiceStack;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.Government
{
    public class GovernmentDto
    {
        public Guid Id { get; set; }
        public LocalizedDto? Name { get; set; }
        public LocalizedNameDto? Country { get; set; }
       public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive  { get; set; }   

    }
}
