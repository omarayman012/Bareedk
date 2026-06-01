using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Domain.Entities.SystemManagment;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;

public sealed class UpdateSystemManagementCommand<T> : IRequest<Result<SystemManagementResponse>>
    where T : BaseSystemManagementEntity
{
    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }
}