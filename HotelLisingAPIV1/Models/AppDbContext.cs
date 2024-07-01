using HotelLisingAPIV1.Models.Configrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelLisingAPIV1.Models
{
    public class AppDbContext: IdentityDbContext<ApiUser> // Not DbContext To apply user identity
    {

        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new HotelConfigrations());
            modelBuilder.ApplyConfiguration(new CountryConfigrations());
            modelBuilder.ApplyConfiguration(new RoleConfigrations());
        }
    }
}
