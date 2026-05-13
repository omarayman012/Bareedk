using BaridikExpress.Application.Commands.AuthModules;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Handlers.AuthModules
{
    public class ResendConfirmationEmailCommandHandler(
        UserManager<User> userManager,
        IMemoryCache memoryCache,
        IEmailService emailService,
        IHasherService hasher,
        IStringLocalizer localizer
    ) : IRequestHandler<ResendConfirmationEmailCommand, Result<bool>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IEmailService _emailService = emailService;
        private readonly IHasherService _hasher = hasher;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<bool>> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<bool>.Failure(_localizer["UserNotFound"], 404);

            if (user.EmailConfirmed)
                return Result<bool>.Failure(_localizer["EmailAlreadyConfirmed"], 400);

            var otp = new Random().Next(100000, 999999).ToString();
            var hashedOtp = _hasher.Hash(otp);
            var cacheKey = $"EmailOTP_{request.Email}";
            _memoryCache.Set(cacheKey, hashedOtp, TimeSpan.FromMinutes(5));

            await _emailService.SendConfirmationEmail(user, otp );
            return Result<bool>.Success(true, _localizer["OTPResent"]);
        }
    }
}