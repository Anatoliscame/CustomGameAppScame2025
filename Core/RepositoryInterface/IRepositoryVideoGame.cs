using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RepositoryInterface
{
    public interface IRepositoryVideoGame : IRepository<VideoGame>
    {
        string CercaVideoGame(string titolo);
        List<VideoGame> GetAllVideoGame();
        VideoGame GetById(int id);
        string RicercaUnVideoGame(string titolo);
    } 
}
