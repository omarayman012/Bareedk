using AutoMapper;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateCountryCommand, Domain.Entities.Location.Country>()
            .ForMember(dest => dest.CountryNameAr, opt => opt.MapFrom(src => src.NameAr))
            .ForMember(dest => dest.CountryNameEn, opt => opt.MapFrom(src => src.NameEn));

        CreateMap<Domain.Entities.Location.Country, CreateCountryResponse>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => new LocalizedDto
                {
                    AR = src.CountryNameAr,
                    EN = src.CountryNameEn
                }))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src => src.CreatedBy != null
                    ? src.CreatedBy.UserName ?? src.CreatedById
                    : src.CreatedById))
            .ForMember(dest => dest.UpdatedBy,
                opt => opt.MapFrom(src => src.UpdatedBy != null
                    ? src.UpdatedBy.UserName ?? src.UpdatedById
                    : src.UpdatedById));
    }
}