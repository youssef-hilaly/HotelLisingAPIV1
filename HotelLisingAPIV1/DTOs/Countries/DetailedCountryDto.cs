using HotelLisingAPIV1.DTOs.Hotels;
using HotelLisingAPIV1.Models;

namespace HotelLisingAPIV1.DTOs.Countries
{
    public class DetailedCountryDto: CountryDto
    {
        public List<HotelDto> Hotels {  get; set; }
    }
}
