using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models;


namespace IKEA.DAL.Presistance.Repositories._Generic
{
    public interface IGenericRepository<T>where T:ModelBase
    {
       Task< IEnumerable<T>> GetAllAsync(bool WithAsNoTracking = true);
        IQueryable<T> GetAllAsQuerable();
       Task< T?> GetByIdAsync(int id);
        void Add(T entity);
        void Update(T entity);
       void Delete(T entity);
    }
}
