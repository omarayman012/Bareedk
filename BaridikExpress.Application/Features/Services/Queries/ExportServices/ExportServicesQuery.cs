using MediatR;

namespace BaridikExpress.Application.Features.Services.Queries.ExportServices;

public sealed record ExportServicesQuery : IRequest<byte[]>;