using AutoMapper;
using PTO_Manager.DTOs;
using PTO_Manager.Entities;

namespace PTO_Manager.Additional;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        /*
         *
         * 
        CreateMap<User, UserCreateDto>().ReverseMap()
            .ForMember(dest=> dest.Password, opt=>opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)))//Encrypts the password
            ; 
        
         */

        CreateMap<User, Request>().ReverseMap();
        CreateMap<User, UserRegisterDto>().ReverseMap()
            .ForMember(dest => dest.FennmaradoNapok, opt => opt.Ignore())
            .ForMember(dest => dest.Jelszo, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Jelszo)));

        CreateMap<SpecialDays, SpecialDaysAddDto>().ReverseMap();
        CreateMap<SpecialDays, SpecialDaysGetDto>().ReverseMap();
    }
}