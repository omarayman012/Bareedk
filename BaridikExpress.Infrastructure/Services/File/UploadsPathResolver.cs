using Microsoft.AspNetCore.Hosting;

namespace Infrastructure.Services.File;

/// <summary>
/// يحدد مجلداً للرفوعات لا يُمسّه <c>dotnet publish</c> ولا <c>git clean</c> داخل مجلد الريبو.
/// </summary>
public static class UploadsPathResolver
{
    public const string UploadsRootEnvVar = "UPLOADS_ROOT";
    private const string LinuxDefaultUploadsRoot = "/var/www/shared/uploads";

    /// <summary>
    /// مجلد بجانب مجلد الريبو (أب لـ <c>app</c>) — مثال: .../backend-api.../api-media بينما الريبو في .../app
    /// </summary>
    public const string SiblingMediaFolderName = "api-media";

    /// <summary>
    /// ترتيب المحاولات: متغير البيئة، ثم مجلد ثابت بجانب النشر، ثم المسار الافتراضي على لينكس، ثم wwwroot.
    /// </summary>
    public static IReadOnlyList<string> GetCandidateUploadRoots(IWebHostEnvironment env)
    {
        var roots = new List<string>(capacity: 5);
        void TryAdd(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return;
            string full;
            try
            {
                full = Path.GetFullPath(path);
            }
            catch
            {
                return;
            }

            if (roots.Any(r => string.Equals(r, full, StringComparison.OrdinalIgnoreCase)))
                return;
            roots.Add(full);
        }

        TryAdd(Environment.GetEnvironmentVariable(UploadsRootEnvVar));
        TryAdd(TryGetSiblingOfGitRepositoryMediaRoot(env.ContentRootPath));
        if (OperatingSystem.IsLinux())
            TryAdd(LinuxDefaultUploadsRoot);
        TryAdd(env.WebRootPath);
        return roots;
    }

    /// <summary>
    /// عند التشغيل من <c>.../app/publish</c> يرجع <c>.../api-media</c> (شقيق لمجلد <c>app</c>).
    /// </summary>
    public static string? TryGetSiblingOfGitRepositoryMediaRoot(string contentRootPath)
    {
        try
        {
            var publishRoot = Path.GetFullPath(contentRootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            if (!string.Equals(Path.GetFileName(publishRoot), "publish", StringComparison.OrdinalIgnoreCase))
                return null;

            var appRepositoryRoot = Directory.GetParent(publishRoot)?.FullName;
            if (string.IsNullOrEmpty(appRepositoryRoot))
                return null;

            var siteRoot = Directory.GetParent(appRepositoryRoot)?.FullName;
            if (string.IsNullOrEmpty(siteRoot))
                return null;

            return Path.GetFullPath(Path.Combine(siteRoot, SiblingMediaFolderName));
        }
        catch
        {
            return null;
        }
    }
}
