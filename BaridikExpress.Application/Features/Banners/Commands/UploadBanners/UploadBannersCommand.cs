using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Commands.UploadBanners
{
    public record UploadBannersCommand(IFormFile File) 
        : IRequest<Result<ExcelUploadResult<Banner>>>;
}
