```csharp
using Api;
using BaridikExpress.API.Extensions;
using BaridikExpress.API.Middlewares;
using BaridikExpress.Application;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces;
using BaridikExpress.Application.Interfaces.Auth;
using BaridikExpress.Infrastructure;
using BaridikExpress.Infrastructure.Data.Seeder.IdentitySeed;
using BaridikExpress.Infrastructure.Data.Seeder.NationalitySeeder;
using BaridikExpress.Infrastructure.Data.Seeder.SystemManagementSeeder;
using BaridikExpress.Infrastructure.Persistence;
using BaridikExpress.Infrastructure.Services.Email;
using BaridikExpress.Infrastructure.Services.SmsService;
using BaridikExpress.Infrastructure.Services.File;
using Infrastructure.Services.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.Extensions.FileProviders;

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
});

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<MailSettings>(
    builder.Configuration.GetSection("MailSettings"));
builder.Services.Configure<TwilioSettings>(
    builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174",
                "http://127.0.0.1:5173",
                "http://127.0.0.1:5174",
                "https://admin.baridikexpress.com"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    await NationalitySeeder.SeedAsync(dbContext);
    await SystemManagementSeeder.SeedAsync(dbContext);
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

await app.InitializeAsync();

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

app.UseCors("AllowFrontend");

app.UseStaticFiles();

var candidateRoots = UploadsPathResolver.GetCandidateUploadRoots(app.Environment);

foreach (var root in candidateRoots)
{
    if (!Directory.Exists(root)) continue;
    if (string.Equals(root, app.Environment.WebRootPath, StringComparison.OrdinalIgnoreCase)) continue;

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(root),
        RequestPath = ""
    });
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/auth-v1/swagger.json", "Auth API V1");
    options.SwaggerEndpoint("/swagger/admin-v1/swagger.json", "Admin API V1");
    options.SwaggerEndpoint("/swagger/client-v1/swagger.json", "Client API V1");
    options.SwaggerEndpoint("/swagger/delivery-v1/swagger.json", "Delivery API V1");
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
```
