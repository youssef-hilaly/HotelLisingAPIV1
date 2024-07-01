using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelLisingAPIV1.Models.Configrations
{
    public class CountryConfigrations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country { Id = 1, Name = "Pakistan", ShortName = "PK"},
                new Country { Id = 2, Name = "United States of America", ShortName = "USA"},
                new Country{ Id = 3, Name = "United Kingdom", ShortName = "UK"}
                );
        }
    }
}
