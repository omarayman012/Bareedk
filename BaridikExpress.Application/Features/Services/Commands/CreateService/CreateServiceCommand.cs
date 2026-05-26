using BaridikExpress.Application.Features.Services.DTOs;

namespace BaridikExpress.Application.Features.Services.Commands.CreateService;

public sealed record CreateServiceCommand(
    string? NameEn,
    string? NameAr,
    decimal Price,
    IFormFile? Image)
    : IRequest<Result<ServiceResponse>>;