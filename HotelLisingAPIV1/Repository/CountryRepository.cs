using HotelLisingAPIV1.Interfaces;
using HotelLisingAPIV1.Models;
using HotelListingAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Linq.Expressions;

namespace HotelLisingAPIV1.Repository
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<Country> GetAsync(int id)
        {
            return await _context.Countries.Include(c => c.Hotels).FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<List<Country>> GetAllAsync(QueryParameters queryParameters)
        {
            int skip = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int take = queryParameters.PageSize;

            string searchTerm = queryParameters.SearchTerm;

            string orderCol = queryParameters.OrderBy;

            string sortOrder = queryParameters.SortOrder;

            IQueryable<Country> query = _context.Countries;

            // Serch
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.Name.Contains(searchTerm));
            }


            // Order By
            if (!string.IsNullOrEmpty(orderCol))
            {
                if (sortOrder == "desc")
                {
                    query = query.OrderByDescending(GetSortProperty(orderCol));
                }
                else
                {
                    query = query.OrderBy(GetSortProperty(orderCol));
                }
            }


            return await query.Skip(skip).Take(take).ToListAsync();
        }

        private static Expression<Func<Country, object>> GetSortProperty(String orderCol)
        {
            return orderCol.ToLower() switch
            {
                "name" => country => country.Name,
                "shortname" => country => country.ShortName,
                _ => country => country.Id
            };
        }
    }
}
