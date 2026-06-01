using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Infrastructure.Services.File;

public class BaseUrlService(IHttpContextAccessor httpContextAccessor) : IBaseUrlService
{
    public string GetBaseUrl()
    {
        var request = httpContextAccessor.HttpContext?.Request;
        if (request == null) return string.Empty;

        var scheme = request.Headers["X-Forwarded-Proto"].FirstOrDefault()
                     ?? request.Scheme;

        var host = request.Headers["X-Forwarded-Host"].FirstOrDefault()
                   ?? request.Host.Value;

        return $"{scheme}://{host}";
    }

    public string ToAbsoluteMediaUrl(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return path ?? string.Empty;

        var p = path.Trim();

        if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return p;

        var baseUrl = GetBaseUrl().TrimEnd('/');
        var rel = p.StartsWith('/') ? p : "/" + p;

        return string.IsNullOrEmpty(baseUrl) ? rel : $"{baseUrl}{rel}";
    }
}