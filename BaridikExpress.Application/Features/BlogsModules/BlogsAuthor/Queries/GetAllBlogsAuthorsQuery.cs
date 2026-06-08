
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Queries
{
    public class GetAllBlogsAuthorsQuery
        : IRequest<Result<PaginatedList<GetAllBlogsAuthorDto>>>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public Guid? CategoryId { get; set; }
        public string? CreatedById { get; set; }
        public UserGender? Gender { get; set; }
        public bool? IsActive { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int PageNumber { get; set; } = 1;
        public int? pageSize { get; set; }
    }
}