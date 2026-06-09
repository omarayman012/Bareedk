using BaridikExpress.Application.Behaviors;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Common.Mapping;
using BaridikExpress.Application.Features.Auth.Commands.CreateAccount;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.GenericSelectMenu;
using BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Location;
using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;
using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetSystemManagement;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.NotificationModules;
using BaridikExpress.Domain.Entities.SystemManagment;
using BaridikExpress.Domain.Entities.Vehicles;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaridikExpress.Application;

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

        #region System Management Handlers

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<TermsAndConditions>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<TermsAndConditions>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<PrivacyPolicy>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<PrivacyPolicy>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<ShippingPolicy>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<ShippingPolicy>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<SalesAndPurchasePolicy>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<SalesAndPurchasePolicy>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<Help>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<Help>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<DeliveryDriverRegistrationTerms>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<DeliveryDriverRegistrationTerms>>();

        services.AddTransient<
            IRequestHandler<UpdateSystemManagementCommand<CustomerRegistration>, Result<SystemManagementResponse>>,
            UpdateSystemManagementCommandHandler<CustomerRegistration>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<TermsAndConditions>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<TermsAndConditions>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<PrivacyPolicy>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<PrivacyPolicy>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<ShippingPolicy>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<ShippingPolicy>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<SalesAndPurchasePolicy>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<SalesAndPurchasePolicy>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<Help>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<Help>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<DeliveryDriverRegistrationTerms>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<DeliveryDriverRegistrationTerms>>();

        services.AddTransient<
            IRequestHandler<GetSystemManagementQuery<CustomerRegistration>, Result<SystemManagementResponse>>,
            GetSystemManagementQueryHandler<CustomerRegistration>>();
        services.AddTransient<IRequestHandler<UpdateSystemManagementCommand<MessageNotification>, Result<SystemManagementResponse>>,
           UpdateSystemManagementCommandHandler<MessageNotification>>();
        services.AddTransient<
    IRequestHandler<GetSystemManagementQuery<MessageNotification>, Result<SystemManagementResponse>>,
    GetSystemManagementQueryHandler<MessageNotification>>();

        #endregion

        #region Select Menu Handlers

        services.AddTransient<
            IRequestHandler<GetSelectMenuQuery<Country>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuQueryHandler<Country>>();

        services.AddTransient<
            IRequestHandler<GetSelectMenuQuery<Government>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuQueryHandler<Government>>();

        services.AddTransient<
            IRequestHandler<GetSelectMenuQuery<City>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuQueryHandler<City>>();

        services.AddTransient<
            IRequestHandler<GetSelectMenuQuery<Village>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuQueryHandler<Village>>();

        services.AddTransient<
            IRequestHandler<GetSelectMenubaseQuery<CareerField>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuBaseQueryHandler<CareerField>>();

        services.AddTransient<
            IRequestHandler<GetSelectMenubaseQuery<Vehicle>, Result<IEnumerable<SelectMenuResponse>>>,
            GetSelectMenuBaseQueryHandler<Vehicle>>();
        #endregion
        return services;
    }
}
