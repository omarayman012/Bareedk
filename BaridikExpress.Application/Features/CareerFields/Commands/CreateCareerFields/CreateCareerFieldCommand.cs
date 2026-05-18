namespace BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields
{
    public record CreateCareerFieldCommand(
            string NameAr ,
            string NameEn 
   ) : IRequest<Result<Guid?>>;
}
