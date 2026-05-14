using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.File
{
    public interface IFileStorageService
    {
        Task<string?> SaveFileAsync(Stream fileStream, string fileName, string folderName);
        Task DeleteFileAsync(string? filePath);
        Task<string?> UpdateFileAsync(Stream fileStream, string fileName, string? oldFilePath, string folderName);
    }
}
