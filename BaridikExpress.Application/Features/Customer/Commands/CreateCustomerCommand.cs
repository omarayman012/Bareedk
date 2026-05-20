using BaridikExpress.Application.Features.Customer.Dtos;

namespace BaridikExpress.Application.Features.Customer.Commands;

public record CreateCustomerCommand(
    string Name,
    IFormFile? Image,
    Guid? NationalityId,
    Guid? CareerFieldId,
    List<CreateContactDto> Contacts,
    string Password,
    string ConfirmPassword,
    List<CreateAddressDto>? Addresses,
    CreateAccountDto? Account
) : IRequest<Result<CustomerDetailsResponse>>;
