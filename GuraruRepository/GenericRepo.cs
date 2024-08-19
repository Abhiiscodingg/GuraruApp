using GuraruRepository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuraruRepository
{
    public class GenericRepo<T> : IGenericRepository<T> where T : class
    {
        private DbContext _dbContext { get; set; }
        public GenericRepo(DbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public bool Delete(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return true;
        }

        public T Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }
    }
}
