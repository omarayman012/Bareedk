using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Services.Commands.CreateService;

public sealed record CreateServiceCommand(
    string? NameEn,
    string? NameAr,
    decimal Price,
    Currency Currency,
    IFormFile? Image)
    : IRequest<Result<ServiceResponse>>;