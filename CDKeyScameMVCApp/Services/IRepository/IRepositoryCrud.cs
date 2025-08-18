using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CDKeyScameMVCApp.Services.IRepository
{
    public interface IRepositoryCrud<T>
    {
        IEnumerable<T> GetAll();
        T GetById(Guid id);
        Guid Create(T item);
        bool Update(T item);
        bool Delete(Guid id);
    }
} 
