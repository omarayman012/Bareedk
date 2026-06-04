using AutoMapper;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;
using BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;

namespace BaridikExpress.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region Country

        CreateMap<CreateCountryCommand, Domain.Entities.Location.Country>()
            .ForMember(dest => dest.CountryNameAr,
                opt => opt.MapFrom(src => src.NameAr))
            .ForMember(dest => dest.CountryNameEn,
                opt => opt.MapFrom(src => src.NameEn))
            .ForMember(dest => dest.PhoneCode,
                opt => opt.MapFrom(src => src.PhoneCode));

        CreateMap<Domain.Entities.Location.Country, CreateCountryResponse>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.CountryId))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => new LocalizedDto
                {
                    AR = src.CountryNameAr,
                    EN = src.CountryNameEn
                }))
                .ForMember(dest => dest.PhoneCode,
                opt => opt.MapFrom(src => src.PhoneCode))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src =>
                    src.CreatedBy != null
                        ? src.CreatedBy.UserName ?? src.CreatedById
                        : src.CreatedById))
            .ForMember(dest => dest.UpdatedBy,
                opt => opt.MapFrom(src =>
                    src.UpdatedBy != null
                        ? src.UpdatedBy.UserName ?? src.UpdatedById
                        : src.UpdatedById));

        #endregion

        #region Government

        CreateMap<CreateGovernmentCommand, Domain.Entities.Location.Government>()
            .ForMember(dest => dest.GovernmentNameAr,
                opt => opt.MapFrom(src => src.NameAr))
            .ForMember(dest => dest.GovernmentNameEn,
                opt => opt.MapFrom(src => src.NameEn));

        CreateMap<Domain.Entities.Location.Government, GovernmentDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.GovernmentId))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => new LocalizedDto
                {
                    AR = src.GovernmentNameAr,
                    EN = src.GovernmentNameEn
                }))
            .ForMember(dest => dest.Country,
                opt => opt.MapFrom(src => new LocalizedNameDto
                {
                    Id = src.Country!.CountryId,
                    AR = src.Country.CountryNameAr,
                    EN = src.Country.CountryNameEn
                }))
            .ForMember(dest => dest.CreatedBy,
                opt => opt.MapFrom(src =>
                    src.CreatedBy != null
                        ? src.CreatedBy.UserName ?? src.CreatedById
                        : src.CreatedById))
            .ForMember(dest => dest.UpdatedBy,
                opt => opt.MapFrom(src =>
                    src.UpdatedBy != null
                        ? src.UpdatedBy.UserName ?? src.UpdatedById
                        : src.UpdatedById));

        #endregion
    }
}