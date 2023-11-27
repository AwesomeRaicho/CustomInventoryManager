using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IRepository<T> where T : class
    {
        public  Task<T?> GetById(Guid guid);
        public Task<IEnumerable<T>> GetAll(int page, int pageSize);
        public  Task Add(T entity);
        public  Task Update(T entity);
        public  Task Delete(T entity);
    }
}
