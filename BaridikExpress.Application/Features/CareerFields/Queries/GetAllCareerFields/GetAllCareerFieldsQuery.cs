using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.CareerFields.DTOs;

namespace BaridikExpress.Application.Features.CareerFields.Queries.GetAllCareerFields;

public class GetAllCareerFieldsQuery
    : BaseFilter,
      IRequest<Result<PaginatedList<GetAllCareerFieldsDto>>>
{
    public string? Name { get; set; }
}