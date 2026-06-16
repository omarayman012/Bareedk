using BaridikExpress.Application.Common.Abstractions;
using MediatR;

namespace BaridikExpress.Application.Features.Services.Queries.DownloadServiceTemplate;

public sealed record DownloadServiceTemplateQuery : IRequest<byte[]>;