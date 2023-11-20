

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


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> GetAll(int page, int pageSize)
        {
            return _context.Set<T>().Skip((page - 1) * pageSize).Take(pageSize);
        }

        public T? GetById(Guid guid)
        {
            return _context.Set<T>().Find(guid);
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

    }
}