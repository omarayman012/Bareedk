using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Import;

public sealed class ImportServiceBusinessPlansCommand
: IRequest<Result<string>>
{
    public IFormFile File { get; set; } = null!;
}
