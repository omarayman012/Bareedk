using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.DTOs;

public sealed record ServiceBusinessPlanResponse(
Guid Id,
LocalizedDto Name,
LocalizedDto Description,
string? Image,
bool IsActive,
string? CreatedBy,
DateTime CreatedAt,
string? UpdatedBy,
DateTime? UpdatedAt
);
