using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Application.Features.OurPlans.Commands.UploadPlans;

public record UploadPlansCommand(IFormFile File)
    : IRequest<Result<ExcelUploadResult<Plan>>>;