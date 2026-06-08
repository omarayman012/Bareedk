using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Validators
{
    public class CreatePublishingHouseCommandValidator
        : AbstractValidator<CreatePublishingHouseCommand>
    {
        public CreatePublishingHouseCommandValidator(
            IStringLocalizer localizer)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(localizer["CodeRequired"]);

            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage(localizer["ArabicNameRequired"]);

            RuleFor(x => x.CountryId)
                .NotEmpty()
                .WithMessage(localizer["CountryRequired"]);

            RuleFor(x => x.GovernmentId)
                .NotEmpty()
                .WithMessage(localizer["GovernmentRequired"]);

            RuleFor(x => x.EmailAddress)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.EmailAddress))
                .WithMessage(localizer["InvalidEmail"]);

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+\d{7,15}$")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage(localizer["InvalidPhoneNumber"]);

            // التحقق من وجود الصورة وحجمها وامتدادها
            RuleFor(x => x.Image)
                .NotNull().WithMessage(localizer["LogoRequired"])
                .Must(image => image.Length <= 5 * 1024 * 1024)
                    .WithMessage(localizer["exceeded"])
                .Must(HaveValidExtension)
                    .WithMessage(localizer["InvalidImageExtension"]);
        }

        private bool HaveValidExtension(IFormFile file)
        {
            if (file == null) return false;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

    }
}
