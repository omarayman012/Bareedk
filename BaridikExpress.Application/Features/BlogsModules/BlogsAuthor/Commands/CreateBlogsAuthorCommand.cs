using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands
{
    public class CreateBlogsAuthorCommand
        : IRequest<Result<ResponseBlogsAuthorDto>>
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;

        public UserGender Gender { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }

        public Guid BlogsCategoryId { get; set; }
    }
}