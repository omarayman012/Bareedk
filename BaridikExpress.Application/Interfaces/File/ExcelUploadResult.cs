using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.File
{
    public sealed class ExcelUploadResult<T>
    {
        public string Message { get; init; } = string.Empty;
        public int TotalRows { get; init; }
        public int UploadedCount { get; init; }
        public int SkippedCount { get; init; }
        public List<int> SkippedRows { get; init; } = new();
        public List<ExcelSkippedRowDetail> SkippedDetails { get; init; } = new();
        public List<T> UploadedItems { get; init; } = new();
    }
    public enum ExcelSkipReason
    {
        ExistsInDatabase = 1,
        DuplicateInFile = 2,
        NoUniqueKeyFound = 3
    }

    public sealed class ExcelSkippedRowDetail
    {
        public int RowNumber { get; init; }
        public ExcelSkipReason Reason { get; init; }

        /// <summary>
        /// الأعمدة التي تم استخدامها كمفتاح فريد (Unique Key) لاكتشاف التكرار.
        /// </summary>
        public List<string> KeyColumns { get; init; } = new();

        /// <summary>
        /// قيم الأعمدة التي تم استخدامها كمفتاح فريد لاكتشاف التكرار.
        /// </summary>
        public Dictionary<string, object?> KeyValues { get; init; } = new();
    }




}
