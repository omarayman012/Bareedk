using BaridikExpress.Application.Features.Auth.DTO.User;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Domain.Entities.AuthModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateAccount
{
    public class RegisterUserCommandHandler(UserManager<User> userManager
        ,IApplicationDbContext context
        ,IStringLocalizer localizer
          , IMemoryCache memoryCache
        , IEmailService emailService
            , IHasherService hasher
         ) : IRequestHandler<RegisterUserCommand, Result<UserResponseDto>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IApplicationDbContext _context = context;
        private readonly IStringLocalizer _localizer = localizer;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly IEmailService _emailService = emailService;
        private readonly IHasherService _hasher = hasher;

        public async Task<Result<UserResponseDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
           var userExists =await _userManager.FindByEmailAsync(request.Email);
            if(userExists != null) {
                return Result<UserResponseDto>.Failure(_localizer[ "Useralreadyexists"], 400);
            }
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.Phone,
                UserName = request.Email 
            };

             var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {

                //TODO assign default role to user after Seeding Roles in Database

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    var error = roleResult.Errors.Select(e => e.Description).First();
                    return Result<UserResponseDto>.Failure(_localizer["Userregistrationfailed"] + ": " + error, 400);
                }
                var otp = new Random().Next(100000, 999999).ToString();
                var hashedOtp = _hasher.Hash(otp);
                _memoryCache.Set($"EmailOTP_{request.Email}", hashedOtp, TimeSpan.FromMinutes(5));
                await _emailService.SendConfirmationEmail(user, otp);

                var userResponse = new UserResponseDto(user.Id, user.FullName, user.Email, user.EmailConfirmed);
                return Result<UserResponseDto>.Success(userResponse, _localizer["Userregisteredsuccessfully"], 201);
            }
            else
            {
                var error = result.Errors.Select(e => e.Description).First();
                return Result<UserResponseDto>.Failure(_localizer["Userregistrationfailed"] + ": " + error, 400);
            }
        }
    }
}
