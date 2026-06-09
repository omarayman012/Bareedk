using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Application.Interfaces.BlogModules;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Application.Interfaces.Realtime;
using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Infrastructure.Localizer;
using BaridikExpress.Infrastructure.Persistence;
using BaridikExpress.Infrastructure.Repositories;
using BaridikExpress.Infrastructure.Services.AuthModules;
using BaridikExpress.Infrastructure.Services.BlogModules;
using BaridikExpress.Infrastructure.Services.Email;
using BaridikExpress.Infrastructure.Services.File;
using BaridikExpress.Infrastructure.Services.Hasher;
using BaridikExpress.Infrastructure.Services.Maps;
using BaridikExpress.Infrastructure.Services.Realtime;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Infrastructure
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services
                .AddDatabaseConfig(configuration)
                .AddPersistence()
                .AddLocalizationConfig()
                .AddIdentityConfig();
        }

        // ── Localization ─────────────────────────────
        private static IServiceCollection AddLocalizationConfig(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddScoped<IStringLocalizer, JsonStringLocalizer>();
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IHasherService, HasherService>();
            services.AddScoped<IGetCurrentUserRepository, GetCurrentUserRepository>();
            services.AddScoped<IFileStorageService, LocalFileStorageService>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<IBaseUrlService, BaseUrlService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddHttpClient<IMapService, GoogleGeocodingService>();
            services.AddScoped<ICommentRealtimeService, CommentRealtimeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IBlogService, BlogService>();

            return services;
        }

        private static IServiceCollection AddDatabaseConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.Configure<GoogleMapsOptions>(configuration.GetSection(GoogleMapsOptions.SectionName));

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