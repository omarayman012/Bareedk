using BaridikExpress.Application.Interfaces.File;
using Infrastructure.Services.File;
using Microsoft.AspNetCore.Hosting;

namespace BaridikExpress.Infrastructure.Services.File
{

    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IBaseUrlService _baseUrlService;


        public LocalFileStorageService(IWebHostEnvironment env, IBaseUrlService baseUrlService)
        {
            _env = env;
            _baseUrlService = baseUrlService;
        }

        public async Task<string?> SaveFileAsync(Stream fileStream, string fileName, string folderName)
        {
            try
            {
                if (fileStream == null)
                    return null;

                var ext = Path.GetExtension(fileName);
                var uniqueFileName = $"{Guid.NewGuid()}{ext}";

                var candidateRoots = UploadsPathResolver.GetCandidateUploadRoots(_env);

                foreach (var root in candidateRoots)
                {
                    try
                    {
                        var uploadsPath = Path.Combine(root, folderName);

                        Directory.CreateDirectory(uploadsPath);

                        var fullPath = Path.Combine(uploadsPath, uniqueFileName);

                        fileStream.Position = 0; // 🔥 مهم جدًا

                        using var stream = new FileStream(fullPath, FileMode.Create);
                        await fileStream.CopyToAsync(stream);

                        var relative = $"{folderName.Replace("\\", "/")}/{uniqueFileName}";
                        var baseUrl = _baseUrlService.GetBaseUrl()?.TrimEnd('/');

                        return string.IsNullOrEmpty(baseUrl)
                            ? $"/{relative}"
                            : $"{baseUrl}/{relative}";
                    }
                    catch
                    {
                        continue; // جرب root تاني
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public Task DeleteFileAsync(string? filePath)
        {
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                var relativePath = ExtractRelativePath(filePath);
                var candidateRoots = UploadsPathResolver.GetCandidateUploadRoots(_env);

                foreach (var root in candidateRoots)
                {
                    var fullPath = Path.Combine(root, relativePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        break;
                    }
                }
            }
            return Task.CompletedTask;
        }

        public async Task<string?> UpdateFileAsync(Stream fileStream, string fileName, string? oldFilePath, string folderName)
        {
            await DeleteFileAsync(oldFilePath);
            return await SaveFileAsync(fileStream, fileName, folderName);
        }

        private string ExtractRelativePath(string? filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            if (filePath.StartsWith("http"))
            {
                var uri = new Uri(filePath);
                var p = uri.AbsolutePath.TrimStart('/');
                // Accept both "/uploads/{folder}/{file}" and legacy "/{folder}/{file}"
                if (p.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
                    p = p.Substring("uploads/".Length);
                return p;
            }

            var raw = filePath.TrimStart('/');
            if (raw.StartsWith("uploads/", StringComparison.OrdinalIgnoreCase))
                raw = raw.Substring("uploads/".Length);
            return raw;
        }
    }
}

