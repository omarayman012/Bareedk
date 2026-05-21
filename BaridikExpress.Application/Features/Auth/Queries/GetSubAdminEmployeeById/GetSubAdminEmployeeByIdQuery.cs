using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetSubAdminEmployeeById
{
    public class GetSubAdminEmployeeByIdQuery : IRequest<Result<SubAdminEmployeeResponse>>
    {
        public Guid Id { get; set; }
    }
}