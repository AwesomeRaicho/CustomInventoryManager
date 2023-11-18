using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IRepository<T> where T : class
    {
        public T? GetById(Guid guid);
        public IEnumerable<T> GetAll(int page, int pageSize);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
