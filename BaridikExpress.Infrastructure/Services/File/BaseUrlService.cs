
using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.File
{
    public class BaseUrlService : IBaseUrlService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseUrlService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return string.Empty;

            var host = request.Host.Value;
            var scheme = request.Scheme;
            return $"{scheme}://{host}";
        }

        public string ToAbsoluteMediaUrl(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path ?? string.Empty;

            var p = path.Trim();
            if (p.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                p.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return p;

            var baseUrl = GetBaseUrl().TrimEnd('/');
            var rel = p.StartsWith('/') ? p : "/" + p;
            if (string.IsNullOrEmpty(baseUrl))
                return rel;

            return $"{baseUrl}{rel}";
        }
    }
}
