using BaridikExpress.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;

public class UpdateBlogsAuthorCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }

    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
    public UserGender? Gender { get; set; }

    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }

    public Guid? BlogsCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public IFormFile? Image { get; set; }
}