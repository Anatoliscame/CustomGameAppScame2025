using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BusinessLogic
{
    interface IBusinessLayer
    {
        List<Account> FetchAccountB(Func<Account, bool> filter = null);

        VideoGame CercaVideoGameB(string videogame);

        //User Login(string username, string password);

        List<VideoGame> GetVideoGames();
        VideoGame GetVideoGameId(int id);
        VideoGame InsertVideoGameId(VideoGame v);
        VideoGame UpdateVideoGame(VideoGame v);
        void DeleteVideoGame(int id);

        void VisualizzaStoricoAcquisti();

    }
}
