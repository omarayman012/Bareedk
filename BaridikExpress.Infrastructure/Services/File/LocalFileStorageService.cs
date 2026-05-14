using BaridikExpress.Application.Interfaces.File;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (fileStream is null || fileStream.Length == 0)
                return null;

            var ext = Path.GetExtension(fileName);
            var uniqueId = Guid.NewGuid();
            var uniqueFileName = $"{uniqueId}{ext}";
            var folderForUrl = folderName.Trim().Trim('/').Replace('\\', '/');

            var candidateRoots = UploadsPathResolver.GetCandidateUploadRoots(_env);

            string? fullPath = null;
            foreach (var root in candidateRoots)
            {
                try
                {
                    var uploadsPath = Path.Combine(root, folderName);
                    Directory.CreateDirectory(uploadsPath);
                    fullPath = Path.Combine(uploadsPath, uniqueFileName);
                    break;
                }
                catch
                {
                    // Try next candidate root (e.g., fallback to wwwroot).
                }
            }

            if (string.IsNullOrWhiteSpace(fullPath))
                return null;

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await fileStream.CopyToAsync(stream);
            }

            var relative = $"{folderForUrl}/{uniqueFileName}";
            var baseUrl = _baseUrlService.GetBaseUrl().TrimEnd('/');
            if (string.IsNullOrEmpty(baseUrl))
                return $"/{relative}";

            return $"{baseUrl}/{relative}";
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
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
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

