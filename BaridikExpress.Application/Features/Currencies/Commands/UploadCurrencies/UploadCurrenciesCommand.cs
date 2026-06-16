using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.CurrencyModule;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Currencies.Commands.UploadCurrencies;

public record UploadCurrenciesCommand(IFormFile File) : IRequest<Result<ExcelUploadResult<Currency>>>;