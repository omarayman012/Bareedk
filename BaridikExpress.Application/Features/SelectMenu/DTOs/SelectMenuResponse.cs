using BaridikExpress.Application.DTOs;

namespace BaridikExpress.Application.Features.SelectMenu.DTOs
{
    public sealed record SelectMenuResponse(
     Guid Id,
     LocalizeLang? Name
    );
}
