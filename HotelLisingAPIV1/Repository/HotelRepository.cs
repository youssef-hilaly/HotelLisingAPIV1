using HotelLisingAPIV1.Interfaces;
using HotelLisingAPIV1.Models;

namespace HotelLisingAPIV1.Repository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
    {
        public HotelRepository(AppDbContext context) : base(context)
        {
        }
    }
}
