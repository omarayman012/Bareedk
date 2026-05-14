using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Infrastructure.Localizer;
using BaridikExpress.Infrastructure.Persistence;
using BaridikExpress.Infrastructure.Repositories;
using BaridikExpress.Infrastructure.Services.AuthModules;
using BaridikExpress.Infrastructure.Services.Email;
using BaridikExpress.Infrastructure.Services.File;
using BaridikExpress.Infrastructure.Services.Hasher;
using Infrastructure.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using ServiceStack.Auth;

namespace BaridikExpress.Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(
    configuration.GetSection("MailSettings"));
            return services
                .AddDatabaseConfig(configuration)
                .AddPersistence()
                    .AddLocalizationConfig()
                .AddIdentityConfig();


        }
        // ── Localization ─────────────────────────────
        private static IServiceCollection AddLocalizationConfig(
            this IServiceCollection services)
        {
            services.AddLocalization();

            services.AddScoped<IStringLocalizer, JsonStringLocalizer>();

            return services;
        }



        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
           // services.AddScoped<IUnitOfWork>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService , EmailService>();
            services.AddScoped<IApplicationDbContext>(provider =>
                 provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IHasherService, HasherService>();
            services.AddScoped<IGetCurrentUserRepository, GetCurrentUserRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IBaseUrlService, BaseUrlService>();



            return services;
        }
        private static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {

            services.AddIdentityCore<User>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();
            return services;
        }

           }
}
