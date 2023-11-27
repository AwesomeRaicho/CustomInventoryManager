

using Entities;
using Microsoft.EntityFrameworkCore;
using ServicesContracts;
using System.Reflection.Metadata.Ecma335;

namespace Repository
{

    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly AllDbContext _context;

        public Repository(AllDbContext context)
        {
            _context = context;
        }


        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll(int page, int pageSize)
        {
            return await _context.Set<T>().Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<T?> GetById(Guid guid)
        {
            return await _context.Set<T>().FindAsync(guid);
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

    }
}