namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.UpdateGovernment;

public class UpdateGovernmentCommand:IRequest<Result<bool>>
{
    public Guid Id { get; set; }
    public Guid?CountryId { get; set; }
    public string? NameAr { get; set; }
    public string? NameEn { get; set; }
}
