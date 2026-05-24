using Api;
using BaridikExpress.API.Extensions;
using BaridikExpress.API.Middlewares;
using BaridikExpress.Application;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Infrastructure;
using BaridikExpress.Infrastructure.Data.Seeder.NationalitySeeder;
using BaridikExpress.Infrastructure.Persistence;
using BaridikExpress.Infrastructure.Services.Email;
using BaridikExpress.Infrastructure.Services.SmsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var message = context.ModelState.GetFirstErrorMessage();
        var body = Result<object>.Failure(message, 400);
        return new BadRequestObjectResult(body);
    };
});

// Add services to the container.
builder.Services.AddAPIDependencies(builder.Configuration);
builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddDataProtection();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("auth-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auth API",
        Version = "v1"
    });

    options.SwaggerDoc("admin-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Admin Dashboard API",
        Version = "v1"
    });

    options.SwaggerDoc("client-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Client API",
        Version = "v1"
    });

    options.SwaggerDoc("delivery-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Delivery API",
        Version = "v1"
    });

    options.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.GroupName == docName;
    });
    options.SwaggerDoc("location-geography-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Location Geography API",
        Version = "v1"
    });

    options.SwaggerDoc("role-management-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Role Management API",
        Version = "v1"
    });


    options.SwaggerDoc("select-menu-v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Select Menu API",
        Version = "v1"
    });
});
builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IEmailService, EmailService>();


var app = builder.Build();
await app.InitializeAsync();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await NationalitySeeder.SeedAsync(dbContext);
}
// Localization configuration
var supportedCultures = new[] { "en", "ar" };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
    RequestCultureProviders = new[]
    {
        new AcceptLanguageHeaderRequestCultureProvider()
    }
});

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/auth-v1/swagger.json", "Auth API V1");

        options.SwaggerEndpoint("/swagger/admin-v1/swagger.json", "Admin API V1");

        options.SwaggerEndpoint("/swagger/client-v1/swagger.json", "Client API V1");

        options.SwaggerEndpoint("/swagger/delivery-v1/swagger.json", "Delivery API V1");
        options.SwaggerEndpoint(
    "/swagger/location-geography-v1/swagger.json",
    "Location Geography API V1");

        options.SwaggerEndpoint(
            "/swagger/role-management-v1/swagger.json",
            "Role Management API V1");



        options.SwaggerEndpoint(
            "/swagger/select-menu-v1/swagger.json",
            "Select Menu API V1");

    });
}

app.MapGet("/", () => Results.Redirect("/swagger"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();