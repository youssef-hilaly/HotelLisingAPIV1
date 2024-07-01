using System.ComponentModel.DataAnnotations;

namespace HotelLisingAPIV1.DTOs.Countries
{
    public class BaseCountryDto
    {
        [Required]
        public string Name {  get; set; }

        public string ShortName {  get; set; }
    }
}
