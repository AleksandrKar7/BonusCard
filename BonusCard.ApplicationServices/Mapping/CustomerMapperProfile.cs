using AutoMapper;
using BonusCardManager.ApplicationServices.DTOs;
using BonusCardManager.DataAccess.Entities;

namespace BonusCardManager.ApplicationServices.Mapping
{
    class CustomerMapperProfile : Profile
    {
        public CustomerMapperProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
