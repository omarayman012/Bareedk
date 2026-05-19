using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetAllSubAdminEmployees
{
    public class GetAllSubAdminEmployeesQuery : BaseFilter, IRequest<Result<PaginatedList<SubAdminEmployeeResponse>>>
    {
        public string? Name { get; set; }
    }
}