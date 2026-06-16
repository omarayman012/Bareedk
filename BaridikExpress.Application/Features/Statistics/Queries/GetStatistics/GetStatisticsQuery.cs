using BaridikExpress.Application.Features.Statistics.DTO;
using BaridikExpress.Domain.Enums;
using MediatR;

namespace BaridikExpress.Application.Features.Statistics.Queries.GetStatistics;

public record GetStatisticsQuery(StatisticsFilter Filter) : IRequest<Result <StatisticsDto>>;