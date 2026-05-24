using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.ClientModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthClientModule.Handler
{
    public class RegisterClientHandler
        : IRequestHandler<RegisterClientCommand, Result<RegisterClientResponseDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;

        public RegisterClientHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IStringLocalizer localizer)
        {
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<RegisterClientResponseDto>> Handle(
            RegisterClientCommand request,
            CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.Users
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailExists)
                return Result<RegisterClientResponseDto>.Failure(
                    _localizer["EmailAlreadyExists"], 400);

            var phoneExists = await _userManager.Users
                .AnyAsync(u => u.PhoneNumber == request.Phone, cancellationToken);

            if (phoneExists)
                return Result<RegisterClientResponseDto>.Failure(
                    _localizer["PhoneAlreadyExists"], 400);

            var careerField = await _context.CareerFields
                .FirstOrDefaultAsync(cf => cf.Id == request.CareerFieldId, cancellationToken);

            if (careerField == null)
                return Result<RegisterClientResponseDto>.Failure(
                    _localizer["CareerFieldNotFound"], 400);

            var user = new User
            {
                FullName = request.FullName,
                UserName = request.Email,
                Email = request.Email,
                PhoneNumber = request.Phone,
                EmailConfirmed = false
            };

            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (!createUserResult.Succeeded)
            {
                var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                return Result<RegisterClientResponseDto>.Failure(errors, 400);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Client");

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return Result<RegisterClientResponseDto>.Failure(errors, 400);
            }

            var client = new Client
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CareerFieldId = request.CareerFieldId,
                CompanyName = request.CompanyName,
                CompanyLink = request.CompanyLink,
                AcceptTerms = request.AcceptTerms,
                AcceptPrivacy = request.AcceptPrivacy
            };

            await _context.Clients.AddAsync(client, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
           

            var userRoles = await _userManager.GetRolesAsync(user);
            var role = userRoles.FirstOrDefault() ?? "Client";

            // ================= RESPONSE =================
            var response = new RegisterClientResponseDto
            {
                Id = client.UserId,
                FullName = user.FullName,
                Email = user.Email!,
                Phone = user.PhoneNumber!,

                CareerFieldName = new LocalizedDto
                {
                    AR = careerField.Name.Ar,
                    EN = careerField.Name.En
                },

                CompanyName = client.CompanyName,
                CompanyLink = client.CompanyLink,
                AcceptTerms = client.AcceptTerms,
                AcceptPrivacy = client.AcceptPrivacy,
                CreatedAt = client.CreatedAt,
                Role = role
            };

            return Result<RegisterClientResponseDto>.Success(
                response,
                _localizer["ClientCreatedSuccessfully"],
                201);
        }
    }
} 