using BaridikExpress.Application.Features.Services.DTOs;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Services.Commands.UpdateService;

public sealed class UpdateServiceCommand : IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public decimal? Price { get; set; }
    public IFormFile? Image { get; set; }
}