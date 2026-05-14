using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler(
        UserManager<User> userManager,
        IMemoryCache memoryCache,
        IApplicationDbContext context,
        IStringLocalizer localizer,
        IHasherService hasher
    ) : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IApplicationDbContext _context = context;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly IHasherService _hasher = hasher;

        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Result<bool>.Failure(_localizer["UserNotfound"], 404);

            var cacheKey = $"ResetToken_{request.Email}";
            var cachedToken = _memoryCache.Get<string>(cacheKey);

            if (cachedToken == null)
                return Result<bool>.Failure(_localizer["Expiredtoken"], 400);

            if (!_hasher.Verify(cachedToken, request.Token))
                return Result<bool>.Failure(_localizer["Invalidtoken"], 400);


            var identityToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, identityToken, request.NewPassword);

            if (!result.Succeeded)
                return Result<bool>.Failure(_localizer["Weakpassword"], 400);
            _memoryCache.Remove(cacheKey); 

            var tokens = _context.RefreshTokens.Where(x => x.UserId == user.Id);
            _context.RefreshTokens.RemoveRange(tokens);

            await _context.SaveChangesAsync(cancellationToken);

            return Result<bool>.Success(true, _localizer["Passwordresetsuccessfully"]);
        }
    }
}