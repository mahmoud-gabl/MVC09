using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.DAL.Models;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Data;
using Microsoft.EntityFrameworkCore;

namespace IKEA.DAL.Presistance.Repositories._Generic
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public  async Task<IEnumerable<T>> GetAllAsync(bool WithAsNoTracking = true)
        {
            if (WithAsNoTracking)
            {
                return  await _dbContext.Set<T>().Where(X=>!X.IsDeleted).AsNoTracking().ToListAsync();
            }
            return  await _dbContext.Set<T>().Where(X => !X.IsDeleted).ToListAsync();
        }

        public  async Task<T?> GetByIdAsync(int id)
        {
            return  await _dbContext.Set<T>().FindAsync(id);
        }
        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
          
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
          

        }
        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            // _dbContext.Set<T>().Remove(entity);
            _dbContext.Set<T>().Update(entity);
           
        }

        public IQueryable<T> GetAllAsQuerable()
        {
            return _dbContext.Set<T>();
        }

    }
}
