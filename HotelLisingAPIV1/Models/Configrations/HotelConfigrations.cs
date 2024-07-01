using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelLisingAPIV1.Models.Configrations
{
    public class HotelConfigrations : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                   new Hotel{ Id = 1, Name = "Marriot", Address = "Islamabad", Rating = 4.5, CountryId = 1},
                   new Hotel { Id = 2, Name = "Hilton", Address = "Karachi", Rating = 4.0, CountryId = 1 }, 
                   new Hotel { Id = 3, Name = "Hyatt", Address = "Lahore", Rating = 4.3, CountryId = 1 }, 
                   new Hotel { Id = 4, Name = "Holiday Inn", Address = "New York", Rating = 4.5, CountryId = 2 }, 
                   new Hotel { Id = 5, Name = "Marriot", Address = "London", Rating = 4.0, CountryId = 3 }
                   );
        }
    }
}
