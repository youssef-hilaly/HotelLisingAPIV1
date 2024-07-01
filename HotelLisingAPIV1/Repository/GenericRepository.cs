using HotelLisingAPIV1.Interfaces;
using HotelLisingAPIV1.Models;
using HotelListingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelLisingAPIV1.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await GetAsync(id) != null;
        }

        // only pagination
        public async virtual Task<List<T>> GetAllAsync(QueryParameters queryParameters)
        {
            int skip = (queryParameters.PageNumber - 1) * queryParameters.PageSize;
            int take = queryParameters.PageSize;
            //Select, Filter, OrderBy, SearchTerm

            return await _context.Set<T>().Skip(skip).Take(take).ToListAsync();
        }

        //public Task<List<T>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public virtual async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
