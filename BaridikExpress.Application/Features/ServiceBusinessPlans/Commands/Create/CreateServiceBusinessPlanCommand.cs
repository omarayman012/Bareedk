using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Create;

public sealed class CreateServiceBusinessPlanCommand : IRequest<Result<Guid>>
{
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }

    public string? DescriptionEn { get; set; }
    public string? DescriptionAr { get; set; }

    public IFormFile? Image { get; set; }
}