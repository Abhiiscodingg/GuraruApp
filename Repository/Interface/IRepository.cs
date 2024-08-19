using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IRepository<T> where T : class
    {
        public T GetRecord(int id);
        public IEnumerable<T> GetRecords();
        public bool DeleteRecord(int id);
        public bool CreateRecord(T record);
        public bool UpdateRecord(T record);
    }
}
