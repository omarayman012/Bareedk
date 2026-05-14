namespace BaridikExpress.Domain.Entities.ValueObjects
{
    public class LocalizedString
    {
        public string En { get; private set; } = string.Empty;
        public string Ar { get; private set; } = string.Empty;

        private LocalizedString() { }

        private LocalizedString(string en, string ar)
        {
            En = en;
            Ar = ar;
        }

        public static LocalizedString Create(string? en, string? ar)
        {
            if (string.IsNullOrWhiteSpace(en) && string.IsNullOrWhiteSpace(ar))
                throw new ArgumentException(LocalizationKeys.Required);

            var finalEn = string.IsNullOrWhiteSpace(en) ? ar! : en!;
            var finalAr = string.IsNullOrWhiteSpace(ar) ? en! : ar!;

            return new LocalizedString(finalEn.Trim(), finalAr.Trim());
        }

        public override string ToString()
        {
            return En; 
        }

    }
}
