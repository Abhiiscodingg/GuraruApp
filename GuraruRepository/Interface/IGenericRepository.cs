using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuraruRepository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        public T Add(T entity);
        public T Update(T entity);
        public bool Delete(T entity);
    }
}
