using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Update;

public sealed class UpdateServiceBusinessPlanCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }

    public string? NameEn { get; set; }

    public string? NameAr { get; set; }

    public string? DescriptionEn { get; set; }

    public string? DescriptionAr { get; set; }

    public IFormFile? Image { get; set; }

}
