using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace BaridikExpress.Infrastructure.Services.File
{
    public class ExcelService : IExcelService
    {
        private readonly ApplicationDbContext _context;

        public ExcelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> DownloadTemplateAsync<T>() where T : class, new()
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(typeof(T).Name);

            var entityType = _context.Model.FindEntityType(typeof(T));
            var properties = GetExcelProperties<T>(entityType);

            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }
            using (var range = worksheet.Cells[1, 1, 1, properties.Length])
            {
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);

                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


            }

            worksheet.Row(1).Height = 10;
            worksheet.Cells.AutoFitColumns();
            return await package.GetAsByteArrayAsync();
        }

        public async Task<byte[]> DownloadDataAsync<T>(IEnumerable<T> data) where T : class
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(typeof(T).Name);

            var entityType = _context.Model.FindEntityType(typeof(T));
            var properties = GetExcelProperties<T>(entityType);

            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
            }

            int row = 2;
            foreach (var item in data)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row, col + 1].Value = properties[col].GetValue(item);
                }
                row++;
            }

            using (var range = worksheet.Cells[1, 1, 1, properties.Length])
            {
                range.Style.Font.Color.SetColor(Color.White);
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(color: Color.Black);

                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            }

            worksheet.Row(1).Height = 10;
            worksheet.Cells.AutoFitColumns();

            return await package.GetAsByteArrayAsync();
        }

        //public async Task<ExcelUploadResult<T>> UploadAsync<T>(IFormFile file)
        // where T : class, new()
        //{
        //    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

        //    var uploaded = new List<T>();
        //    var skippedRows = new List<int>();
        //    var skippedDetails = new List<ExcelSkippedRowDetail>();
        //    var totalRows = 0;
        //    var seenKeysInFile = new HashSet<string>(StringComparer.Ordinal);

        //    using var stream = new MemoryStream();
        //    await file.CopyToAsync(stream);
        //    using var package = new ExcelPackage(stream);

        //    var worksheet = package.Workbook.Worksheets.First();
        //    var entityType = _context.Model.FindEntityType(typeof(T));
        //    var properties = GetExcelProperties<T>(entityType);
        //    var uniqueKeyOptions = GetUniqueKeyOptions(entityType, properties.Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase));
        //    uniqueKeyOptions = AddFallbackNameKeyOptions<T>(uniqueKeyOptions, properties.Select(p => p.Name).ToHashSet(StringComparer.OrdinalIgnoreCase));

        //    // قراءة الهيدر
        //    var headers = new Dictionary<int, string>();
        //    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
        //    {
        //        var header = worksheet.Cells[1, col].Text?.Trim();
        //        if (!string.IsNullOrEmpty(header))
        //            headers[col] = header;
        //    }

        //    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        //    {
        //        var entity = new T();
        //        totalRows++;

        //        foreach (var header in headers)
        //        {
        //            var prop = properties.FirstOrDefault(p => p.Name == header.Value);

        //            if (prop == null)
        //                continue;

        //            var cellValue = worksheet.Cells[row, header.Key].Value;
        //            if (cellValue == null) continue;

        //            try
        //            {
        //                var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
        //                object safeValue;

        //                if (targetType == typeof(string))
        //                    safeValue = cellValue.ToString();
        //                else if (targetType.IsEnum)
        //                    safeValue = Enum.Parse(targetType, cellValue.ToString(), true);
        //                else if (targetType == typeof(DateTime))
        //                {
        //                    if (DateTime.TryParse(cellValue.ToString(), out var dt))
        //                        safeValue = dt;
        //                    else
        //                        safeValue = DateTime.Now;
        //                }
        //                else
        //                    safeValue = Convert.ChangeType(cellValue, targetType);

        //                prop.SetValue(entity, safeValue);
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception($"Invalid value '{cellValue}' for column '{header.Value}' at row {row}", ex);
        //            }
        //        }

        //        // pick the first unique key option that has values for this row
        //        var keyProps = PickKeyPropsForEntity(entity, uniqueKeyOptions);
        //        if (keyProps != null)
        //        {
        //            // Skip duplicates within the same uploaded Excel file first
        //            var keySignature = BuildKeySignature(entity, keyProps);
        //            if (!string.IsNullOrEmpty(keySignature))
        //            {
        //                if (!seenKeysInFile.Add(keySignature))
        //                {
        //                    skippedRows.Add(row);
        //                    skippedDetails.Add(new ExcelSkippedRowDetail
        //                    {
        //                        RowNumber = row,
        //                        Reason = ExcelSkipReason.DuplicateInFile,
        //                        KeyColumns = keyProps.ToList(),
        //                        KeyValues = ExtractKeyValues(entity, keyProps)
        //                    });
        //                    continue;
        //                }
        //            }

        //            var exists = await ExistsAsync(entity, keyProps);
        //            if (exists)
        //            {
        //                skippedRows.Add(row);
        //                skippedDetails.Add(new ExcelSkippedRowDetail
        //                {
        //                    RowNumber = row,
        //                    Reason = ExcelSkipReason.ExistsInDatabase,
        //                    KeyColumns = keyProps.ToList(),
        //                    KeyValues = ExtractKeyValues(entity, keyProps)
        //                });
        //                continue;
        //            }
        //        }

        //        uploaded.Add(entity);
        //    }

        //    if (uploaded.Count > 0)
        //    {
        //        _context.Set<T>().AddRange(uploaded);
        //        await _context.SaveChangesAsync();
        //    }

        //    return new ExcelUploadResult<T>
        //    {
        //        Message = $"تم رفع عدد {uploaded.Count} ولم يتم رفع عدد {skippedRows.Count}",
        //        TotalRows = totalRows,
        //        UploadedCount = uploaded.Count,
        //        SkippedCount = skippedRows.Count,
        //        SkippedRows = skippedRows,
        //        SkippedDetails = skippedDetails,
        //        UploadedItems = uploaded
        //    };
        //}


        public async Task<ExcelUploadResult<TEntity>> UploadAsync<TExcel, TEntity>(
          IFormFile file,
          Func<TExcel, TEntity> mapper,
          Func<TEntity, Task<bool>>? existsChecker = null,  
          Func<TEntity, string>? inFileKeySelector = null,  
          CancellationToken cancellationToken = default)
          where TExcel : class, new()
          where TEntity : class
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var uploaded = new List<TEntity>();
            var skippedRows = new List<int>();
            var skippedDetails = new List<ExcelSkippedRowDetail>();
            var seenKeysInFile = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var totalRows = 0;

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.First();

            var excelProperties = typeof(TExcel)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .ToArray();

            // ── Headers ──────────────────────────────────────────────
            var headers = new Dictionary<int, string>();
            for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            {
                var header = worksheet.Cells[1, col].Text?.Trim();
                if (!string.IsNullOrWhiteSpace(header))
                    headers[col] = header;
            }

            // ── Rows ─────────────────────────────────────────────────
            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                totalRows++;
                var excelDto = new TExcel();

                foreach (var header in headers)
                {
                    var prop = excelProperties.FirstOrDefault(p =>
                        p.Name.Equals(header.Value, StringComparison.OrdinalIgnoreCase));
                    if (prop is null) continue;

                    var cellValue = worksheet.Cells[row, header.Key].Value;
                    if (cellValue is null) continue;

                    try
                    {
                        var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                        object safeValue;

                        if (targetType == typeof(string))
                            safeValue = cellValue.ToString()!;
                        else if (targetType.IsEnum)
                            safeValue = Enum.Parse(targetType, cellValue.ToString()!, true);
                        else if (targetType == typeof(DateTime))
                            safeValue = DateTime.TryParse(cellValue.ToString(), out var dt) ? dt : DateTime.UtcNow;
                        else
                            safeValue = Convert.ChangeType(cellValue, targetType);

                        prop.SetValue(excelDto, safeValue);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            $"Invalid value '{cellValue}' for column '{header.Value}' at row {row}", ex);
                    }
                }

                // ── Map to Entity ─────────────────────────────────────
                var entity = mapper(excelDto);

                // ── In-File Duplicate Check ───────────────────────────
                if (inFileKeySelector is not null)
                {
                    var key = inFileKeySelector(entity)?.Trim().ToLowerInvariant();
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        if (!seenKeysInFile.Add(key))
                        {
                            skippedRows.Add(row);
                            skippedDetails.Add(new ExcelSkippedRowDetail
                            {
                                RowNumber = row,
                                Reason = ExcelSkipReason.DuplicateInFile,
                                KeyColumns = new List<string> { key },
                                KeyValues = new Dictionary<string, object?> { ["key"] = key }
                            });
                            continue;
                        }
                    }
                }

                // ── Database Duplicate Check ──────────────────────────
                if (existsChecker is not null && await existsChecker(entity))
                {
                    skippedRows.Add(row);
                    skippedDetails.Add(new ExcelSkippedRowDetail
                    {
                        RowNumber = row,
                        Reason = ExcelSkipReason.ExistsInDatabase,
                        KeyColumns = new List<string>(),
                        KeyValues = new Dictionary<string, object?>()
                    });
                    continue;
                }

                uploaded.Add(entity);
            }

            if (uploaded.Count > 0)
            {
                await _context.Set<TEntity>().AddRangeAsync(uploaded, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new ExcelUploadResult<TEntity>
            {
                Message = $"تم رفع عدد {uploaded.Count} ولم يتم رفع عدد {skippedRows.Count}",
                TotalRows = totalRows,
                UploadedCount = uploaded.Count,
                SkippedCount = skippedRows.Count,
                SkippedRows = skippedRows,
                SkippedDetails = skippedDetails,
                UploadedItems = uploaded
            };
        }

        private static PropertyInfo[] GetExcelProperties<TEntity>(Microsoft.EntityFrameworkCore.Metadata.IEntityType? entityType)
        {
            var primaryKeyNames = entityType?.FindPrimaryKey()?.Properties
                .Select(p => p.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase)
                ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Id" };

            return typeof(TEntity)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite)
                .Where(p => !Attribute.IsDefined(p, typeof(KeyAttribute)))
                .Where(p => !primaryKeyNames.Contains(p.Name)) // don't allow setting PKs from Excel
                .Where(p => IsExcelScalarType(p.PropertyType))
                .ToArray();
        }

        private static bool IsExcelScalarType(Type type)
        {
            var t = Nullable.GetUnderlyingType(type) ?? type;

            if (t.IsEnum) return true;
            if (t == typeof(string)) return true;
            if (t == typeof(Guid)) return true;
            if (t == typeof(bool)) return true;
            if (t == typeof(DateTime)) return true;
            if (t == typeof(DateTimeOffset)) return true;
            if (t == typeof(decimal)) return true;

            if (t.IsPrimitive) return true; // int, double, etc.

            return false;
        }

        private static List<string[]> GetUniqueKeyOptions(
            Microsoft.EntityFrameworkCore.Metadata.IEntityType? entityType,
            HashSet<string> excelPropertyNames)
        {
            if (entityType == null) return new List<string[]>();

            // Prefer smaller unique indexes (single-column first).
            return entityType.GetIndexes()
                .Where(i => i.IsUnique)
                .Select(i => i.Properties.Select(p => p.Name).ToArray())
                .Where(props => props.Length > 0 && props.All(p => excelPropertyNames.Contains(p)))
                .OrderBy(props => props.Length)
                .ToList();
        }

        private static List<string[]> AddFallbackNameKeyOptions<TEntity>(
            List<string[]> keyOptions,
            HashSet<string> excelPropertyNames)
        {
            // If the entity doesn't have unique indexes (or the Excel sheet doesn't include them),
            // still allow a "Name"-based uniqueness check when name columns are present.
            var candidates = new[] { "Name", "NameEn", "NameAr" };

            var fallback = candidates
                .Where(n => excelPropertyNames.Contains(n))
                .Where(n => typeof(TEntity).GetProperty(n, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase) != null)
                .Select(n => new[] { n })
                .ToList();

            if (fallback.Count == 0) return keyOptions;

            var existing = new HashSet<string>(
                keyOptions.Select(o => string.Join("|", o.Select(x => x.ToLowerInvariant()))),
                StringComparer.Ordinal);

            foreach (var option in fallback)
            {
                var sig = string.Join("|", option.Select(x => x.ToLowerInvariant()));
                if (existing.Add(sig))
                    keyOptions.Add(option);
            }

            return keyOptions.OrderBy(o => o.Length).ToList();
        }

        private static string[]? PickKeyPropsForEntity<TEntity>(TEntity entity, List<string[]> keyOptions)
        {
            if (keyOptions.Count == 0) return null;

            foreach (var option in keyOptions)
            {
                var ok = true;
                foreach (var propName in option)
                {
                    var prop = typeof(TEntity).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (prop == null) { ok = false; break; }

                    var value = prop.GetValue(entity);
                    if (value == null) { ok = false; break; }
                    if (value is string s && string.IsNullOrWhiteSpace(s)) { ok = false; break; }
                }

                if (ok) return option;
            }

            return null;
        }

        private async Task<bool> ExistsAsync<TEntity>(TEntity entity, string[] keyProps) where TEntity : class
        {
            // Build: e => e.Prop1 == value1 && e.Prop2 == value2 ...
            var param = Expression.Parameter(typeof(TEntity), "e");
            Expression? body = null;

            foreach (var propName in keyProps)
            {
                var propInfo = typeof(TEntity).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo == null) return false;

                var propType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;
                var valueObj = propInfo.GetValue(entity);
                if (valueObj == null) return false;

                var constant = Expression.Constant(valueObj, propInfo.PropertyType);
                var member = Expression.Property(param, propInfo);

                // Normalize string comparison (trim) so it matches typical uniqueness expectations.
                Expression equalsExpr;
                if (propType == typeof(string))
                {
                    var trimMethod = typeof(string).GetMethod(nameof(string.Trim), Type.EmptyTypes)!;
                    var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;

                    var leftTrim = Expression.Call(member, trimMethod);
                    var rightTrim = Expression.Call(Expression.Convert(constant, typeof(string)), trimMethod);

                    var left = Expression.Call(leftTrim, toLowerMethod);
                    var right = Expression.Call(rightTrim, toLowerMethod);

                    equalsExpr = Expression.Equal(left, right);
                }
                else
                {
                    equalsExpr = Expression.Equal(member, constant);
                }

                body = body == null ? equalsExpr : Expression.AndAlso(body, equalsExpr);
            }

            if (body == null) return false;

            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            return await _context.Set<TEntity>().AsNoTracking().AnyAsync(lambda);
        }

        private static Dictionary<string, object?> ExtractKeyValues<TEntity>(TEntity entity, string[] keyProps)
        {
            var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            foreach (var propName in keyProps)
            {
                var propInfo = typeof(TEntity).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo == null) continue;
                dict[propInfo.Name] = propInfo.GetValue(entity);
            }
            return dict;
        }

        private static string BuildKeySignature<TEntity>(TEntity entity, string[] keyProps)
        {
            // Stable string representation to detect duplicates inside the file.
            // Normalize strings by trimming and lower-casing.
            var parts = new List<string>(capacity: keyProps.Length);
            foreach (var propName in keyProps)
            {
                var propInfo = typeof(TEntity).GetProperty(propName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo == null) return string.Empty;
                var value = propInfo.GetValue(entity);
                if (value == null) return string.Empty;

                string normalized;
                if (value is string s)
                    normalized = s.Trim().ToLowerInvariant();
                else if (value is DateTime dt)
                    normalized = dt.ToUniversalTime().ToString("O");
                else if (value is DateTimeOffset dto)
                    normalized = dto.ToUniversalTime().ToString("O");
                else
                    normalized = value.ToString() ?? string.Empty;

                parts.Add($"{propInfo.Name}={normalized}");
            }

            return string.Join("|", parts);
        }

    }
}
