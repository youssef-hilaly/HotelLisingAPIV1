using Microsoft.AspNetCore.Identity;

namespace HotelLisingAPIV1.Models
{
    public class ApiUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
