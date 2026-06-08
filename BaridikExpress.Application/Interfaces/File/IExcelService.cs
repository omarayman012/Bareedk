using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.File
{
    public interface IExcelService
    {
        Task<byte[]> DownloadTemplateAsync<T>() where T : class, new();
        Task<byte[]> DownloadDataAsync<T>(IEnumerable<T> data) where T : class;
      // Task<ExcelUploadResult<T>> UploadAsync<T>(IFormFile file) where T : class, new();
        Task<ExcelUploadResult<TEntity>> UploadAsync<TExcel, TEntity>(
                IFormFile file,
                Func<TExcel, TEntity> mapper,
                Func<TEntity, Task<bool>>? existsChecker = null,
                Func<TEntity, string>? inFileKeySelector = null,
                CancellationToken cancellationToken = default)
                where TExcel : class, new()
                where TEntity : class;

            
    }
}

