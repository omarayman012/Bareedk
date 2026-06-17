
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.OurPlans.Commands.CreatePlan;
public record CreatePlanCommand(
 string NameAr,
 PlanType Type,
 string NameEn,
 List<string> FeaturesAr,
 List<string> FeaturesEn,
 string? DescriptionAr,
 string? DescriptionEn) : IRequest<Result<Guid>>;
