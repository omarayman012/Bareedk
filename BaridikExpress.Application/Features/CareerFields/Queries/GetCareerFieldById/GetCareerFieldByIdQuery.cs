using BaridikExpress.Application.Features.CareerFields.DTO;

namespace BaridikExpress.Application.Features.CareerFields.Queries.GetCareerFieldById;

public record GetCareerFieldByIdQuery(
    Guid Id
) : IRequest<Result<GetCareerFieldByIdDto>>;