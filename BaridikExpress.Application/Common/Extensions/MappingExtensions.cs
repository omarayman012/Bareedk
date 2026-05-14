using AutoMapper;

namespace Applicationons;

public static class MappingExtensions
{
    public static string Localize(this ResolutionContext context, string ar, string en)
    {
        var lang = context.Items.ContainsKey("lang")
            ? context.Items["lang"]?.ToString()
            : "en";

        return lang == "ar" ? ar! : en!;
    }
}
