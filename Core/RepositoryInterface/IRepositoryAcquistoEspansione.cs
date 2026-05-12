using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryAcquistoEspansione : IRepository<OrderAcquistoEspansione>
    {
        string CercaOrderAcquistoEspansione(string titolo);
        List<OrderAcquistoEspansione> GetAllOrderAcquistoEspansione();
        OrderAcquistoEspansione GetById(int id);

        string RicercaUnOrderAcquistoEspansione(string titolo);
    }
}
