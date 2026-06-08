using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands
{
    public class UpdateBlogsCategoryCommand
        : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }

        public string? NameAr { get; set; }
        public string? NameEn { get; set; }

        public int? Priorty { get; set; }

        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public IFormFile? Image { get; set; }

        public bool? IsActive { get; set; }
    }
}