namespace BaridikExpress.Application.Common.Helpers
{

    public static class NormalizeHelper
    {
        public static (string NameAr, string NameEn) Normalize(string? nameAr, string? nameEn)
        {
            nameAr = nameAr?.Trim();
            nameEn = nameEn?.Trim();

            if (string.IsNullOrWhiteSpace(nameAr) && !string.IsNullOrWhiteSpace(nameEn))
            {
                nameAr = nameEn;
            }
            else if (!string.IsNullOrWhiteSpace(nameAr) && string.IsNullOrWhiteSpace(nameEn))
            {
                nameEn = nameAr;
            }

            return (nameAr!, nameEn!);
        }
    }

    }

