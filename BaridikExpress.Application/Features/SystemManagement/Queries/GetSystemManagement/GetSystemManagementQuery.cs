using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Domain.Entities.SystemManagment;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetSystemManagement;

public sealed class GetSystemManagementQuery<T> : IRequest<Result<SystemManagementResponse>>
    where T : BaseSystemManagementEntity;