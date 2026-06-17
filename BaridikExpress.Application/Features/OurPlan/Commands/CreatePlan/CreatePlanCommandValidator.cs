using BaridikExpress.Domain.Enum;
using FluentValidation;

namespace BaridikExpress.Application.Features.OurPlans.Commands.CreatePlan;

public sealed class CreatePlanCommandValidator
    : AbstractValidator<CreatePlanCommand>
{
    public CreatePlanCommandValidator(IStringLocalizer localizer)
    {
        RuleFor(x => x)
         .Must(x =>
             !string.IsNullOrWhiteSpace(x.NameEn) ||
             !string.IsNullOrWhiteSpace(x.NameAr)
         )
          .WithMessage(localizer["PlanNameRequired"]);

        RuleFor(x => x.Type)
        .IsInEnum()
        .NotEqual(PlanType.None)
        .WithMessage(localizer["PlanTypeRequired"]);


        RuleFor(x => x)
            .Must(x =>
                (x.FeaturesAr is not null && x.FeaturesAr.Any()) ||
                (x.FeaturesEn is not null && x.FeaturesEn.Any()))
            .WithMessage(localizer["FeaturesRequired"]);
    }
}