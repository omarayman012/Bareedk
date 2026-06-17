
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.OurPlans.Commands.UpdatePlan;
public record UpdatePlanCommand(
 Guid Id,
 string? NameAr,
 string? NameEn,
 PlanType? Type,
 List<string>? FeaturesAr,
 List<string>? FeaturesEn,
 string? DescriptionAr,
 string? DescriptionEn) : IRequest<Result<Guid>>;
