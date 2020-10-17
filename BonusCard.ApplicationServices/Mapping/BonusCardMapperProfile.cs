using AutoMapper;
using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.DataAccess.Entities;

namespace BonusCardManager.ApplicationServices.Mapping
{
    public class BonusCardMapperProfile : Profile
    {
        public BonusCardMapperProfile()
        {
            CreateMap<BonusCard, BonusCardDto>()
                .ForMember(dto => dto.CustomerId, o => o.MapFrom(x => x.Customer.Id))
                .ForMember(dto => dto.CustomerFullName, o => o.MapFrom(x => x.Customer.FullName))
                .ForMember(dto => dto.CustomerPhoneNumber, o => o.MapFrom(x => x.Customer.PhoneNumber))
                .ReverseMap();
        }
    }
}
