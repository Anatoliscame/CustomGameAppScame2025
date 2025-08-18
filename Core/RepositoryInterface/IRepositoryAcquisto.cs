using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryAcquisto : IRepository<Acquisto>
    {
        string CercaAcquisto(string titolo);
        List<Acquisto> GetAllAcquisto();
        Acquisto GetById(int id);

        string RicercaUnAcquisto(string titolo);
    }
}
