using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepository<T>
    {
        //operazioni CRUD
        List<T> GetAll();
        T Add(T item);
        T Update(T item);
        bool Delete(T item);
    }
}
