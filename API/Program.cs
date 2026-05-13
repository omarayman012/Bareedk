using Api;
using BaridikExpress.API.Extensions;
using BaridikExpress.API.Middlewares;
using BaridikExpress.Application;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Infrastructure;
using BaridikExpress.Infrastructure.Persistence;
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

var app = builder.Build();
await app.InitializeAsync();


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
    app.UseSwaggerUI();
}
app.MapGet("/", () => Results.Redirect("/swagger"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
