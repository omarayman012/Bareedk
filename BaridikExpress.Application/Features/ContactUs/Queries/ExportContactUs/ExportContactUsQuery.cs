namespace BaridikExpress.Application.Features.ContactUs.Queries.ExportContactUs;

public sealed record ExportContactUsQuery(
    string? Search,
    bool? IsRead
) : IRequest<byte[]>;