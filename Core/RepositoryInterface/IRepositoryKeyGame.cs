using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryKeyGame : IRepository<KeyGame>
    {
        string CercaKeyGame(string titolo);
        List<KeyGame> GetAllKeyGame();
        KeyGame GetById(int id);

        string RicercaUnKeyGame(string titolo);
    }
}
