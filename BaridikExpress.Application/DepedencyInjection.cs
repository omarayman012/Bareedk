using BaridikExpress.Application.Behaviors;
using BaridikExpress.Application.Common.Mapping;
using BaridikExpress.Application.Features.Auth.Commands.CreateAccount;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
namespace BaridikExpress.Application
{
    public static class DepedencyInjection
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblyContaining(typeof(DepedencyInjection));
            });

            services.AddValidatorsFromAssembly(typeof(RegisterUserCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }


    }
}
