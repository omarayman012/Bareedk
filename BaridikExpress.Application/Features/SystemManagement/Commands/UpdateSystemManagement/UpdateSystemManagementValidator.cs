using BaridikExpress.Domain.Entities.SystemManagment;
using FluentValidation;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;

public sealed class UpdateSystemManagementValidator<T>
    : AbstractValidator<UpdateSystemManagementCommand<T>>
    where T : BaseSystemManagementEntity
{
    public UpdateSystemManagementValidator(IStringLocalizer localizer)
    {
        #region Description (if sent)

        When(x => x.DescriptionAr is not null, () =>
        {
            RuleFor(x => x.DescriptionAr)
                .MaximumLength(5000).WithMessage(localizer["DescriptionArMaxLength"]);
        });

        When(x => x.DescriptionEn is not null, () =>
        {
            RuleFor(x => x.DescriptionEn)
                .MaximumLength(5000).WithMessage(localizer["DescriptionEnMaxLength"]);
        });

        #endregion
    }
}