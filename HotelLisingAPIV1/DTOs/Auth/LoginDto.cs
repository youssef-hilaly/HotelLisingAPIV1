using System.ComponentModel.DataAnnotations;

namespace HotelLisingAPIV1.DTOs.Login
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string Password { get; set; }

    }
}
