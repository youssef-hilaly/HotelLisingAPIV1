using HotelLisingAPIV1.DTOs.Login;
using System.ComponentModel.DataAnnotations;

namespace HotelLisingAPIV1.DTOs.Auth
{
    public class ApiUserDto : LoginDto
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
    }
}
