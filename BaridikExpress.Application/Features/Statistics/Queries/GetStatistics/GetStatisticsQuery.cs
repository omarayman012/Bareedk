using BaridikExpress.Application.Features.Statistics.DTO;
using MediatR;

namespace BaridikExpress.Application.Features.Statistics.Queries.GetStatistics;

public record GetStatisticsQuery : IRequest<StatisticsDto>;