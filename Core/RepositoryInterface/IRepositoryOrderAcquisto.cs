using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryOrderAcquisto : IRepository<OrdineAcquisto>
    {
        string CercaOrdineAcquisto(string titolo);
        List<OrdineAcquisto> GetAllOrdineAcquisto();
        OrdineAcquisto GetById(int id);
    }
}
 