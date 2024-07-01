using AutoMapper;
using HotelLisingAPIV1.DTOs.Auth;
using HotelLisingAPIV1.DTOs.Countries;
using HotelLisingAPIV1.DTOs.Hotels;
using HotelLisingAPIV1.Models;

namespace HotelLisingAPIV1.DTOs
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, BaseCountryDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, DetailedCountryDto>().ReverseMap();

            CreateMap<Hotel, BaseHotelDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();

            CreateMap<ApiUser,ApiUserDto>().ReverseMap();
        }
    }
}
