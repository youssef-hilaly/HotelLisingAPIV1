using System.ComponentModel.DataAnnotations;

namespace HotelLisingAPIV1.DTOs.Countries
{
    public class CountryDto : BaseCountryDto
    {
        [Required]
        public int Id {  get; set; }
    }
}
